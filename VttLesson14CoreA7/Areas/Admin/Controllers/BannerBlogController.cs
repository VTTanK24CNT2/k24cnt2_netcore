using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VttLesson14CoreA7.Models;

namespace VttLesson14CoreA7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerBlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BannerBlogController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/BannerBlog
        public async Task<IActionResult> Index(string? searchString, string? type)
        {
            var query = _context.BannerBlogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(b => b.Title.Contains(searchString) || b.Description.Contains(searchString));
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(b => b.Type == type);
            }

            var items = await query.OrderByDescending(b => b.Id).ToListAsync();

            ViewBag.SearchString = searchString;
            ViewBag.Type = type;
            ViewData["Title"] = "Quản lý Banner & Blog";
            return View(items);
        }

        // GET: Admin/BannerBlog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.BannerBlogs.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Chi tiết: " + item.Title;
            return View(item);
        }

        // GET: Admin/BannerBlog/Create
        public IActionResult Create()
        {
            var item = new BannerBlog
            {
                Type = "Banner",
                CreatedDate = DateTime.Now,
                IsActive = true
            };
            ViewData["Title"] = "Thêm mới Banner / Blog";
            return View(item);
        }

        // POST: Admin/BannerBlog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Image,Type,CreatedDate,IsActive")] BannerBlog bannerBlog, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "banners");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString("N").Substring(0, 8) + "_" + Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                bannerBlog.Image = "/images/banners/" + uniqueFileName;
            }

            if (ModelState.IsValid)
            {
                bannerBlog.Title = bannerBlog.Title.Trim();
                _context.Add(bannerBlog);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã thêm {bannerBlog.Type} \"{bannerBlog.Title}\" thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = "Thêm mới Banner / Blog";
            return View(bannerBlog);
        }

        // GET: Admin/BannerBlog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.BannerBlogs.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Chỉnh sửa: " + item.Title;
            return View(item);
        }

        // POST: Admin/BannerBlog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Image,Type,CreatedDate,IsActive")] BannerBlog bannerBlog, IFormFile? imageFile)
        {
            if (id != bannerBlog.Id)
            {
                return NotFound();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "banners");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString("N").Substring(0, 8) + "_" + Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                bannerBlog.Image = "/images/banners/" + uniqueFileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bannerBlog.Title = bannerBlog.Title.Trim();
                    _context.Update(bannerBlog);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Cập nhật {bannerBlog.Type} \"{bannerBlog.Title}\" thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.BannerBlogs.AnyAsync(e => e.Id == bannerBlog.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = "Chỉnh sửa: " + bannerBlog.Title;
            return View(bannerBlog);
        }

        // GET: Admin/BannerBlog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.BannerBlogs.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Xác nhận xóa Banner / Blog";
            return View(item);
        }

        // POST: Admin/BannerBlog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.BannerBlogs.FindAsync(id);
            if (item != null)
            {
                _context.BannerBlogs.Remove(item);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã xóa \"{item.Title}\" thành công!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
