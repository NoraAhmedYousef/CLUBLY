using System.ComponentModel.DataAnnotations;

namespace Clubly.DTO
{
    public class NotificationDto
    {
        [Required, MinLength(3)]
        public string Title { get; set; }

        [Required, MinLength(5)]
        public string Description { get; set; }

        public bool ToMembers { get; set; }
        public bool ToTrainers { get; set; }
        public bool ToAdmins { get; set; }
        public bool ToGuests { get; set; }
        public string? Type { get; set; } = "Info";
        public IFormFile? Image { get; set; }
    }
}
