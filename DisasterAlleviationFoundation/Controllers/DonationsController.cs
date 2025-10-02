using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundation.Controllers
{
    [Authorize]
    public class DonationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Donations
        public async Task<IActionResult> Index()
        {
            return View(await _context.GoodsDonations.OrderByDescending(d => d.DonationDate).ToListAsync());
        }

        // GET: Donations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsDonation = await _context.GoodsDonations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goodsDonation == null)
            {
                return NotFound();
            }

            return View(goodsDonation);
        }

        // GET: Donations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Donations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GoodsDonation goodsDonation)
        {
            if (ModelState.IsValid)
            {
                goodsDonation.DonationDate = DateTime.Now;
                _context.Add(goodsDonation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(goodsDonation);
        }

        // GET: Donations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsDonation = await _context.GoodsDonations.FindAsync(id);
            if (goodsDonation == null)
            {
                return NotFound();
            }
            return View(goodsDonation);
        }

        // POST: Donations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GoodsDonation goodsDonation)
        {
            if (id != goodsDonation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goodsDonation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsDonationExists(goodsDonation.Id))
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
            return View(goodsDonation);
        }

        // GET: Donations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsDonation = await _context.GoodsDonations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goodsDonation == null)
            {
                return NotFound();
            }

            return View(goodsDonation);
        }

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goodsDonation = await _context.GoodsDonations.FindAsync(id);
            if (goodsDonation != null)
            {
                _context.GoodsDonations.Remove(goodsDonation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoodsDonationExists(int id)
        {
            return _context.GoodsDonations.Any(e => e.Id == id);
        }
    }
}