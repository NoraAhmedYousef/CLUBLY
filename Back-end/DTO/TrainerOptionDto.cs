namespace SignUp.DTO
{
    public class TrainerOptionDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string? ImageUrl { get; set; }
        public int YearsOfExperience { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
