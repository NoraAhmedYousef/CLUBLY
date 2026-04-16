using SignUp.Model;

namespace Clubly.Model
{
    public class TrainerRating
    {

        public int Id { get; set; }
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int ActivityBookingId { get; set; }  // عشان نعرف من أي booking
        public ActivityBooking ActivityBooking { get; set; }  // ← أضف السطر ده

        public int Rating { get; set; }             // 1-5
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
