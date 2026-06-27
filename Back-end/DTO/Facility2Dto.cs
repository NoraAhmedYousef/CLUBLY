using SignUp.Model;
using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class Facility2Dto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }

        public int FacilityCategoryId { get; set; }
        public string? FacilityCategoryName { get; set; }

        public int Capacity { get; set; }

        public string Status { get; set; } = "Active";

        public string? ImageUrl { get; set; }


    }

}

