using DisasterAlleviationFoundation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var stats = new
            {
                TotalIncidents = await _context.DisasterIncidents.CountAsync(),
                ActiveIncidents = await _context.DisasterIncidents.CountAsync(i => i.Status == "Active"),
                TotalDonations = await _context.GoodsDonations.CountAsync(),
                PendingDonations = await _context.GoodsDonations.CountAsync(d => d.Status == "Pending"),
                OpenTasks = await _context.VolunteerTasks.CountAsync(t => t.Status == "Open"),
                TotalVolunteers = await _context.VolunteerSignUps.CountAsync()
            };

            ViewBag.Stats = stats;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}