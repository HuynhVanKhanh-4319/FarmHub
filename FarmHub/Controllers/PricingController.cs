using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarBok.Controllers
{
    [Route("pricing")]
    public class PricingController : Controller
    {
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
