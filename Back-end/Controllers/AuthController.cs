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
    }
}