using Microsoft.AspNetCore.Mvc;
using VttLesson13.Models;

namespace VttLesson13.Controllers
{
    public class ProductsController : Controller
    {
        // Danh sách sản phẩm mẫu để phục vụ demo trực quan
        private static readonly List<Product> _products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Laptop ASUS Zenbook 14 OLED",
                Price = 24990000,
                Category = "Laptop",
                Description = "Màn hình OLED 2.8K 120Hz, chip Intel Core Ultra 7 mạnh mẽ, thiết kế siêu mỏng nhẹ chỉ 1.2kg.",
                ImageUrl = "bi-laptop",
                IsFeatured = true
            },
            new Product
            {
                Id = 2,
                Name = "Điện thoại iPhone 16 Pro Max",
                Price = 34500000,
                Category = "Điện thoại",
                Description = "Khung viền Titan sa mạc, chip A18 Pro, nút điều khiển camera mới, thời lượng pin ấn tượng.",
                ImageUrl = "bi-phone",
                IsFeatured = true
            },
            new Product
            {
                Id = 3,
                Name = "Bàn phím cơ không dây NuPhy Air75",
                Price = 2850000,
                Category = "Phụ kiện",
                Description = "Switch Gateron Low Profile, hỗ trợ kết nối đa thiết bị Bluetooth 5.0, 2.4G và Type-C.",
                ImageUrl = "bi-keyboard",
                IsFeatured = false
            },
            new Product
            {
                Id = 4,
                Name = "Chuột không dây Logitech MX Master 3S",
                Price = 2190000,
                Category = "Phụ kiện",
                Description = "Cảm biến 8000 DPI theo dõi trên mọi bề mặt, con lăn điện từ MagSpeed êm ái.",
                ImageUrl = "bi-mouse",
                IsFeatured = true
            },
            new Product
            {
                Id = 5,
                Name = "Tai nghe Sony WH-1000XM5",
                Price = 7990000,
                Category = "Âm thanh",
                Description = "Khả năng chống ồn chủ động đỉnh cao, âm thanh chuẩn Hi-Res Audio, thời lượng pin 30 giờ.",
                ImageUrl = "bi-headphones",
                IsFeatured = false
            },
            new Product
            {
                Id = 6,
                Name = "Màn hình Dell UltraSharp 27 4K",
                Price = 14200000,
                Category = "Màn hình",
                Description = "Độ phân giải 4K IPS Black, chuẩn màu 98% DCI-P3, cổng kết nối Type-C sạc 90W tiện lợi.",
                ImageUrl = "bi-display",
                IsFeatured = true
            }
        };

        // GET: /Products
        public IActionResult Index(string? category)
        {
            var list = string.IsNullOrEmpty(category)
                ? _products
                : _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

            ViewData["Title"] = "Danh mục sản phẩm";
            ViewData["CurrentCategory"] = category ?? "Tất cả";
            return View(list);
        }

        // GET: /Products/Details/1
        public IActionResult Details(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["Title"] = product.Name;
            return View(product);
        }

        // GET: /Products/SpecialDeals (Demo trang dùng Section Banner riêng)
        public IActionResult SpecialDeals()
        {
            var hotProducts = _products.Where(p => p.IsFeatured).ToList();
            ViewData["Title"] = "Sản phẩm khuyến mãi sốc";
            return View(hotProducts);
        }
    }
}
