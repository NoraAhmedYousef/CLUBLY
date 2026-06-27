namespace SignUp.DTO
{
    public class Activity2Dto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }

        public int FacilityId { get; set; }
        public string FacilityName { get; set; } = "Not assigned";

        public string Status { get; set; } = "Active";
        public string? ImageUrl { get; set; }


    }
}
