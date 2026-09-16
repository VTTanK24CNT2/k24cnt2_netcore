using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VttLesson08Models.Models;

namespace VttLesson08Models.Controllers
{
    public class VttHomeController : Controller
    {
        private readonly ILogger<VttHomeController> _logger;

        public VttHomeController(ILogger<VttHomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(VttIndex));
        }

        public IActionResult VttIndex()
        {
            return View();
        }

        public IActionResult VttPrivacy()
        {
            return View();
        }

        public IActionResult VttAbout()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
