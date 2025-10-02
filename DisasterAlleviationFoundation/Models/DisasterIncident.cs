using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class DisasterIncident
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime ReportedOn { get; set; } = DateTime.Now;
        public string? ReportedByUserId { get; set; }
        public virtual ApplicationUser? ReportedByUser { get; set; }
    }
}