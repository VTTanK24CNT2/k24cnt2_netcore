using Microsoft.AspNetCore.Mvc;
using VttLesson07Models.Models.DataModels;

namespace VttLesson07Models.Controllers
{
    public class VttMemberController : Controller
    {
        // Mock Data
        protected static List<VttMember> _members = new List<VttMember>
        {
             new VttMember
            {
                VttMemberId = Guid.NewGuid().ToString(),
                VttUserName = "chungtv",
                VttPassword = "123456",
                VttFullName = "Trịnh Văn Chung",
                VttEmail = "chungtrinhj@example.com"
            },
            new VttMember
            {
                VttMemberId = Guid.NewGuid().ToString(),
                VttUserName = "tranthibinh",
                VttPassword = "123456",
                VttFullName = "Trần Thị Bình",
                VttEmail = "tranthibinh@example.com"
            },
            new VttMember
            {
                VttMemberId = Guid.NewGuid().ToString(),
                VttUserName = "levancuong",
                VttPassword = "123456",
                VttFullName = "Lê Văn Cường",
                VttEmail = "levancuong@example.com"
            },
            new VttMember
            {
                VttMemberId = Guid.NewGuid().ToString(),
                VttUserName = "phamthiduyen",
                VttPassword = "123456",
                VttFullName = "Phạm Thị Duyên",
                VttEmail = "phamthiduyen@example.com"
            },
            new VttMember
            {
                VttMemberId = Guid.NewGuid().ToString(),
                VttUserName = "hoangminhduc",
                VttPassword = "123456",
                VttFullName = "Hoàng Minh Đức",
                VttEmail = "hoangminhduc@example.com"
            }
        };

        public IActionResult Index()
        {
            return View(_members);
        }
        public IActionResult GetMember()
        {
            var member = new VttMember
            {
                VttMemberId = Guid.NewGuid().ToString(),
                VttUserName = "TaanVuX",
                VttPassword = "password123",
                VttFullName = "Vũ Trọng Tân",
                VttEmail = "vutrongtan1212@gmail.com"
            };
            ViewBag.Member = member;
            return View(member);
        }

        // Đưa dữ liệu dạng List ra View
        public IActionResult GetMembers()
        {
            // Lấy từ mock data
            ViewBag.Members = _members;
            return View();

        }

        // GET: Create member
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create member
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VttMember member)
        {
            if (ModelState.IsValid)
            {
                member.VttMemberId = Guid.NewGuid().ToString();
                _members.Add(member);
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
    }
}