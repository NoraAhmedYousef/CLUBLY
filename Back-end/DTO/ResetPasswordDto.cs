namespace SignUp.DTO
{
    public class ResetPasswordDto
    {
        public string Identifier { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
