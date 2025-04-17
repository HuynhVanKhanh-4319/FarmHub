using FarmHub.Data;
using FarmHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FarmHub.Controllers
{
    [Authorize]
    [Route("schedule")]
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScheduleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager; 
        }
        [Route("")]
        [Route("index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var schedules = await _context.Schedules
                .Include(s => s.Season)
                .Where(s => !s.IsDeleted && s.Season.ApplicationUserId == userId)
                .ToListAsync();

            return View(schedules);
        }
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);
            var userSeasons = await _context.Seasons
                .Where(s => !s.IsDeleted && s.ApplicationUserId == userId)
                .ToListAsync();

            ViewBag.SeasonId = new SelectList(userSeasons, "Id", "Name");

            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var userId = _userManager.GetUserId(User);
            var userSeasons = await _context.Seasons
                .Where(s => !s.IsDeleted && s.ApplicationUserId == userId)
                .ToListAsync();

            ViewBag.SeasonId = new SelectList(userSeasons, "Id", "Name", schedule.SeasonId);
            return View(schedule);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null || schedule.IsDeleted) return NotFound();

            var userId = _userManager.GetUserId(User);
            var userSeasons = await _context.Seasons
                .Where(s => !s.IsDeleted && s.ApplicationUserId == userId)
                .ToListAsync();

            ViewData["SeasonId"] = new SelectList(userSeasons, "Id", "Name", schedule.SeasonId);

            return View(schedule);
        }


        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Schedule schedule)
        {
            if (id != schedule.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Schedules.Any(s => s.Id == id)) return NotFound();
                    throw;
                }
            }

            var userId = _userManager.GetUserId(User);
            var userSeasons = await _context.Seasons
                .Where(s => !s.IsDeleted && s.ApplicationUserId == userId)
                .ToListAsync();

            ViewData["SeasonId"] = new SelectList(userSeasons, "Id", "Name", schedule.SeasonId);
            return View(schedule);
        }

        [HttpPost("delete/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                schedule.IsDeleted = true;
                _context.Update(schedule);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
