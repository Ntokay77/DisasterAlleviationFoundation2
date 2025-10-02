using Microsoft.AspNetCore.Identity;

namespace DisasterAlleviationFoundation.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.Now;
    }
}