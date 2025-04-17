using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmHub.Areas.Admin.Controllers
{
    [Authorize]
    [Area("admin")]
    [Route("admin/dashboard")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
