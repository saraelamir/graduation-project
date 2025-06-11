using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GraduProjj.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using GraduProjj.DTOs;
using Microsoft.AspNetCore.Identity;
using GraduProjj.Mapping;
using GraduProjj.Models;

namespace GraduProjj.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public UserProfileController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


		[HttpPost("upload-profile-picture")]
		public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile file)
		{
			if (file == null || file.Length == 0)
				return BadRequest("No file uploaded.");

			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null)
				return Unauthorized("User ID not found in token.");

			if (!int.TryParse(userIdClaim.Value, out int userId))
				return Unauthorized("Invalid user ID in token.");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
			if (user == null)
				return NotFound("User not found.");

			// التأكد من أن الفولدر موجود داخل wwwroot
			var uploadsFolder = Path.Combine(_environment.WebRootPath, "ProfilePictures");
			if (!Directory.Exists(uploadsFolder))
				Directory.CreateDirectory(uploadsFolder);

			// إنشاء اسم فريد للملف
			var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
			var filePath = Path.Combine(uploadsFolder, uniqueFileName);

			// حفظ الصورة
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			// تخزين المسار داخل قاعدة البيانات بالنسبة للرابط النسبي
			user.ProfilePicturePath = $"/ProfilePictures/{uniqueFileName}";
			await _context.SaveChangesAsync();

			// بناء رابط الصورة الكامل للوصول من الواجهة الأمامية
			var baseUrl = $"{Request.Scheme}://{Request.Host}";
			var fullImageUrl = $"{baseUrl}{user.ProfilePicturePath}";

			return Ok(new
			{
				Message = "Profile picture uploaded successfully.",
				ImageUrl = fullImageUrl
			});
		}


		[HttpGet("profile")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = int.Parse(userIdClaim.Value);

            var user = await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.Country,
                    u.ProfilePicturePath
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound("User not found.");

            // بناء المسار الكامل للصورة
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var fullImageUrl = string.IsNullOrEmpty(user.ProfilePicturePath)
                ? null
                : $"{baseUrl}/{user.ProfilePicturePath.Replace("\\", "/")}";

            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.Country,
                ProfilePictureUrl = fullImageUrl
            });
        }




        [HttpPut("update-profile-picture")]
        public async Task<IActionResult> UpdateProfilePicture([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = int.Parse(userIdClaim.Value);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return NotFound("User not found.");

            // مسار رفع الصورة
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "ProfilePictures");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // اسم الملف يكون فريد باستخدام GUID
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // نسخ الصورة إلى المجلد المحدد
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // تحديث المسار في قاعدة البيانات
            user.ProfilePicturePath = "/ProfilePictures/" + uniqueFileName;
            await _context.SaveChangesAsync();

            // هنا بنرجع المسار الكامل
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var fullImageUrl = baseUrl + user.ProfilePicturePath;

            return Ok(new
            {
                Message = "Profile picture updated successfully.",
                ImageUrl = fullImageUrl
            });
        }


        [HttpDelete("delete-profile-picture")]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = int.Parse(userIdClaim.Value);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return NotFound("User not found.");

            if (!string.IsNullOrWhiteSpace(user.ProfilePicturePath))
            {
                var relativePath = user.ProfilePicturePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
                var filePath = Path.Combine(_environment.WebRootPath, relativePath);

                Console.WriteLine($"Trying to delete file at: {filePath}");

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    Console.WriteLine("File deleted successfully.");
                }
                else
                {
                    Console.WriteLine("File does not exist at path: " + filePath);
                }

                user.ProfilePicturePath = null;
                await _context.SaveChangesAsync();

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var fullImageUrl = $"{baseUrl}/ProfilePictures/default.png";

                return Ok(new { Message = "Profile picture deleted successfully.", ImageUrl = fullImageUrl });
            }

            Console.WriteLine("No profile picture path found in database.");
            return Ok(new { Message = "No profile picture to delete. User has no picture set." });
        }








        [HttpPut("update-name")]
        public async Task<IActionResult> UpdateName([FromBody] UpdateNameDto dto)

        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = int.Parse(userIdClaim.Value);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return NotFound("User not found.");

            if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
                return BadRequest("First name and last name are required.");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Name updated successfully." });
        }




        
        [HttpPut("update-email")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = int.Parse(userIdClaim.Value);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return NotFound("User not found.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Email is required.");

            // ممكن تضيف هنا تحقق من صحة الإيميل إذا حبيت
            user.Email = dto.Email;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Email updated successfully." });
        }




        [HttpPut("update-country")]
        public async Task<IActionResult> UpdateCountry([FromBody] UpdateCountryDto updateCountryDto)
        {
            if (updateCountryDto == null || string.IsNullOrEmpty(updateCountryDto.Country))
                return BadRequest("Country cannot be empty.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = int.Parse(userIdClaim.Value);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return NotFound("User not found.");

            // تحديث البلد في قاعدة البيانات
            user.Country = updateCountryDto.Country;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Country updated successfully.",
                Country = user.Country
            });
        }


        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CurrentPassword) ||
                string.IsNullOrWhiteSpace(dto.NewPassword) ||
                string.IsNullOrWhiteSpace(dto.ConfirmNewPassword))
            {
                return BadRequest("All password fields are required.");
            }

            if (dto.NewPassword != dto.ConfirmNewPassword)
            {
                return BadRequest("New password and confirmation do not match.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = int.Parse(userIdClaim.Value);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return NotFound("User not found.");

            var passwordHasher = new PasswordHasher<object>();

            // تحقق من كلمة المرور الحالية
            var verificationResult = passwordHasher.VerifyHashedPassword(null, user.Password, dto.CurrentPassword);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest("Current password is incorrect.");
            }

            // تشفير كلمة المرور الجديدة
            string hashedNewPassword = passwordHasher.HashPassword(null, dto.NewPassword);
            user.Password = hashedNewPassword;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Password changed successfully." });
        }



		[HttpDelete("delete-account")]
		public async Task<IActionResult> DeleteAccount()
		{
			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null)
				return Unauthorized("User ID not found in token.");

			var userId = int.Parse(userIdClaim.Value);

			var user = await _context.Users
				.Include(u => u.Feedbacks)
				.Include(u => u.Input)
				.Include(u => u.Goals)
					.ThenInclude(g => g.Plans) // ✅ تصحيح هنا
				.FirstOrDefaultAsync(u => u.UserId == userId);

			if (user == null)
				return NotFound("User not found.");

			// حذف البيانات المرتبطة
			_context.Feedbacks.RemoveRange(user.Feedbacks);
			_context.Inputs.RemoveRange(user.Input);
			_context.Plans.RemoveRange(user.Goals.SelectMany(g => g.Plans));
			_context.Goals.RemoveRange(user.Goals);

			// حذف صورة البروفايل من السيرفر إن وجدت
			if (!string.IsNullOrEmpty(user.ProfilePicturePath))
			{
				var filePath = Path.Combine(_environment.WebRootPath, user.ProfilePicturePath.TrimStart('/'));
				if (System.IO.File.Exists(filePath))
					System.IO.File.Delete(filePath);
			}

			// حذف المستخدم
			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			return Ok(new { Message = "Account deleted successfully." });
		}

	}
}
