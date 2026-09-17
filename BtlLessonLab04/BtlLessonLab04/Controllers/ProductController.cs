using BtlLessonLab04.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BtlLessonLab04.Controllers
{
    public class ProductController : Controller
    {
        // GET: ProductController
        public ActionResult Index()
        {
            var products = DataLocal.GetProducts();
            ViewBag.Categories = DataLocal.GetCategories();
            return View(products);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var product = DataLocal.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryName = DataLocal.GetCategories()
                                    .Find(c => c.Id == product.CategoryId)?.Name;
            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            // Truyền danh sách Category vào SelectList cho Combobox
            ViewBag.CategoryId = new SelectList(DataLocal.GetCategories(), "Id", "Name");
            return View(new Product());
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model, IFormFile? imageFile)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(imageFolder))
                    {
                        Directory.CreateDirectory(imageFolder);
                    }
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(imageFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }

                    model.Image = "/images/" + fileName;
                }
                else if (string.IsNullOrEmpty(model.Image))
                {
                    model.Image = "/images/DaiViet.jpg";
                }

                model.Id = DataLocal._products.Any() ? DataLocal._products.Max(p => p.Id) + 1 : 1;
                model.CreatedDate = DateTime.Now;

                DataLocal._products.Add(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.CategoryId = new SelectList(DataLocal.GetCategories(), "Id", "Name", model.CategoryId);
                return View(model);
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var product = DataLocal.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(DataLocal.GetCategories(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product model, IFormFile? imageFile)
        {
            try
            {
                var product = DataLocal.GetProductById(id);
                if (product != null)
                {
                    product.Name = model.Name;
                    product.Price = model.Price;
                    product.SalePrice = model.SalePrice;
                    product.Status = model.Status;
                    product.CategoryId = model.CategoryId;
                    product.Description = model.Description;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                        if (!Directory.Exists(imageFolder))
                        {
                            Directory.CreateDirectory(imageFolder);
                        }
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(imageFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            imageFile.CopyTo(stream);
                        }

                        product.Image = "/images/" + fileName;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.CategoryId = new SelectList(DataLocal.GetCategories(), "Id", "Name", model.CategoryId);
                return View(model);
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            var product = DataLocal.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryName = DataLocal.GetCategories()
                                    .Find(c => c.Id == product.CategoryId)?.Name;
            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Product model)
        {
            try
            {
                var product = DataLocal.GetProductById(id);
                if (product != null)
                {
                    DataLocal._products.Remove(product);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}