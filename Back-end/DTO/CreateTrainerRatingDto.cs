namespace Clubly.DTO
{
    public class CreateTrainerRatingDto
    {

        public int TrainerId { get; set; }
        public int MemberId { get; set; }
        public int ActivityBookingId { get; set; }
        public int Rating { get; set; } // 1-5
        public string? Comment { get; set; }
    }
}
