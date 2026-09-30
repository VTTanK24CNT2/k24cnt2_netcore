using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VttLesson12.Models;

namespace VttLesson12.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VttLesson12Context _context;

        public HomeController(ILogger<HomeController> logger, VttLesson12Context context)
        {
            _logger = logger;
            _context = context;
        }

        // Trang chủ
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .OrderByDescending(p => p.VttCreateDate)
                .Take(8)
                .ToListAsync();

            ViewBag.Banners = await _context.Banners
                .Where(b => b.Status == 1)
                .ToListAsync();

            return View(products);
        }

        // Bài 2: Hiển thị sản phẩm dạng grid trên HomeController
        public async Task<IActionResult> Product()
        {
            var products = await _context.Products
                .OrderByDescending(p => p.VttCreateDate)
                .ToListAsync();
            return View(products);
        }

        public IActionResult Privacy()
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
