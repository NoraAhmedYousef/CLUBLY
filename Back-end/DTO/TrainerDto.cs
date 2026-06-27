namespace SignUp.DTO
{
    public class TrainerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string? Phone { get; set; }
        public string Email { get; set; } = "";
        public int YearsOfExperience { get; set; }
        public string? NationalId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string Activities { get; set; } // للعرض
        public List<int> ActivityIds { get; set; } // للتعديل
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }

    }
}
