namespace SignUp.DTO
{
    public class UpdateTrainerDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? NationalId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public List<int>? ActivityIds { get; set; }
        public bool? IsActive { get; set; }
        public IFormFile? Image { get; set; }
        public string? NewPassword { get; set; }
    }
}
