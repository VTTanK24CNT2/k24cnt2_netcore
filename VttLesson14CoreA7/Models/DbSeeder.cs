using System;
using System.Linq;

namespace VttLesson14CoreA7.Models
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Tự động tạo bảng nếu chưa có
            context.Database.EnsureCreated();

            // Nếu đã có danh mục thì không cần seed lại
            if (!context.Categories.Any())
            {
                var catPhone = new Category
                {
                    Name = "Điện thoại & Smartphone",
                    Status = 1,
                    CreatedDate = DateTime.Now.AddDays(-20)
                };
                var catLaptop = new Category
                {
                    Name = "Laptop & Máy tính xách tay",
                    Status = 1,
                    CreatedDate = DateTime.Now.AddDays(-18)
                };
                var catWatch = new Category
                {
                    Name = "Đồng hồ thông minh",
                    Status = 1,
                    CreatedDate = DateTime.Now.AddDays(-15)
                };
                var catAccessory = new Category
                {
                    Name = "Phụ kiện cao cấp",
                    Status = 1,
                    CreatedDate = DateTime.Now.AddDays(-10)
                };

                context.Categories.AddRange(catPhone, catLaptop, catWatch, catAccessory);
                context.SaveChanges();

                var products = new[]
                {
                    new Product
                    {
                        Name = "iPhone 16 Pro Max 256GB Titan Sa Mạc",
                        Price = 34990000,
                        CategoryId = catPhone.Id,
                        Image = "https://cdn.tgdd.vn/Products/Images/42/329149/iphone-16-pro-max-titan-sa-mac-thumb-600x600.jpg"
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy S24 Ultra 5G 256GB",
                        Price = 28990000,
                        CategoryId = catPhone.Id,
                        Image = "https://cdn.tgdd.vn/Products/Images/42/307174/samsung-galaxy-s24-ultra-xam-thumb-600x600.jpg"
                    },
                    new Product
                    {
                        Name = "MacBook Air M3 13.6 inch 16GB / 512GB",
                        Price = 28490000,
                        CategoryId = catLaptop.Id,
                        Image = "https://cdn.tgdd.vn/Products/Images/44/322610/macbook-air-13-inch-m3-midnight-thumb-600x600.jpg"
                    },
                    new Product
                    {
                        Name = "Laptop ASUS Zenbook 14 OLED UX3405",
                        Price = 26990000,
                        CategoryId = catLaptop.Id,
                        Image = "https://cdn.tgdd.vn/Products/Images/44/322047/asus-zenbook-14-oled-ux3405ma-oledu715w-thumb-600x600.jpg"
                    },
                    new Product
                    {
                        Name = "Apple Watch Series 10 Nhôm GPS 46mm",
                        Price = 11490000,
                        CategoryId = catWatch.Id,
                        Image = "https://cdn.tgdd.vn/Products/Images/7077/329584/apple-watch-s10-46mm-vien-nhom-day-cao-su-den-thumb-600x600.jpg"
                    },
                    new Product
                    {
                        Name = "Tai nghe chống ồn Sony WH-1000XM5",
                        Price = 6990000,
                        CategoryId = catAccessory.Id,
                        Image = "https://cdn.tgdd.vn/Products/Images/54/281691/tai-nghe-chup-tai-sony-wh-1000xm5-thumb-600x600.jpg"
                    },
                    new Product
                    {
                        Name = "Bàn phím cơ không dây NuPhy Air75 V2",
                        Price = 2850000,
                        CategoryId = catAccessory.Id,
                        Image = "https://cdn.tgdd.vn/Products/Images/54/236016/bluetooth-airpods-pro-magsafe-charge-apple-mlwk3-thumb-600x600.jpg"
                    }
                };
                context.Products.AddRange(products);
                context.SaveChanges();
            }

            if (!context.BannerBlogs.Any())
            {
                var banners = new[]
                {
                    new BannerBlog
                    {
                        Title = "Đại tiệc công nghệ Chào năm học mới - Giảm tới 40%",
                        Description = "Cơ hội sở hữu laptop, smartphone và phụ kiện cao cấp chính hãng với giá tốt nhất thị trường cùng gói bảo hành VIP 24 tháng.",
                        Image = "https://images.unsplash.com/photo-1531297484001-80022131f5a1?w=1200",
                        Type = "Banner",
                        CreatedDate = DateTime.Now.AddDays(-15),
                        IsActive = true
                    },
                    new BannerBlog
                    {
                        Title = "Khai mở kỷ nguyên AI cùng thế hệ vi xử lý mới",
                        Description = "Tìm hiểu sức mạnh phần cứng tích hợp trí tuệ nhân tạo trên các dòng máy tính xách tay và smartphone hàng đầu hiện nay.",
                        Image = "https://images.unsplash.com/photo-1518770660439-4636190af475?w=1200",
                        Type = "Banner",
                        CreatedDate = DateTime.Now.AddDays(-10),
                        IsActive = true
                    },
                    new BannerBlog
                    {
                        Title = "Top 5 ultrabook mỏng nhẹ, pin trâu đáng sở hữu cho sinh viên CNTT",
                        Description = "Đánh giá chi tiết 5 chiếc laptop văn phòng và lập trình với màn hình chuẩn màu sắc nét, bàn phím gõ êm và thời lượng pin trên 10 tiếng.",
                        Image = "https://images.unsplash.com/photo-1498050108023-c5249f4df085?w=800",
                        Type = "Blog",
                        CreatedDate = DateTime.Now.AddDays(-5),
                        IsActive = true
                    },
                    new BannerBlog
                    {
                        Title = "Kinh nghiệm chọn phụ kiện công nghệ nâng cấp góc học tập",
                        Description = "Bí quyết setup bàn học công thái học, chọn màn hình rời bảo vệ mắt, chuột công thái học và đèn bàn chất lượng cao.",
                        Image = "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?w=800",
                        Type = "Blog",
                        CreatedDate = DateTime.Now.AddDays(-2),
                        IsActive = true
                    }
                };
                context.BannerBlogs.AddRange(banners);
                context.SaveChanges();
            }
        }
    }
}
