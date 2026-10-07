using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VttLesson14CoreA7.Models;

namespace VttLesson14CoreA7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categoryCount = await _context.Categories.CountAsync();
            var productCount = await _context.Products.CountAsync();
            var bannerCount = await _context.BannerBlogs.CountAsync();
            var totalProductValue = await _context.Products.SumAsync(p => (double?)p.Price) ?? 0;

            ViewBag.CategoryCount = categoryCount;
            ViewBag.ProductCount = productCount;
            ViewBag.BannerCount = bannerCount;
            ViewBag.TotalProductValue = totalProductValue;

            // Lấy 5 sản phẩm mới nhất
            var recentProducts = await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToListAsync();

            // Lấy 5 bài viết / banner mới nhất
            var recentBanners = await _context.BannerBlogs
                .OrderByDescending(b => b.Id)
                .Take(5)
                .ToListAsync();

            ViewBag.RecentProducts = recentProducts;
            ViewBag.RecentBanners = recentBanners;

            ViewData["Title"] = "Bảng điều khiển (Dashboard)";
            return View();
        }
    }
}
