using FarmHub.Data;
using FarmHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FarmHub.Controllers
{
    [Authorize]
    [Route("crop")]
    public class CropController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public CropController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("index")]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User); 
            var crops = await _context.Crops
                .Where(c => !c.IsDeleted && c.ApplicationUserId == userId)
                .ToListAsync();
            return View(crops);
        }



        [HttpGet]
        [Route("create")]
        public IActionResult Create() => View();

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(Crop crop)
        {
            if (ModelState.IsValid)
            {
               
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    crop.ApplicationUserId = currentUser.Id; 
                }
                crop.IsDeleted = false;

                _context.Crops.Add(crop);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(crop);
        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var crop = _context.Crops.Find(id);
            if (crop == null) return NotFound();
            return View(crop);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Crop crop)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    crop.ApplicationUserId = currentUser.Id;
                }

                _context.Crops.Update(crop);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(crop);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var crop = await _context.Crops.FindAsync(id);
            if (crop == null) return NotFound();

            crop.IsDeleted = true;
            _context.Update(crop);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
