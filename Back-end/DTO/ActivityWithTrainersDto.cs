namespace SignUp.DTO
{
    public class ActivityWithTrainersDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = "Active";
        public string? ImageUrl { get; set; }

        public List<TrainerDto> Trainers { get; set; } = new();
    }
}
