namespace Clubly.DTO
{
    public class NotificationResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime SentAt { get; set; }
        public string Status { get; set; }
        public bool ToMembers { get; set; }
        public bool ToTrainers { get; set; }
        public bool ToAdmins { get; set; }
        public bool ToGuests { get; set; }
        public string? Type { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsRead { get; set; }
        public List<string> Recipients
        {
            get
            {
                var list = new List<string>();
                if (ToAdmins) list.Add("Admins");
                if (ToTrainers) list.Add("Trainers");
                if (ToMembers) list.Add("Members");
                if (ToGuests) list.Add("Guests");
                return list;
            }
        }
    }
}