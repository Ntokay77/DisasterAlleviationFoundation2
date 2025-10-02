using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundation.Data
{
    public static class DataSeeder
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure database is created
            context.Database.EnsureCreated();

            // Create roles
            string[] roleNames = { "Admin", "Manager", "Volunteer" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create admin user
            var adminUser = await userManager.FindByEmailAsync("admin@daf.org");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@daf.org",
                    Email = "admin@daf.org",
                    FullName = "System Administrator",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Add sample data if database is empty
            if (!context.DisasterIncidents.Any())
            {
                context.DisasterIncidents.AddRange(
                    new DisasterIncident
                    {
                        Title = "Flooding in Coastal Region",
                        Description = "Severe flooding affecting multiple communities in the coastal area. Emergency shelters needed.",
                        Location = "Coastal District",
                        StartDate = DateTime.Now.AddDays(-5),
                        Status = "Active",
                        ReportedOn = DateTime.Now.AddDays(-5)
                    },
                    new DisasterIncident
                    {
                        Title = "Earthquake Relief Needed",
                        Description = "6.5 magnitude earthquake has damaged infrastructure in the mountain region.",
                        Location = "Mountain Region",
                        StartDate = DateTime.Now.AddDays(-2),
                        Status = "Active",
                        ReportedOn = DateTime.Now.AddDays(-2)
                    }
                );
            }

            if (!context.GoodsDonations.Any())
            {
                context.GoodsDonations.AddRange(
                    new GoodsDonation
                    {
                        DonorName = "Community Helpers",
                        Category = "Food",
                        Description = "Non-perishable food items",
                        NumberOfItems = 150,
                        DonationDate = DateTime.Now.AddDays(-3),
                        IsAnonymous = false,
                        Status = "Received"
                    },
                    new GoodsDonation
                    {
                        DonorName = "Anonymous",
                        Category = "Clothing",
                        Description = "Winter clothing and blankets",
                        NumberOfItems = 200,
                        DonationDate = DateTime.Now.AddDays(-1),
                        IsAnonymous = true,
                        Status = "Pending"
                    }
                );
            }

            if (!context.VolunteerTasks.Any())
            {
                context.VolunteerTasks.AddRange(
                    new VolunteerTask
                    {
                        Title = "Food Distribution",
                        Description = "Help distribute food packages to affected families in the coastal region",
                        Location = "Coastal Relief Center",
                        TaskDate = DateTime.Now.AddDays(2),
                        RequiredVolunteers = 10,
                        CurrentVolunteers = 3,
                        Status = "Open",
                        CreatedAt = DateTime.Now.AddDays(-1)
                    },
                    new VolunteerTask
                    {
                        Title = "Shelter Setup",
                        Description = "Assist in setting up emergency shelters for displaced families",
                        Location = "Mountain Region Base",
                        TaskDate = DateTime.Now.AddDays(1),
                        RequiredVolunteers = 15,
                        CurrentVolunteers = 15,
                        Status = "Filled",
                        CreatedAt = DateTime.Now.AddDays(-2)
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}