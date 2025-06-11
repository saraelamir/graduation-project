using GraduProjj.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GraduProjj.Models;
using Microsoft.OpenApi.Models;
using GraduProjj;
//using GraduProjj.Services;
using GraduProjj.Configurations;
using Microsoft.Extensions.Options;
using GraduProjj.Interfaces;
using GraduProjj.Repository;
using System.Text.Json;

namespace GraduProjj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ======== Add services to the container ========
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpClient();

            // ======== Swagger Configuration ========
            builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GraduProjj API", Version = "v1" });
    
    // إعدادات المصادقة (التي لديك بالفعل)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

    // تحسين عرض عمليات تحميل الملفات
    c.OperationFilter<FileUploadOperationFilter>();
    
    // إضافة هذه السطور لتحسين دعم multipart
    c.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
});


            // ======== Database Configuration ========
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ======== JWT Configuration ========
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new ArgumentNullException("JWT SecretKey is not configured");
            var validIssuer = jwtSettings["ValidIssuer"] ?? throw new ArgumentNullException("JWT ValidIssuer is not configured");
            var validAudience = jwtSettings["ValidAudience"] ?? throw new ArgumentNullException("JWT ValidAudience is not configured");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
         ValidAudience = builder.Configuration["JwtSettings:ValidAudience"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
     };
 });

            



            builder.Services.AddAuthorization();

            // ======== CORS Configuration ========
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin() // 👈 يسمح لأي origin
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            // ======== Extra Services ========
            builder.Services.AddHttpContextAccessor();
            //builder.Services.AddHttpClient<IMLService, MLService>();


            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<EmailSettings>>().Value);




            builder.Services.AddScoped<IInputRepository, InputRepository>();
            builder.Services.AddScoped<IGoalRepository, GoalRepository>();
            builder.Services.AddScoped<IAiModelRepository, AiModelRepository>();
            builder.Services.AddScoped<IPlanRepository, PlanRepository>();

            builder.Services.AddHttpClient<IAiModelRepository, AiModelRepository>(client =>
            {
                client.BaseAddress = new Uri("https://47ef-34-60-35-229.ngrok-free.app/");
            });

            // In Program.cs

            //   builder.Services.AddScoped<IPlanRepository, PlanRepository>();

            builder.Services.AddControllers();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });




            //builder.Services.AddHttpsRedirection(options =>
            //{
            //    options.HttpsPort = 5001; // نفس البورت اللي في launchSettings.json
            //});




            var app = builder.Build();

            // ======== Middleware Pipeline ========
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            // Remove app.UseHttpsRedirection() since we're using HTTP in development
            app.UseStaticFiles();

            // Request logging (for debugging)
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"➡ Request: {context.Request.Method} {context.Request.Path}");
                await next.Invoke();
                Console.WriteLine($"⬅ Response: {context.Response.StatusCode}");
            });

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowAll"); // استخدام السياسة الجديدة
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            


            // Logging JWT info
            Console.WriteLine($"🔐 JWT Configuration:");
            Console.WriteLine($"- Issuer: {validIssuer}");
            Console.WriteLine($"- Audience: {validAudience}");
            Console.WriteLine($"- SecretKey Length: {secretKey.Length} (partially shown)");

            app.Run();
        }
    }
}
