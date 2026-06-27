namespace Clubly.DTO
{
    public class GuestDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string NationalId { get; set; } = "";
        public string Gender { get; set; } = "";
        public DateTime? DateOfBirth { get; set; }
        public string? ImageUrl { get; set; }   // ← أضفها

        public DateTime CreatedAt { get; set; }
    }
}
