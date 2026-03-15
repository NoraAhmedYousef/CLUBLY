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

        // ── LOGIN ──────────────────────────────────────────
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            string? passwordHash = null, passwordSalt = null, fullName = null, imageUrl = null;
            int id = 0;
            if (dto.Role.ToLower() == "admin")
            {
                var u = await _context.Admins.FirstOrDefaultAsync(a => a.Email == dto.Email);
                if (u == null) return Unauthorized("Invalid credentials");
                passwordHash = u.PasswordHash; passwordSalt = u.PasswordSalt;
                fullName = u.FullName; id = u.Id; imageUrl = u.ImageUrl;
            }
            else if (dto.Role.ToLower() == "member")
            {
                var u = await _context.Members.FirstOrDefaultAsync(m => m.Email == dto.Email);
                if (u == null) return Unauthorized("Invalid credentials");
                passwordHash = u.PasswordHash; passwordSalt = u.PasswordSalt;
                fullName = u.FullName; id = u.Id; imageUrl = u.ImageUrl;
            }
            else if (dto.Role.ToLower() == "trainer")
            {
                var u = await _context.Trainers.FirstOrDefaultAsync(t => t.Email == dto.Email);
                if (u == null) return Unauthorized("Invalid credentials");
                passwordHash = u.PasswordHash; passwordSalt = u.PasswordSalt;
                fullName = u.FullName; id = u.Id; imageUrl = u.ImageUrl;
            }
            else if (dto.Role.ToLower() == "guest")
            {
                var u = await _context.Guests.FirstOrDefaultAsync(g => g.Email == dto.Email);
                if (u == null) return Unauthorized("Invalid credentials");
                passwordHash = u.PasswordHash; passwordSalt = u.PasswordSalt;
                fullName = u.FullName; id = u.Id;
            }
            else return BadRequest("Invalid role");

            if (!VerifyPassword(dto.Password, passwordHash!, passwordSalt!))
                return Unauthorized("Invalid credentials");

            var token = GenerateToken(id, dto.Email, dto.Role, fullName!);
            return Ok(new AuthResponseDto
            {
                Token = token,
                Role = dto.Role,
                FullName = fullName!,
                Id = id,
                ImageUrl = imageUrl
            });
        }

        // ── REGISTER (Guest only) ──────────────────────────
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Guests.AnyAsync(g => g.Email == dto.Email))
                return BadRequest("Email already exists");

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

        // ── HELPERS ────────────────────────────────────────
        private string GenerateToken(int id, string email, string role, string fullName)
        {
            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, fullName)
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