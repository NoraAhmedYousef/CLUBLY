using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class CreateTrainerDto
    {
        [Required] public string FullName { get; set; } = "";
        [EmailAddress] public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public int YearsOfExperience { get; set; }
        public string? NationalId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public List<int>? ActivityIds { get; set; }
        public bool IsActive { get; set; } = true;
        public IFormFile? Image { get; set; }
        public string Password { get; set; }  

    }
}
