using Microsoft.AspNetCore.Mvc;
using VttLesson04Lab.Models;

namespace VttLesson04Lab.Controllers
{
    public class VttAccountController : Controller
    {
        private readonly List<VttAccount> vttAccounts = new()
        {
        new VttAccount
        {
            Id = 1,
            Name = "Nguyễn Văn An",
            Email = "nguyenvanan@gmail.com",
            Phone = "0901234567",
            Avatar = "/images/1.jpg",
            Address = "Hà Nội",
            Bio = "Sinh viên yêu thích công nghệ và lập trình C#",
            Gender = 1,
            Birthday = new DateTime(2003, 5, 15)
        },

        new VttAccount
        {
            Id = 2,
            Name = "Trần Thị Mai",
            Email = "tranthimai@gmail.com",
            Phone = "0912345678",
            Avatar = "/images/2.jpg",
            Address = "Ninh Bình",
            Bio = "Yêu thích du lịch, âm nhạc và học ngoại ngữ",
            Gender = 2,
            Birthday = new DateTime(2004, 8, 20)
        },

        new VttAccount
        {
            Id = 3,
            Name = "Lê Minh Đức",
            Email = "leminhduc@gmail.com",
            Phone = "0987654321",
            Avatar = "/images/3.jpg",
            Address = "Hải Phòng",
            Bio = "Lập trình viên trẻ, đam mê phát triển phần mềm",
            Gender = 1,
            Birthday = new DateTime(2002, 12, 10)
        },

        new VttAccount
        {
            Id = 4,
            Name = "Phạm Ngọc Anh",
            Email = "phamngocanh@gmail.com",
            Phone = "0934567890",
            Avatar = "/images/4.jpg",
            Address = "Đà Nẵng",
            Bio = "Đam mê thiết kế giao diện và phát triển website",
            Gender = 2,
            Birthday = new DateTime(2003, 3, 25)
        },

        new VttAccount
        {
            Id = 5,
            Name = "Vũ Hoàng Nam",
            Email = "vuhoangnam@gmail.com",
            Phone = "0967890123",
            Avatar = "/images/3.jpg",
            Address = "Thành phố Hồ Chí Minh",
            Bio = "Yêu thích bóng đá, công nghệ và lập trình .NET",
            Gender = 1,
            Birthday = new DateTime(2001, 11, 5)
        }
            };
        public IActionResult Index()
        {
            ViewBag.VttAccounts = vttAccounts;
            return View();
        }
        [Route("ho-so-cua-toi", Name = "Vttprofile")]
        public IActionResult VttProfile(int? id)
        {
            VttAccount vttAccount = new VttAccount
            {
                Id = 5,
                Name = "Vũ Hoàng Nam",
                Email = "vuhoangnam@gmail.com",
                Phone = "0967890123",
                Avatar = "/images/3.jpg",
                Address = "Thành phố Hồ Chí Minh",
                Bio = "Yêu thích bóng đá, công nghệ và lập trình .NET",
                Gender = 1,
                Birthday = new DateTime(2001, 11, 5)
            };
            if (vttAccount != null)

                 vttAccount = vttAccounts.FirstOrDefault(x => x.Id == id);
            ViewBag.VttAccount = vttAccount;
            return View();
        }
    }
}
