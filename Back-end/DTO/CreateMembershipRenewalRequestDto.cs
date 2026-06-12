using System.ComponentModel.DataAnnotations;

namespace Clubly.DTO
{
    public class CreateMembershipRenewalRequestDto
    {
        [Required(ErrorMessage = "MemberId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "MemberId must be greater than zero.")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "MemberShipId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "MemberShipId must be greater than zero.")]
        public int MemberShipId { get; set; }

        [RegularExpression(@"^(InstaPay|EWallet|Cash|Card|)$",
            ErrorMessage = "Invalid payment method.")]
        public string PaymentMethod { get; set; } = "";

        [Required(ErrorMessage = "Transaction ID is required.")]
        [MaxLength(100)]
        public string TransactionId { get; set; } = "";

        // Receipt image file (multipart/form-data) — optional, like FacilityBookings
        public IFormFile? ReceiptImage { get; set; }
    }
}
