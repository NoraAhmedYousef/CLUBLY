using SignUp.Model;

namespace SignUp.DTO
{
    public class Member2Dto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string NationalId { get; set; } = "";
        public string Gender { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public int MemberShipNumber { get; set; }
        public DateTime JoinDate { get; set; }
        public string MemberType { get; set; } = "";

        public int MemberShipId { get; set; }
        public string MembershipName { get; set; } = "";
        public string? ImageUrl { get; set; }

    }
}
