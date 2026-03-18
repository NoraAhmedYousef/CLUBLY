using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class CreateAnnouncementDto
    {
        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Text { get; set; } = "";

        [Required]
        public List<string> ForWho { get; set; } = new();

        public DateTime PublishDate { get; set; } = DateTime.UtcNow;

        public IFormFile? Image { get; set; }

    }
}

