namespace BtlLessonLab04.Models
{
    public class DataLocal
    {
        // Danh sách Category giả lập
        public static List<Category> _categories = new List<Category>()
        {
            new Category() { Id = 1, Name = "Lịch Sử" },
            new Category() { Id = 2, Name = "Truyện kể" },
            new Category() { Id = 3, Name = "SGK" }
        };

        // Danh sách Product giả lập
        public static List<Product> _products = new List<Product>()
        {
            new Product() {
                Id = 1, Name = "Thuyết minh", Price = 28000000, SalePrice = 25500000,
                Status = true, CreatedDate = DateTime.Now, Image = "/images/DaiViet.jpg",
                CategoryId = 1, Description = " Đại Việt Sử Ký Toàn Thư"
            },
            new Product() {
                Id = 2, Name = "Kể", Price = 45000000, SalePrice = 42000000,
                Status = true, CreatedDate = DateTime.Now, Image = "/images/demen.jpg",
                CategoryId = 2, Description = "Dế Mèn Phiêu lưu ký"
            },
            new Product() {
                Id = 3, Name = "SGK", Price = 490000, SalePrice = 53000,
                Status = true, CreatedDate = DateTime.Now, Image = "/images/TiengViet.jpg",
                CategoryId = 3, Description = "Tiếng Việt L1<tập 1>"
            },
            new Product() {
                Id = 4, Name = "Pháp luật", Price = 4090000, SalePrice = 93000,
                Status = true, CreatedDate = DateTime.Now, Image = "/images/LuatHs.jpg",
                CategoryId = 4, Description = "Luật pháp Việt Nam"
            }
        };

        public static List<Product> GetProducts() => _products;

        public static Product? GetProductById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public static List<Category> GetCategories() => _categories;
    }
}