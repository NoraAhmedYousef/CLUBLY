using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class AdminDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string NationalId { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(50)]
        public string? Gender { get; set; }
        public string ImageUrl { get; set; }
    }
}
