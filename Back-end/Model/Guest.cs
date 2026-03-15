namespace Clubly.Model
{
   
        public class Guest
        {
            public int Id { get; set; }
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string FullName => $"{FirstName} {LastName}";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string NationalId { get; set; } = "";
            public string Gender { get; set; } = "";
            public DateTime? DateOfBirth { get; set; }
            public string PasswordHash { get; set; } = "";
            public string PasswordSalt { get; set; } = "";
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        }
    }

