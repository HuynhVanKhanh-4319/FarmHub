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
    [Route("report")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var reports = await _context.Reports
                .Include(r => r.Season)
                .Where(r => !r.IsDeleted && r.Season.ApplicationUserId == userId)
                .ToListAsync();

            return View(reports);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);
            var userSeasons = await _context.Seasons
                .Where(s => !s.IsDeleted && s.ApplicationUserId == userId)
                .ToListAsync();

            ViewData["SeasonId"] = new SelectList(userSeasons, "Id", "Name");
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Report report)
        {
            if (ModelState.IsValid)
            {
                report.IsDeleted = false;
                _context.Reports.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var userId = _userManager.GetUserId(User);
            var userSeasons = await _context.Seasons
                .Where(s => !s.IsDeleted && s.ApplicationUserId == userId)
                .ToListAsync();

            ViewData["SeasonId"] = new SelectList(userSeasons, "Id", "Name", report.SeasonId);
            return View(report);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null || report.IsDeleted) return NotFound();

            var userId = _userManager.GetUserId(User);
            var userSeasons = await _context.Seasons
                .Where(s => !s.IsDeleted && s.ApplicationUserId == userId)
                .ToListAsync();

            ViewData["SeasonId"] = new SelectList(userSeasons, "Id", "Name", report.SeasonId);
            return View(report);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Report report)
        {
            if (id != report.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Reports.Update(report);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var userId = _userManager.GetUserId(User);
            var userSeasons = await _context.Seasons
                .Where(s => !s.IsDeleted && s.ApplicationUserId == userId)
                .ToListAsync();

            ViewData["SeasonId"] = new SelectList(userSeasons, "Id", "Name", report.SeasonId);
            return View(report);
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null) return NotFound();

            report.IsDeleted = true;
            _context.Update(report);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
