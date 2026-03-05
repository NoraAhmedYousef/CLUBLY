namespace SignUp.DTO
{
    public class RegisterDto
    {
        public string NationalId { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; }

        public bool Agree { get; set; }

    }
}