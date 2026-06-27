using System.ComponentModel.DataAnnotations;

namespace Clubly.DTO
{
    public class UpdateMembershipRenewalStatusDto
    {
        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression(@"^(Pending|Approved|Rejected)$",
            ErrorMessage = "Status must be: Pending, Approved, or Rejected.")]
        public string Status { get; set; } = "";
    }
}
