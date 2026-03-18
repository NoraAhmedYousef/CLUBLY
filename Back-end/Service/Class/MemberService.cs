using Microsoft.EntityFrameworkCore;
using SignUp.Data;
using SignUp.DTO;
using SignUp.Model;
using SignUp.Repository.Interfaces;
using SignUp.Service.Interfaces;


namespace SignUp.Service.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repo;
        private readonly IWebHostEnvironment _env;

        public MemberService(IMemberRepository repo,
                             IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        // GET ALL
        public async Task<List<Member2Dto>> GetAllAsync()
        {
            var members = await _repo.GetAllAsync();

            return members.Select(m => new Member2Dto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                Phone = m.Phone,
                NationalId = m.NationalId,
                Gender = m.Gender,
                BirthDate = m.BirthDate,
                MemberShipNumber = m.MemberShipNumber,
                JoinDate = m.JoinDate,
                MemberType = m.MemberType,
                MemberShipId = m.MemberShipId ?? 0,
                MembershipName = m.MemberShip?.Name ?? "",
                ImageUrl = m.ImageUrl
            }).ToList();
        }

        // GET BY ID
        
        public async Task<Member2Dto?> GetByIdAsync(int id)
        {
            var m = await _repo.GetByIdAsync(id);
            if (m == null) return null;

            return new Member2Dto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                Phone = m.Phone,
                NationalId = m.NationalId,
                Gender = m.Gender,
                BirthDate = m.BirthDate,
                MemberShipNumber = m.MemberShipNumber,
                JoinDate = m.JoinDate,
                MemberType = m.MemberType,
                MemberShipId = m.MemberShipId ?? 0,
                MembershipName = m.MemberShip?.Name ?? "",
                ImageUrl = m.ImageUrl
            };
        }

        // CREATE
        public async Task<Member2Dto> CreateAsync(CreateMemberDto dto)
        {
            var member = new Member
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                NationalId = dto.NationalId,
                Gender = dto.Gender,
                BirthDate = dto.BirthDate,
                MemberShipNumber = dto.MemberShipNumber,
                JoinDate = dto.JoinDate,
                MemberType = dto.MemberType,
                MemberShipId = dto.MemberShipId
            };

            // -------- Password Hash ----------
            CreatePasswordHash(dto.Password, out string hash, out string salt);
            member.PasswordHash = hash;
            member.PasswordSalt = salt;

            // -------- Image Upload ----------
            if (dto.Image != null)
                member.ImageUrl = await SaveImage(dto.Image);

            await _repo.AddAsync(member);

            return await GetByIdAsync(member.Id);
        }

        // UPDATE
        public async Task<Member2Dto?> UpdateAsync(int id, UpdateMemberDto dto)
        {
            var m = await _repo.GetByIdAsync(id);
            if (m == null) return null;

            if (dto.FullName != null) m.FullName = dto.FullName;
            if (dto.Email != null) m.Email = dto.Email;
            if (dto.Phone != null) m.Phone = dto.Phone;
            if (dto.NationalId != null) m.NationalId = dto.NationalId;
            if (dto.Gender != null) m.Gender = dto.Gender;
            if (dto.BirthDate != null) m.BirthDate = dto.BirthDate.Value;
            if (dto.MemberShipNumber != null) m.MemberShipNumber = dto.MemberShipNumber.Value;
            if (dto.JoinDate != null) m.JoinDate = dto.JoinDate.Value;
            if (dto.MemberType != null) m.MemberType = dto.MemberType;
            if (dto.MemberShipId != null) m.MemberShipId = dto.MemberShipId;

            // -------- update password ----------
            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                if (string.IsNullOrEmpty(dto.CurrentPassword))
                    throw new Exception("Current password is required");

                if (!VerifyPassword(dto.CurrentPassword, m.PasswordHash, m.PasswordSalt))
                    throw new Exception("Current password is incorrect");

                CreatePasswordHash(dto.NewPassword, out string hash, out string salt);
                m.PasswordHash = hash;
                m.PasswordSalt = salt;
            }
            // --------- Update Image ----------
            if (dto.Image != null)
                m.ImageUrl = await SaveImage(dto.Image);

            await _repo.UpdateAsync(m);

            return await GetByIdAsync(m.Id);
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var m = await _repo.GetByIdAsync(id);
            if (m == null) return false;

            await _repo.DeleteAsync(m);
            return true;
        }

        // IMAGE SAVER
        private async Task<string> SaveImage(IFormFile file)
        {
            string folder = Path.Combine(_env.WebRootPath, "uploads/members");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/members/" + fileName;
        }

        // DASHBOARD LIST
        public async Task<List<MemberDto>> GetAllForDash()
        {
            var members = await _repo.GetAllAsync();
            return members
                .Select(m => new MemberDto
                {
                    Id = m.Id,
                    FullName = m.FullName
                })
                .ToList();
        }

        // PASSWORD HASHING
        private void CreatePasswordHash(string password, out string hash, out string salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            salt = Convert.ToBase64String(hmac.Key);
            hash = Convert.ToBase64String(
                   hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var key = Convert.FromBase64String(storedSalt);

            using var hmac = new System.Security.Cryptography.HMACSHA512(key);
            var computed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return Convert.ToBase64String(computed) == storedHash;
        }
    }
}