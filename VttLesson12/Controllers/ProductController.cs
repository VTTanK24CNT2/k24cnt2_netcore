using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VttLesson12.Models;

namespace VttLesson12.Controllers
{
    public class ProductController : Controller
    {
        private readonly VttLesson12Context _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(VttLesson12Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.OrderByDescending(p => p.VttCreateDate).ToListAsync();
            return View(products);
        }

        // GET: Product/Details/P01
        public async Task<IActionResult> Details(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.VttId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            var model = new Product
            {
                VttCreateDate = DateTime.Now,
                VttStatus = "Còn hàng"
            };
            return View(model);
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            // Kiểm tra trùng mã sản phẩm
            if (await _context.Products.AnyAsync(p => p.VttId == product.VttId))
            {
                ModelState.AddModelError("VttId", "Mã sản phẩm này đã tồn tại, vui lòng chọn mã khác.");
            }

            if (ModelState.IsValid)
            {
                // Xử lý upload hình ảnh
                if (product.ImageUpload != null && product.ImageUpload.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString().Substring(0, 8) + "_" + Path.GetFileName(product.ImageUpload.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageUpload.CopyToAsync(fileStream);
                    }

                    product.VttImages = "/images/products/" + uniqueFileName;
                }

                product.VttCreateDate = DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm mới sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Edit/P01
        public async Task<IActionResult> Edit(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/P01
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Product product)
        {
            if (id != product.VttId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.VttId == id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    // Nếu có ảnh mới upload
                    if (product.ImageUpload != null && product.ImageUpload.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString().Substring(0, 8) + "_" + Path.GetFileName(product.ImageUpload.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await product.ImageUpload.CopyToAsync(fileStream);
                        }

                        product.VttImages = "/images/products/" + uniqueFileName;
                    }
                    else
                    {
                        // Giữ lại ảnh cũ
                        product.VttImages = existingProduct.VttImages;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật sản phẩm thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.VttId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Delete/P01
        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.VttId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/P01
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa sản phẩm thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.VttId == id);
        }
    }
}
