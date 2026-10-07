using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VttLesson14CoreA7.Models;

namespace VttLesson14CoreA7.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Where(c => c.Status == 1).ToListAsync();
            var products = await _context.Products.Include(p => p.Category).Take(8).ToListAsync();
            var banners = await _context.BannerBlogs.Where(b => b.IsActive).ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.Products = products;
            ViewBag.Banners = banners;

            ViewData["Title"] = "Trang chủ - Cửa hàng Công nghệ VTT";
            return View();
        }

        // Demo bài 2: Sử dụng Layout phụ _main.cshtml
        public IActionResult DemoMainLayout()
        {
            ViewData["Title"] = "Demo Sử Dụng _main.cshtml (Bài 2)";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Title"] = "Giới thiệu đồ án Lab 07";
            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["Title"] = "Chính sách bảo mật";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
