using Clubly.DTO;
using Clubly.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SignUp.Data;
using SignUp.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clubly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // ── LOGIN ─────────────────────────────────────
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var role = dto.Role.ToLower();

            // جيب الـ credentials بناءً على الـ role
            var (id, fullName, imageUrl, passwordHash, passwordSalt) = role switch
            {
                "admin" => await GetCredentials(_context.Admins, dto.Email,
                    u => (u.Id, u.FullName, u.ImageUrl, u.PasswordHash, u.PasswordSalt)),

                "member" => await GetCredentials(_context.Members, dto.Email,
                    u => (u.Id, u.FullName, u.ImageUrl, u.PasswordHash, u.PasswordSalt)),

                "trainer" => await GetCredentials(_context.Trainers, dto.Email,
                    u => (u.Id, u.FullName, u.ImageUrl, u.PasswordHash, u.PasswordSalt)),

                "guest" => await GetCredentials(_context.Guests, dto.Email,
                    u => (u.Id, u.FullName, (string?)null, u.PasswordHash, u.PasswordSalt)),

                _ => (0, null, null, null, null)
            };

            if (passwordHash == null || passwordSalt == null)
                return Unauthorized("Invalid credentials.");

            if (!VerifyPassword(dto.Password, passwordHash, passwordSalt))
                return Unauthorized("Invalid credentials.");

            var token = GenerateToken(id, dto.Email, role, fullName!);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Role = role,
                FullName = fullName!,
                Id = id,
                ImageUrl = imageUrl
            });
        }

        // ── REGISTER (Guest only) ──────────────────────
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Guests.AnyAsync(g => g.Email == dto.Email))
                return BadRequest("Email already exists.");

            CreatePasswordHash(dto.Password, out string hash, out string salt);

            var guest = new Guest
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone ?? "",
                NationalId = dto.NationalId ?? "",
                Gender = dto.Gender ?? "",
                DateOfBirth = dto.DateOfBirth,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();

            var token = GenerateToken(guest.Id, guest.Email, "guest", guest.FullName);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Role = "guest",
                FullName = guest.FullName,
                Id = guest.Id
            });
        }

        // ── HELPERS ───────────────────────────────────
        private async Task<(int, string?, string?, string?, string?)> GetCredentials<T>(
            IQueryable<T> dbSet,
            string email,
            Func<T, (int, string?, string?, string?, string?)> selector)
            where T : class
        {
            var user = await dbSet.FirstOrDefaultAsync(
                u => EF.Property<string>(u, "Email") == email);

            return user == null ? (0, null, null, null, null) : selector(user);
        }

        private string GenerateToken(int id, string email, string role, string fullName)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Email,          email),
            new Claim(ClaimTypes.Role,           role),
            new Claim(ClaimTypes.Name,           fullName)
        };

            var key = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void CreatePasswordHash(string password, out string hash, out string salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            salt = Convert.ToBase64String(hmac.Key);
            hash = Convert.ToBase64String(
                       hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private bool VerifyPassword(string password, string hash, string salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(
                                 Convert.FromBase64String(salt));
            return Convert.ToBase64String(
                       hmac.ComputeHash(Encoding.UTF8.GetBytes(password))) == hash;
        }
    

    // ── FORGOT PASSWORD ───────────────────────────────
[HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var email = dto.Email.ToLower().Trim();
            var otp = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(10);
            bool found = false;

            var admin = await _context.Admins.FirstOrDefaultAsync(u => u.Email == email);
            if (admin != null) { admin.OtpCode = otp; admin.OtpExpiry = expiry; found = true; }

            var member = await _context.Members.FirstOrDefaultAsync(u => u.Email == email);
            if (member != null) { member.OtpCode = otp; member.OtpExpiry = expiry; found = true; }

            var trainer = await _context.Trainers.FirstOrDefaultAsync(u => u.Email == email);
            if (trainer != null) { trainer.OtpCode = otp; trainer.OtpExpiry = expiry; found = true; }

            var guest = await _context.Guests.FirstOrDefaultAsync(u => u.Email == email);
            if (guest != null) { guest.OtpCode = otp; guest.OtpExpiry = expiry; found = true; }

            if (!found) return NotFound("Email not found.");

            await _context.SaveChangesAsync();
            await SendOtpEmail(email, otp);

            return Ok("OTP sent.");
        }

        // ── VERIFY OTP ────────────────────────────────────
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var email = dto.Email.ToLower().Trim();
            var now = DateTime.UtcNow;

            bool valid =
                await _context.Admins.AnyAsync(u =>
                    u.Email == email && u.OtpCode == dto.Otp && u.OtpExpiry > now) ||
                await _context.Members.AnyAsync(u =>
                    u.Email == email && u.OtpCode == dto.Otp && u.OtpExpiry > now) ||
                await _context.Trainers.AnyAsync(u =>
                    u.Email == email && u.OtpCode == dto.Otp && u.OtpExpiry > now) ||
                await _context.Guests.AnyAsync(u =>
                    u.Email == email && u.OtpCode == dto.Otp && u.OtpExpiry > now);

            if (!valid) return BadRequest("Invalid or expired OTP.");

            // ← غير ده
            return Ok(new { message = "OTP verified.", otp = dto.Otp });
        }
        // ── RESET PASSWORD ────────────────────────────────
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var email = dto.Email.ToLower().Trim();
            var now = DateTime.UtcNow;
            bool done = false;

            CreatePasswordHash(dto.NewPassword, out string hash, out string salt);

            var admin = await _context.Admins.FirstOrDefaultAsync(u =>
                u.Email == email && u.OtpCode == dto.Otp && u.OtpExpiry > now);
            if (admin != null)
            {
                admin.PasswordHash = hash; admin.PasswordSalt = salt;
                admin.OtpCode = null; admin.OtpExpiry = null;
                done = true;
            }

            var member = await _context.Members.FirstOrDefaultAsync(u =>
                u.Email == email && u.OtpCode == dto.Otp && u.OtpExpiry > now);
            if (member != null)
            {
                member.PasswordHash = hash; member.PasswordSalt = salt;
                member.OtpCode = null; member.OtpExpiry = null;
                done = true;
            }

            var trainer = await _context.Trainers.FirstOrDefaultAsync(u =>
                u.Email == email && u.OtpCode == dto.Otp && u.OtpExpiry > now);
            if (trainer != null)
            {
                trainer.PasswordHash = hash; trainer.PasswordSalt = salt;
                trainer.OtpCode = null; trainer.OtpExpiry = null;
                done = true;
            }

            var guest = await _context.Guests.FirstOrDefaultAsync(u =>
                u.Email == email && u.OtpCode == dto.Otp && u.OtpExpiry > now);
            if (guest != null)
            {
                guest.PasswordHash = hash; guest.PasswordSalt = salt;
                guest.OtpCode = null; guest.OtpExpiry = null;
                done = true;
            }

            if (!done) return BadRequest("Invalid or expired OTP.");

            await _context.SaveChangesAsync();
            return Ok("Password reset successfully.");
        }

        // ── SEND OTP EMAIL ────────────────────────────────
        private async Task SendOtpEmail(string toEmail, string otp)
        {
            var from = _config["Email:From"]!;
            var password = _config["Email:Password"]!;
            var host = _config["Email:Host"] ?? "smtp.gmail.com";
            var port = int.Parse(_config["Email:Port"] ?? "587");

            var message = new System.Net.Mail.MailMessage();
            message.From = new System.Net.Mail.MailAddress(from, "Clubly");
            message.To.Add(toEmail);
            message.Subject = "🔑 Your Clubly Password Reset Code";
            message.IsBodyHtml = true;
            message.Body = $@"
        <div style='font-family:Cairo,sans-serif;max-width:480px;margin:0 auto;
                    background:#0d1b2a;border-radius:16px;padding:32px;color:#fff;'>
            <h2 style='color:#e85d2f;margin-bottom:8px;'>Clubly Password Reset</h2>
            <p style='color:rgba(255,255,255,.7);margin-bottom:24px;'>
                Use the code below to reset your password. Valid for 10 minutes.
            </p>
            <div style='background:#122236;border-radius:12px;padding:20px;
                        text-align:center;letter-spacing:12px;font-size:32px;
                        font-weight:900;color:#2ec4b6;margin-bottom:24px;'>
                {otp}
            </div>
            <p style='color:rgba(255,255,255,.4);font-size:12px;'>
                If you didn't request this, ignore this email.
            </p>
        </div>";

            using var smtp = new System.Net.Mail.SmtpClient(host, port);
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(from, password);
            await smtp.SendMailAsync(message);
        }
    } }