
using Clubly.Model;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SignUp.Model
{
    public class Member
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

        public int? MemberShipId { get; set; } // FK
        public MemberShip? MemberShip { get; set; }

        public string? ImageUrl { get; set; }
        public string PasswordHash { get; set; } = "";
        public string PasswordSalt { get; set; } = "";
        public ICollection<ActivityBooking> ActivityBookings { get; set; } = new List<ActivityBooking>();


    }
}
