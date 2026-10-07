using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VttLesson14CoreA7.Models;

namespace VttLesson14CoreA7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Category
        public async Task<IActionResult> Index(string? searchString, byte? status)
        {
            var query = _context.Categories
                .Include(c => c.Products)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(c => c.Name.Contains(searchString));
            }

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            var categories = await query.OrderByDescending(c => c.Id).ToListAsync();

            ViewBag.SearchString = searchString;
            ViewBag.Status = status;
            ViewData["Title"] = "Quản lý Danh mục (Category)";
            return View(categories);
        }

        // GET: Admin/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Chi tiết danh mục: " + category.Name;
            return View(category);
        }

        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            var category = new Category
            {
                Status = 1,
                CreatedDate = DateTime.Now
            };
            ViewData["Title"] = "Thêm mới Danh mục";
            return View(category);
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,CreatedDate")] Category category)
        {
            // Kiểm tra ràng buộc: Tên không trùng (Unique)
            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "Tên danh mục này đã tồn tại trong hệ thống. Vui lòng nhập tên khác!");
            }

            if (ModelState.IsValid)
            {
                category.Name = category.Name.Trim();
                _context.Add(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã thêm mới danh mục \"{category.Name}\" thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = "Thêm mới Danh mục";
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Chỉnh sửa Danh mục: " + category.Name;
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,CreatedDate")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            // Kiểm tra trùng tên với danh mục khác
            if (await _context.Categories.AnyAsync(c => c.Id != id && c.Name.ToLower() == category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "Tên danh mục này đã trùng với một danh mục khác!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.Name = category.Name.Trim();
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Cập nhật danh mục \"{category.Name}\" thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Categories.AnyAsync(e => e.Id == category.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = "Chỉnh sửa Danh mục: " + category.Name;
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Xác nhận xóa Danh mục";
            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category != null)
            {
                if (category.Products != null && category.Products.Any())
                {
                    TempData["Error"] = $"Không thể xóa danh mục \"{category.Name}\" vì đang có {category.Products.Count} sản phẩm liên kết! Hãy chuyển hoặc xóa các sản phẩm đó trước.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã xóa danh mục \"{category.Name}\" thành công!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
