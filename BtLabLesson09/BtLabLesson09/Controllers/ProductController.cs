using BtLabLesson09.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BtLabLesson09.Controllers
{
    public class ProductController : Controller
    {
        // Danh sách Category mẫu
        public static readonly List<Category> Categories = new List<Category>
        {
            new Category { Id = 1, Name = "Đại Việt sử ký toàn thư"},
            new Category { Id = 2, Name = "Dế mèn phiêu lưu ký" },
            new Category { Id = 3, Name = "Pháp Luật" },
            new Category { Id = 4, Name = "Nhật ký thường ngày" }
        };

        // Danh sách Product mẫu
        public static readonly List<Product> Products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Đại Việt sử ký toàn thư",
                Image = "/products/DaiViet.jpg",
                Price = 30000000,
                SalePrice = 28000000,
                Description = "giáo dục người Việt Bên bển",
                CategoryId = 2
            },
            new Product
            {
                Id = 2,
                Name = "Dế mèn phiêu lưu ký",
                Image = "/products/demen.jpg",
                Price = 34000000,
                SalePrice = 31500000,
                Description = "truyện kể",
                CategoryId = 1
            },
            new Product
            {
                Id = 3,
                Name = "Pháp Luật",
                Image = "/products/LuatHS.jpg",
                Price = 8000000,
                SalePrice = 7500000,
                Description = "Nghịch là tù ngay",
                CategoryId = 3
            },
            new Product

             {
                Id = 4,
                Name = "Nhật ký",
                Image = "/products/nk.jpg",
                Price = 4500000,
                SalePrice = 3600000,
                Description = "Nhật ký thường ngày",
                CategoryId = 3
            }
        };

        // Helper nạp dropdown Category
        private void PopulateCategoriesDropDownList(object? selectedCategory = null)
        {
            ViewBag.CategoryId = new SelectList(Categories, "Id", "Name", selectedCategory);
        }

        // 1. Danh sách sản phẩm
        // GET: Product/Index
        public IActionResult Index()
        {
            ViewBag.Categories = Categories.ToDictionary(c => c.Id, c => c.Name);
            return View(Products);
        }

        // 2. Chi tiết sản phẩm
        // GET: Product/Details/5
        public IActionResult Details(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var category = Categories.FirstOrDefault(c => c.Id == product.CategoryId);
            ViewBag.CategoryName = category != null ? category.Name : "Không xác định";
            return View(product);
        }

        // 3. Thêm mới sản phẩm
        // GET: Product/Create
        public IActionResult Create()
        {
            PopulateCategoriesDropDownList();
            var product = new Product
            {
                Price = 100000f,
                SalePrice = 95000f,
                Image = "/products/"
            };
            return View(product);
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            // Kiểm tra CategoryId có tồn tại trong danh sách Categories hay không
            if (!Categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Danh mục đã chọn không hợp lệ hoặc không có trong hệ thống.");
            }

            if (!ModelState.IsValid)
            {
                PopulateCategoriesDropDownList(model.CategoryId);
                return View(model);
            }

            model.Id = Products.Count > 0 ? Products.Max(p => p.Id) + 1 : 1;
            Products.Add(model);

            TempData["SuccessMessage"] = $"Thêm sản phẩm '{model.Name}' thành công!";
            return RedirectToAction(nameof(Index));
        }

        // 4. Chỉnh sửa sản phẩm
        // GET: Product/Edit/5
        public IActionResult Edit(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            PopulateCategoriesDropDownList(product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            // Kiểm tra CategoryId
            if (!Categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Danh mục đã chọn không hợp lệ hoặc không có trong hệ thống.");
            }

            if (!ModelState.IsValid)
            {
                PopulateCategoriesDropDownList(model.CategoryId);
                return View(model);
            }

            var existing = Products.FirstOrDefault(p => p.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin
            existing.Name = model.Name;
            existing.Image = model.Image;
            existing.Price = model.Price;
            existing.SalePrice = model.SalePrice;
            existing.Description = model.Description;
            existing.CategoryId = model.CategoryId;

            TempData["SuccessMessage"] = $"Cập nhật sản phẩm '{model.Name}' thành công!";
            return RedirectToAction(nameof(Index));
        }

        // 5. Xóa sản phẩm
        // GET: Product/Delete/5
        public IActionResult Delete(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var category = Categories.FirstOrDefault(c => c.Id == product.CategoryId);
            ViewBag.CategoryName = category != null ? category.Name : "Không xác định";
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                Products.Remove(product);
                TempData["SuccessMessage"] = $"Đã xóa sản phẩm #{id} thành công!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
