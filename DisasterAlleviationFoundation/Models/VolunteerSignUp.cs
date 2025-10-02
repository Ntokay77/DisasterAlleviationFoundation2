using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class VolunteerSignUp
    {
        public int Id { get; set; }
        public int VolunteerTaskId { get; set; }
        public virtual VolunteerTask? VolunteerTask { get; set; }
        public string? VolunteerUserId { get; set; }
        public virtual ApplicationUser? VolunteerUser { get; set; }
        public DateTime SignedUpAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Confirmed";
    }
}