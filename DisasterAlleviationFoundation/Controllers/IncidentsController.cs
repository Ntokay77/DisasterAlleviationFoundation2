using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundation.Controllers
{
    [Authorize]
    public class IncidentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncidentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Incidents
        public async Task<IActionResult> Index()
        {
            var incidents = await _context.DisasterIncidents
                .OrderByDescending(i => i.ReportedOn)
                .ToListAsync();
            return View(incidents);
        }

        // GET: Incidents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disasterIncident = await _context.DisasterIncidents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (disasterIncident == null)
            {
                return NotFound();
            }

            return View(disasterIncident);
        }

        // GET: Incidents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Incidents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DisasterIncident disasterIncident)
        {
            if (ModelState.IsValid)
            {
                disasterIncident.ReportedByUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                disasterIncident.ReportedOn = DateTime.Now;

                _context.Add(disasterIncident);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(disasterIncident);
        }

        // GET: Incidents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disasterIncident = await _context.DisasterIncidents.FindAsync(id);
            if (disasterIncident == null)
            {
                return NotFound();
            }
            return View(disasterIncident);
        }

        // POST: Incidents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DisasterIncident disasterIncident)
        {
            if (id != disasterIncident.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disasterIncident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisasterIncidentExists(disasterIncident.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(disasterIncident);
        }

        // GET: Incidents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disasterIncident = await _context.DisasterIncidents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (disasterIncident == null)
            {
                return NotFound();
            }

            return View(disasterIncident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disasterIncident = await _context.DisasterIncidents.FindAsync(id);
            if (disasterIncident != null)
            {
                _context.DisasterIncidents.Remove(disasterIncident);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DisasterIncidentExists(int id)
        {
            return _context.DisasterIncidents.Any(e => e.Id == id);
        }
    }
}