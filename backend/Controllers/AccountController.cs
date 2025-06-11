using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using GraduProjj.Models;
using GraduProjj.Data;
using GraduProjj.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;
using System.Text;
using GraduProjj.Configurations;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace GraduProjj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailSettings _emailSettings;

        public AccountController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null)
                return BadRequest(new { Message = "Registration data is required" });

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);

            if (existingUser != null)
                return BadRequest(new { Message = "Email already exists" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, registerDto.Password);

            // Generate verification code (string)
            var verificationCode = new Random().Next(100000, 999999).ToString();

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Password = hashedPassword,
                Country = registerDto.Country,
                IsEmailVerified = false,
                EmailVerificationCode = verificationCode
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            SendVerificationEmail(user.Email, verificationCode);

            return Ok(new
            {
                Message = "Please check your email for the verification code.",
                Email = user.Email,
            });
        }


        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] EmailVerificationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest(new { Message = "Verification code is required." });

            // البحث عن المستخدم باستخدام الكود فقط
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.EmailVerificationCode == dto.Code);

            if (user == null)
                return NotFound(new { Message = "Invalid verification code." });

            if (user.IsEmailVerified)
                return BadRequest(new { Message = "Email already verified." });

            // التحقق ناجح، فعل الحساب
            user.IsEmailVerified = true;
            user.EmailVerificationCode = null;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Email verified successfully." });
        }








        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest(new { Message = "Login data is required" });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
                return Unauthorized(new { Message = "Invalid email or password" });

            if (!user.IsEmailVerified)
                return Unauthorized(new { Message = "Please verify your email first." });

            var passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(null, user.Password, loginDto.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { Message = "Invalid email or password" });

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:ExpiryInMinutes", 1000)),
                UserId = user.UserId,
                Email = user.Email
            });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:ExpiryInMinutes", 1000));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:ValidIssuer"],
                audience: _configuration["JwtSettings:ValidAudience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return BadRequest(new { Message = "Email not found" });

            var otp = new Random().Next(100000, 999999).ToString();
            user.OtpCode = otp;
            user.OtpExpiration = DateTime.UtcNow.AddMinutes(10);

            await _context.SaveChangesAsync();

            SendOtpEmail(user.Email, otp);

            return Ok(new { Message = "OTP sent to email" });
        }




        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.OtpCode == dto.OtpCode && u.OtpExpiration > DateTime.UtcNow);

            if (user == null)
                return BadRequest(new { Message = "Invalid or expired code" });

            var passwordHasher = new PasswordHasher<object>();
            user.Password = passwordHasher.HashPassword(null, dto.NewPassword);
            user.OtpCode = null;
            user.OtpExpiration = null;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Password changed successfully" });
        }









        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = payload.GivenName,
                        LastName = payload.FamilyName,
                        Email = payload.Email,
                        IsEmailVerified = true,
                        Password = "",
                        Country = "",
                        ProfilePicturePath = payload.Picture
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }

                var token = GenerateJwtToken(user);

                return Ok(new
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:ExpiryInMinutes", 1000)),
                    UserId = user.UserId,
                    Email = user.Email
                });
            }
            catch (InvalidJwtException)
            {
                return Unauthorized(new { Message = "Invalid Google token." });
            }
        }
    














        private void SendOtpEmail(string toEmail, string otpCode)
        {
            try
            {
                Console.WriteLine("Preparing OTP email...");

                var message = new MailMessage(_emailSettings.FromEmail, toEmail)
                {
                    Subject = "Password Reset OTP Code",
                    Body = $"Your OTP code for password reset is: {otpCode}",
                    IsBodyHtml = false
                };

                Console.WriteLine($"Email message prepared: To={toEmail}, Code={otpCode}");

                var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.FromPassword),
                    EnableSsl = true
                };

                Console.WriteLine($"SMTP client ready: Server={_emailSettings.SmtpServer}, Port={_emailSettings.SmtpPort}");

                smtp.Send(message);

                Console.WriteLine("OTP email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OTP email: {ex.Message}");
                throw; // مهم علشان الـ caller يعرف إن فيه Exception حصل
            }
        }










        private void SendVerificationEmail(string toEmail, string code)
        {
            try
            {
                Console.WriteLine("Preparing to send email...");

                // إعداد الرسالة
                using var message = new MailMessage(_emailSettings.FromEmail, toEmail)
                {
                    Subject = "Email Verification Code",
                    Body = $"Your verification code is: {code}",
                    IsBodyHtml = false
                };

                Console.WriteLine($"Message prepared: Subject={message.Subject}, To={toEmail}");

                // إعداد إعدادات SMTP
                using var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.FromPassword),
                    EnableSsl = true
                };

                Console.WriteLine($"SMTP Client prepared: Server={_emailSettings.SmtpServer}, Port={_emailSettings.SmtpPort}");

                // إرسال الرسالة
                smtp.Send(message);

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                // طباعة الخطأ إلى الكونسول
                Console.WriteLine($"Error sending email: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }







    }
}

