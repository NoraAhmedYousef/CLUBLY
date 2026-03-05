

namespace SignUp.DTO
{
    public class UpdateMemberDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? NationalId { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? MemberShipNumber { get; set; }
        public DateTime? JoinDate { get; set; }
        public string? MemberType { get; set; }
        public int? MemberShipId { get; set; }
        public IFormFile? Image { get; set; }
        public string? NewPassword { get; set; }


    }
}
