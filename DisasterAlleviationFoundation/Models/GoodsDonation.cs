using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class GoodsDonation
    {
        public int Id { get; set; }

        [Required]
        public string DonorName { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int NumberOfItems { get; set; }

        public DateTime DonationDate { get; set; } = DateTime.Now;
        public bool IsAnonymous { get; set; }
        public string Status { get; set; } = "Pending";
    }
}