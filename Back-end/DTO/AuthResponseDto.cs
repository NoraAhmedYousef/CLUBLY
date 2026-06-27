namespace Clubly.DTO
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = "";
    public string Role { get; set; } = "";
    public string FullName { get; set; } = "";
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    }
}
