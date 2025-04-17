using FarmHub.Data;
using FarmHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmHub.Areas.Admin.Controllers
{
    [Authorize]
    [Area("admin")]
    [Route("admin/report")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        [Route("index")]
        public IActionResult Index()
        {
            var monthlyYield = _context.Reports
                   .Where(r => !r.IsDeleted && r.Season != null)
                   .Select(r => new
                   {
                       r.Yield,
                       r.Season!.StartDate.Year,
                       r.Season.StartDate.Month
                   })
                   .ToList() 
                   .GroupBy(r => new { r.Year, r.Month })
                   .Select(g => new
                   {
                       Month = $"{g.Key.Month:D2}/{g.Key.Year}",
                       TotalYield = g.Sum(x => x.Yield)
                   })
                   .OrderBy(g => g.Month)
                   .ToList();

            ViewBag.Labels = monthlyYield.Select(m => m.Month).ToList();
            ViewBag.Data = monthlyYield.Select(m => m.TotalYield).ToList();

            return View();
        }
        [Route("scheduleDensity")]
        public IActionResult ScheduleDensity()
        {
            var data = _context.Schedules
                .Where(s => !s.IsDeleted && s.Season != null)
                .GroupBy(s => s.Season!.Name)
                .Select(g => new
                {
                    SeasonName = g.Key,
                    ScheduleCount = g.Count()
                }).ToList();

            ViewBag.Labels = data.Select(d => d.SeasonName).ToList();
            ViewBag.Data = data.Select(d => d.ScheduleCount).ToList();

            return View();
        }
        [Route("globalcalendar")]
        public IActionResult GlobalCalendar()
        {
            return View();
        }

        [HttpGet]
        [Route("get-calendar-events")]
        public IActionResult GetCalendarEvents()
        {
            var events = _context.Schedules
                .Where(s => !s.IsDeleted && s.Season != null && s.Season.ApplicationUser != null)
                .Select(s => new
                {
                    title = $"{s.Activity} ({s.Season.Name}) - {s.Season.ApplicationUser.UserName}",
                    start = s.Date.ToString("yyyy-MM-dd"),
                    season = s.Season.Name,
                    user = s.Season.ApplicationUser.UserName
                }).ToList();

            return Json(events);
        }
        [HttpGet("allseasons")]
        public async Task<IActionResult> AllSeasons()
        {
            var today = DateTime.Today;

            var seasons = await _context.Seasons
                .Where(s => !s.IsDeleted)
                .Select(s => new
                {
                    s.Name,
                    s.StartDate,
                    s.HarvestDate,
                    s.Area,
                    s.Notes,
                    CropName = s.Crop != null ? s.Crop.Name : "Không rõ",
                    Status = s.StartDate > today
                                ? "Chưa bắt đầu"
                                : (s.HarvestDate < today
                                    ? "Đã kết thúc"
                                    : "Đang diễn ra")
                })
                .ToListAsync();

            return View(seasons);
        }



    }
}