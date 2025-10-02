using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundation.Controllers
{
    [Authorize]
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Volunteer/Tasks
        public async Task<IActionResult> Tasks()
        {
            var tasks = await _context.VolunteerTasks
                .OrderBy(t => t.TaskDate)
                .ToListAsync();
            return View(tasks);
        }

        // GET: Volunteer/CreateTask
        public IActionResult CreateTask()
        {
            return View();
        }

        // POST: Volunteer/CreateTask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(VolunteerTask volunteerTask)
        {
            if (ModelState.IsValid)
            {
                volunteerTask.CreatedAt = DateTime.Now;
                volunteerTask.CurrentVolunteers = 0;
                _context.Add(volunteerTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Tasks));
            }
            return View(volunteerTask);
        }

        // GET: Volunteer/TaskDetails/5
        public async Task<IActionResult> TaskDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerTask = await _context.VolunteerTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (volunteerTask == null)
            {
                return NotFound();
            }

            return View(volunteerTask);
        }

        // GET: Volunteer/SignUp/5
        public async Task<IActionResult> SignUp(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerTask = await _context.VolunteerTasks.FindAsync(id);
            if (volunteerTask == null)
            {
                return NotFound();
            }

            // Check if user already signed up
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var existingSignup = await _context.VolunteerSignUps
                .FirstOrDefaultAsync(s => s.VolunteerTaskId == id && s.VolunteerUserId == userId);

            if (existingSignup != null)
            {
                TempData["Message"] = "You have already signed up for this task.";
                return RedirectToAction(nameof(Tasks));
            }

            // Check if task is full
            if (volunteerTask.CurrentVolunteers >= volunteerTask.RequiredVolunteers)
            {
                TempData["Message"] = "This task is already full.";
                return RedirectToAction(nameof(Tasks));
            }

            return View(volunteerTask);
        }

        // POST: Volunteer/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(int volunteerTaskId)
        {
            var volunteerTask = await _context.VolunteerTasks.FindAsync(volunteerTaskId);
            if (volunteerTask == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Check if already signed up
            var existingSignup = await _context.VolunteerSignUps
                .FirstOrDefaultAsync(s => s.VolunteerTaskId == volunteerTaskId && s.VolunteerUserId == userId);

            if (existingSignup != null)
            {
                TempData["Message"] = "You have already signed up for this task.";
                return RedirectToAction(nameof(Tasks));
            }

            // Create new signup
            var signUp = new VolunteerSignUp
            {
                VolunteerTaskId = volunteerTaskId,
                VolunteerUserId = userId,
                SignedUpAt = DateTime.Now,
                Status = "Confirmed"
            };

            _context.VolunteerSignUps.Add(signUp);

            // Update volunteer count
            volunteerTask.CurrentVolunteers++;
            if (volunteerTask.CurrentVolunteers >= volunteerTask.RequiredVolunteers)
            {
                volunteerTask.Status = "Filled";
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Successfully signed up for the volunteer task!";
            return RedirectToAction(nameof(MyTasks));
        }

        // GET: Volunteer/MyTasks
        public async Task<IActionResult> MyTasks()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var mySignUps = await _context.VolunteerSignUps
                .Include(s => s.VolunteerTask)
                .Where(s => s.VolunteerUserId == userId)
                .OrderBy(s => s.VolunteerTask.TaskDate)
                .ToListAsync();

            return View(mySignUps);
        }

        // POST: Volunteer/CancelSignUp/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelSignUp(int id)
        {
            var signUp = await _context.VolunteerSignUps
                .Include(s => s.VolunteerTask)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (signUp == null)
            {
                return NotFound();
            }

            // Update volunteer count
            if (signUp.VolunteerTask != null)
            {
                signUp.VolunteerTask.CurrentVolunteers--;
                if (signUp.VolunteerTask.Status == "Filled")
                {
                    signUp.VolunteerTask.Status = "Open";
                }
            }

            _context.VolunteerSignUps.Remove(signUp);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Successfully cancelled your volunteer sign-up.";
            return RedirectToAction(nameof(MyTasks));
        }

            // GET: Volunteer/EditTask/5
public async Task<IActionResult> EditTask(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerTask = await _context.VolunteerTasks.FindAsync(id);
            if (volunteerTask == null)
            {
                return NotFound();
            }
            return View(volunteerTask);
        }

        // POST: Volunteer/EditTask/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(int id, VolunteerTask volunteerTask)
        {
            if (id != volunteerTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerTaskExists(volunteerTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Tasks));
            }
            return View(volunteerTask);
        }

        private bool VolunteerTaskExists(int id)
        {
            return _context.VolunteerTasks.Any(e => e.Id == id);
        }
    }
    }
