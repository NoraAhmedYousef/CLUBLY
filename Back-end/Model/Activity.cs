namespace SignUp.Model
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }

        public int FacilityId { get; set; }
        public Facility Facility { get; set; }

        public string Status { get; set; } = "Active";
        public string? ImageUrl { get; set; }

        public List<ActivityGroup> ActivityGroups { get; set; } = new();
        public List<Trainer> Trainers { get; set; } = new List<Trainer>();


    }
}
