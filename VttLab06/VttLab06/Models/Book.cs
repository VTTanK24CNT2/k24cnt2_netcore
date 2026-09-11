using Microsoft.AspNetCore.Mvc.Rendering;

namespace VttLab06.Models
{
    public class Book
    {
        public int VttId { get; set; }
        public string VttTitle { get; set; }
        public int VttAuthorID { get; set; }
        public int VttGenreId { get; set; }
        public string VttImages { get; set; }
        public float VttPrice { get; set; }
        public int VttTotalPage { get; set; }
        public string VttSumary { get; set; }

        public List<Book> GetBookList()

        {
            List<Book> books = new List<Book>()

            {
                 new Book(){
                    VttId =1,
                    VttTitle = "AOV",
                    VttAuthorID = 1,
                    VttGenreId = 1,
                    VttImages = "/images/lq.jpg",
                    VttPrice = 500000,
                    VttTotalPage =250,
                    VttSumary ="",
                },
                 new Book(){
                    VttId =2,
                    VttTitle = "By kip luen rong 2",
                    VttAuthorID = 2,
                    VttGenreId = 2,
                    VttImages = "/images/2rong.jpg",
                    VttPrice = 220000,
                    VttTotalPage =250,
                    VttSumary ="",
                },
                 new Book(){
                    VttId =3,
                    VttTitle = "By kip luen rong 4",
                    VttAuthorID = 3,
                    VttGenreId = 3,
                    VttImages = "/images/4rong.jpg",
                    VttPrice = 549000,
                    VttTotalPage =250,
                    VttSumary ="",
                },
                 new Book(){
                    VttId =4,
                    VttTitle = "Meo Trang",
                    VttAuthorID = 4,
                    VttGenreId = 3,
                    VttImages = "/images/MeoMeo.jpg",
                    VttPrice = 150000,
                    VttTotalPage =250,
                    VttSumary ="",
                },
                 new Book(){
                    VttId =5,
                    VttTitle = "Đại Việt Sử Ký Toàn Thư",
                    VttAuthorID = 4,
                    VttGenreId = 6,
                    VttImages = "/images/DaiViet.jpg",
                    VttPrice = 1550000,
                    VttTotalPage =250,
                    VttSumary ="",
                },
                 new Book(){
                    VttId =6,
                    VttTitle = "Dế MÈN Phiêu Lưu Ký",
                    VttAuthorID = 6,
                    VttGenreId = 6,
                    VttImages = "/images/demen.jpg",
                    VttPrice = 2150000,
                    VttTotalPage =2150,
                    VttSumary ="",
                },
                 new Book(){
                    VttId =7,
                    VttTitle = "Sách Luật Sửa Đổi BS 2014",
                    VttAuthorID = 8,
                    VttGenreId = 9,
                    VttImages = "/images/LuatHS.jpg",
                    VttPrice = 4490000,
                    VttTotalPage =750,
                    VttSumary ="",
                },
                 new Book(){
                    VttId =8,
                    VttTitle = "Tiếng Việt 1(tập 1)",
                    VttAuthorID = 8,
                    VttGenreId = 8,
                    VttImages = "/images/TiengViet.jpg",
                    VttPrice = 1150000,
                    VttTotalPage =255,
                    VttSumary ="",
                },
                 new Book(){
                    VttId =9,
                    VttTitle = "Khám phá cố đô Huế",
                    VttAuthorID = 8,
                    VttGenreId = 9,
                    VttImages = "/images/nk.jpg",
                    VttPrice = 650000,
                    VttTotalPage =250,
                    VttSumary ="",
                },
            };

            return books;
        }
        public Book GetBookById(int id)
        {
            Book book = this.GetBookList().FirstOrDefault(b => b.VttId == id );
            return book;
        }
        public List<SelectListItem> Authors { get; } = new List<SelectListItem>
            { 
                new SelectListItem {Value="1", Text="Garena"},
                new SelectListItem {Value="2", Text="Alectder"},
                new SelectListItem {Value="3", Text="Ali maxcimuoop"},
                new SelectListItem {Value="4", Text="Vu Tong Tan"},
                new SelectListItem {Value="5", Text="Nha xuat ban Kim Dong"},
                new SelectListItem {Value="6", Text="To Hoai"},
                new SelectListItem {Value="7", Text="Bo Cong An"},
                new SelectListItem {Value="8", Text="Nha Xuat Ban Kim Dong"},
                new SelectListItem {Value="9", Text="Nhiep anh gia TaanVuX"},

            };
        public List<SelectListItem> Genres { get; } = new List<SelectListItem>
            {
                new SelectListItem {Value="1", Text="Game"},
                new SelectListItem {Value="2", Text="Phim 14+"},
                new SelectListItem {Value="3", Text="Truyện tranh"},
                new SelectListItem {Value="4", Text="Ảnh "},

                new SelectListItem {Value="5", Text="Thịnh Vượng"},
                new SelectListItem {Value="6", Text="Truyện tự sự  "},
                new SelectListItem {Value="7", Text="Luật Pháp "},
                new SelectListItem {Value="8", Text="Sách Giáo Khoa "},
                new SelectListItem {Value="9", Text="Khám phá "},
            };
    }

}
