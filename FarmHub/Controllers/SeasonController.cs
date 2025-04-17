using FarmHub.Data;
using FarmHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FarmHub.Controllers
{
  //  [Authorize]
    [Route("season")]
    public class SeasonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SeasonController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
            var seasons = await _context.Seasons
                .Where(s => !s.IsDeleted && s.ApplicationUserId == userId)
                .Include(s => s.Crop)
                .Include(s => s.Schedules)
                .Include(s => s.Reports)
                .ToListAsync();
            return View(seasons);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {

            var userId = _userManager.GetUserId(User);
            ViewBag.Crops = new SelectList(await _context.Crops
                .Where(c => c.ApplicationUserId == userId) 
                .ToListAsync(), "Id", "Name");

            var season = new Season
            {
                StartDate = DateTime.Now,
                HarvestDate = DateTime.Now.AddMonths(3)
            };
            return View(season);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Season season)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);


                var crop = await _context.Crops
                    .FirstOrDefaultAsync(c => c.Id == season.CropId && c.ApplicationUserId == userId);

                if (crop == null)
                {
                  
                    ModelState.AddModelError("CropId", "Crop này không phải của bạn.");
                    ViewBag.Crops = new SelectList(await _context.Crops
                        .Where(c => c.ApplicationUserId == userId)
                        .ToListAsync(), "Id", "Name", season.CropId);
                    return View(season);
                }

                season.ApplicationUserId = userId;
                season.IsDeleted = false;

                _context.Seasons.Add(season);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Crops = new SelectList(await _context.Crops
                .Where(c => c.ApplicationUserId == _userManager.GetUserId(User))
                .ToListAsync(), "Id", "Name", season.CropId);

            return View(season);
        }


        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var season = await _context.Seasons.FindAsync(id);
            if (season == null || season.IsDeleted)
                return NotFound();

            ViewBag.Crops = new SelectList(await _context.Crops.ToListAsync(), "Id", "Name", season.CropId);
            return View(season);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Season season)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(season);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Lỗi cập nhật dữ liệu.");
                }
            }

            ViewBag.Crops = new SelectList(await _context.Crops.ToListAsync(), "Id", "Name", season.CropId);
            return View(season);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var season = await _context.Seasons.FindAsync(id);
            if (season != null)
            {
                season.IsDeleted = true;
                _context.Update(season);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
