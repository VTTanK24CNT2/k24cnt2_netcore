using Microsoft.AspNetCore.Mvc;
using vttlesson02theory.Models;

namespace vttlesson02theory.Controllers
{
    public class vttproductController : Controller
    {
        public IActionResult vttIndex()
        {
            // Dữ liệu lưu trữ trong đối tương
            ViewBag.name = "Vũ Trọng Tân";
            ViewData["productVD"] = "laptop HPpro";
            TempData["UNI"] = "Trường Đh Nguyễn Trãi-NTU";

            return View();
        }

        public IActionResult GetProduct()
        {
            // Tạo mock data product
            vttproduct vttproduct = new vttproduct()
            {
                ProductID = "2410900069",
                ProductName = "Vũ Trọng Tân",
                YearRelease=2006,
                Price = 1000
            };

            ViewBag.product = vttproduct;
            ViewData["product"] = vttproduct;
                
            return View("product");
        }
    }
}
