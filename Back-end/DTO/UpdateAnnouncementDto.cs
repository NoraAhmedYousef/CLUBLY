namespace SignUp.DTO
{
    public class UpdateAnnouncementDto
    {
        public string? Title { get; set; }
        public string? Text { get; set; }
        public List<string> ForWho { get; set; } = new();
        public DateTime? PublishDate { get; set; }
        public IFormFile? Image { get; set; }
    }

}
