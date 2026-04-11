using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class FAQ
    {
        public int Id { get; set; }

        [Required, MaxLength(300)]
        public string Question { get; set; } = "";

        [Required, MaxLength(2000)]
        public string Answer { get; set; } = "";

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}