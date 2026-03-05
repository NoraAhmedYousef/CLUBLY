namespace SignUp.Model
{
    public class MemberShip
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }         
        public string Status { get; set; } = "Active"; // Active / Inactive
    }
}
