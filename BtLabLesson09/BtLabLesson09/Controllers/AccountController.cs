using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BtLabLesson09.Models;

namespace BtLabLesson09.Controllers
{
    public class AccountController : Controller
    {
        // Dữ liệu mẫu lưu trữ trong bộ nhớ
        private static readonly List<Account> accounts = new List<Account>
        {
            new Account
            {
                Id = 1,
                FullName = "Nguyễn Văn An",
                Email = "nguyenvanan@gmail.com",
                Phone = "0987654321",
                Address = "Số 12 Chùa Bộc, Đống Đa, Hà Nội",
                Avatar = "avatar1.png",
                Birthday = new DateTime(2000, 5, 15),
                Gender = "Nam",
                Password = "Password123@",
                Facebook = "https://facebook.com/nguyenvanan"
            },
            new Account
            {
                Id = 2,
                FullName = "Trần Thị Mai",
                Email = "tranthimai@gmail.com",
                Phone = "0912345678",
                Address = "Số 25 Cầu Giấy, Hà Nội",
                Avatar = "avatar2.png",
                Birthday = new DateTime(2002, 10, 20),
                Gender = "Nữ",
                Password = "Password456@",
                Facebook = "https://facebook.com/tranthimai"
            },
             new Account
            {
                Id = 3,
                FullName = "Vũ Trọng Tân",
                Email = "Tancony11@gmail.com",
                Phone = "0678910JQK",
                Address = "sn7 ngách 1, ngõ 30A, Lê Trọng Tấn, Hà Đông , Hà Nội",
                Avatar = "avatar2.png",
                Birthday = new DateTime(2006, 12, 12),
                Gender = "Nam",
                Password = "Password8910@",
                Facebook = "https://facebook.com/VuTrongTan"
             },
              new Account
               {
                Id = 4,
                FullName = "Hoàng Mạnh Huy",
                Email = "hoangmanhhuy@gmail.com",
                Phone = "0362926455",
                Address = "326 Bắc Linh Đàm, Hoàng Mai, Hà Nội",
                Avatar = "avatar2.png",
                Birthday = new DateTime(2005, 10, 20),
                Gender = "Nam",
                Password = "Password6666@",
                Facebook = "https://facebook.com/hoangmanhhuy"
              }
        };

        // GET: Account/Index
        public IActionResult Index()
        {
            return View(accounts);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            Account model = new Account();
            return View(model);
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Account model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Sinh Id tự động và thêm vào danh sách
            model.Id = accounts.Count > 0 ? accounts.Max(a => a.Id) + 1 : 1;
            accounts.Add(model);

            // Chuyển hướng về trang danh sách khi thêm thành công
            return RedirectToAction(nameof(Index));
        }

        // Action kiểm tra trùng số điện thoại (Remote Validation)
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyPhone(string phone)
        {
            // Kiểm tra số điện thoại mẫu hoặc đã tồn tại trong danh sách
            if (phone == "0987654321" || phone == "0912345678" || accounts.Any(a => a.Phone == phone))
            {
                return Json($"Số điện thoại {phone} đã được sử dụng.");
            }

            return Json(true);
        }
    }
}
