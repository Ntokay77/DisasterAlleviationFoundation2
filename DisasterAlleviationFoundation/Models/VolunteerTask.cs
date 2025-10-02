using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class VolunteerTask
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public DateTime TaskDate { get; set; } = DateTime.Now.AddDays(1);

        [Range(1, 50)]
        public int RequiredVolunteers { get; set; } = 5;

        public int CurrentVolunteers { get; set; } = 0;
        public string Status { get; set; } = "Open";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}