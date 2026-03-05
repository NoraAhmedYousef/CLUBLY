
using Clubly.Repository.Class;
using Clubly.Repository.Interface;
using Clubly.Repository.Interfaces;
using Clubly.Service.Class;
using Clubly.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SignUp.Data;
using SignUp.Model;
using SignUp.Repository.Class;
using SignUp.Repository.Classes;
using SignUp.Repository.Interface;
using SignUp.Repository.Interfaces;
using SignUp.Service.Class;
using SignUp.Service.Classes;
using SignUp.Service.Interfaces;
using System.Text;
namespace SignUp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // ????????????????? DATABASE ?????????????????
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // ????????????????? JWT AUTH ?????????????????
            var jwtKey = builder.Configuration["Jwt:Key"]!;
            var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
            var jwtAudience = builder.Configuration["Jwt:Audience"]!;

            builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();


            // ????????????????? CORS ?????????????????
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                    policy.WithOrigins(
                            "http://localhost:3000",
                            "http://localhost:4200",
                            "http://localhost:5173",
                            "http://127.0.0.1:5500",
                            "https://yourclub.com"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });


            // ????????????????? CONTROLLERS ?????????????????
            builder.Services.AddControllers();


            // ????????????????? SWAGGER ?????????????????
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Club Management API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT — Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {{
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        },
        Array.Empty<string>()
    }});
            });


            // ????????????????? PASSWORD HASHER ?????????????????
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


            // ????????????????? REPOSITORIES ?????????????????
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();
            builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
            builder.Services.AddScoped<IFacilityRepository, FacilityRepository>();
            builder.Services.AddScoped<IFacilityCategoryRepository, FacilityCategoryRepository>();
            builder.Services.AddScoped<IMemberShipRepository, MemberShipRepository>();
            builder.Services.AddScoped<IActivityGroupRepository, ActivityGroupRepository>();
            builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IFacilityBookingRepository, FacilityBookingRepository>();



            // ????????????????? SERVICES ?????????????????

            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IActivityService, ActivityService>();
            builder.Services.AddScoped<IFacilityService, FacilityService>();
            builder.Services.AddScoped<IFacilityCategoryService, FacilityCategoryService>();
            builder.Services.AddScoped<IMemberShipService, MemberShipService>();
            builder.Services.AddScoped<IActivityGroupService, ActivityGroupService>();
            builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IFacilityScheduleRepository, FacilityScheduleRepository>();
            builder.Services.AddScoped<IFacilityScheduleService, FacilityScheduleService>();
            builder.Services.AddScoped<IFacilityBookingService, FacilityBookingService>();


            // ????????????????? BUILD ?????????????????
            var app = builder.Build();


            // ????????????????? MIDDLEWARE ?????????????????
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("Frontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();


            // ????????????????? AUTO MIGRATION ?????????????????
            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }


            app.Run();
        }
    }
}