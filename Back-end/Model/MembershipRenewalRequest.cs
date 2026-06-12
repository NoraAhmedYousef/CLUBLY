using SignUp.Model;
using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class MembershipRenewalRequest : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        // ── Member ─────────────────────────────────────
        [Required(ErrorMessage = "Member is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "MemberId must be greater than zero.")]
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        // ── Requested Plan ─────────────────────────────
        [Required(ErrorMessage = "MemberShip is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "MemberShipId must be greater than zero.")]
        public int MemberShipId { get; set; }
        public MemberShip MemberShip { get; set; } = null!;

        // ── Status ──────────────────────────────────────
        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression(@"^(Pending|Approved|Rejected)$",
            ErrorMessage = "Status must be: Pending, Approved, or Rejected.")]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        // ── Payment ─────────────────────────────────────
        [RegularExpression(@"^(InstaPay|EWallet|Cash|Card|)$",
            ErrorMessage = "Invalid payment method.")]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "";

        [MaxLength(100)]
        public string TransactionId { get; set; } = "";

        [Range(0, 999999.99,
            ErrorMessage = "Price must be between 0 and 999,999.99.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [StringLength(2048,
            ErrorMessage = "Receipt URL cannot exceed 2048 characters.")]
        public string ReceiptImageUrl { get; set; } = "";

        // ── Dates ───────────────────────────────────────
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public DateTime? ProcessedDate { get; set; }

        // ── Cross-field validation ──────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            if (Price < 0)
                yield return new ValidationResult(
                    "Price cannot be negative.",
                    new[] { nameof(Price) });
        }
    }
}