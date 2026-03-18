namespace SignUp.DTO
{
    public class AnnouncementDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Text { get; set; } = "";
        public List<string> ForWho { get; set; } = new();

        public DateTime PublishDate { get; set; }
        public string? ImageUrl { get; set; }

    }
}
