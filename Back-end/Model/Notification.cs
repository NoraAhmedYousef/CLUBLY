namespace Clubly.Model
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Sent"; // Sent / Draft

        // Audience flags
        public bool ToMembers { get; set; }
        public bool ToTrainers { get; set; }
        public bool ToAdmins { get; set; }
        public bool ToGuests { get; set; }
        public string? Type { get; set; } = "Info"; 
        public string? ImageUrl { get; set; }
    }
}
