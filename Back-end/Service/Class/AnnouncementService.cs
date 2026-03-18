

using SignUp.DTO;
using SignUp.Model;
using SignUp.Repository.Interfaces;
using SignUp.Service.Interfaces;

namespace SignUp.Service.Class
{
    public class AnnouncementService: IAnnouncementService
    {
        private readonly IAnnouncementRepository _repo;
        private readonly IWebHostEnvironment _env;

        public AnnouncementService(IAnnouncementRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        // ------------------ Save Image ------------------
        private async Task<string?> SaveImageAsync(IFormFile? image)
        {
            if (image == null || image.Length == 0) return null;

            string folder = Path.Combine(_env.WebRootPath, "Uploads", "Announcements");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            string filePath = Path.Combine(folder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            return $"/Uploads/Announcements/{fileName}";
        }

        // ------------------ Delete Image ------------------
        private void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string fullPath = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }

        // ------------------ Get All ------------------
        public async Task<List<AnnouncementDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(a => new AnnouncementDto
            {
                Id = a.Id,
                Title = a.Title,
                Text = a.Text,
                ForWho = a.ForWho,
                PublishDate = a.PublishDate,
                ImageUrl = a.ImageUrl
            }).ToList();
        }

        // ------------------ Get By Id ------------------
        public async Task<AnnouncementDto?> GetByIdAsync(int id)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return null;

            return new AnnouncementDto
            {
                Id = a.Id,
                Title = a.Title,
                Text = a.Text,
                ForWho = a.ForWho,
                PublishDate = a.PublishDate,
                ImageUrl = a.ImageUrl
            };
        }

        // ------------------ Create ------------------
        public async Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto)
        {
            string? imageUrl = await SaveImageAsync(dto.Image);

            var a = new Announcement
            {
                Title = dto.Title,
                Text = dto.Text,
                ForWho = dto.ForWho,
                PublishDate = dto.PublishDate,
                ImageUrl = imageUrl
            };

            await _repo.CreateAsync(a);

            return await GetByIdAsync(a.Id) ?? new AnnouncementDto();
        }

        // ------------------ Update ------------------
        public async Task<bool> UpdateAsync(int id, UpdateAnnouncementDto dto)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return false;

            a.Title = dto.Title ?? a.Title;
            a.Text = dto.Text ?? a.Text;
            a.ForWho = dto.ForWho ?? a.ForWho;
            a.PublishDate = dto.PublishDate ?? a.PublishDate;

            if (dto.Image != null)
            {
                DeleteImage(a.ImageUrl);
                a.ImageUrl = await SaveImageAsync(dto.Image);
            }

            return await _repo.UpdateAsync(a);
        }

        // ------------------ Delete ------------------
        public async Task<bool> DeleteAsync(int id)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return false;

            DeleteImage(a.ImageUrl);

            return await _repo.DeleteAsync(id);
        }
    }
}

