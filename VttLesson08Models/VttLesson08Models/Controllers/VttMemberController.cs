
using Microsoft.AspNetCore.Mvc;
using VttLesson08Models.Models;

namespace VttLesson08Models.Controllers
{
    public class VttMemberController : Controller
    {
        // Mock data - VttMember
        private static List<VttMember> _members = new List<VttMember>()
        {
            new VttMember
            {
                VttMemberId = Guid.NewGuid().ToString(),
                VttUserName = "TaanVuX",
                VttPassword = "Password123!",
                VttFullName = "Vũ Trọng Tân",
                VttEmail = "Tancony1111@gmail.com"
            },

            new VttMember
            {
                VttMemberId = Guid.NewGuid().ToString(),
                VttUserName = "tranthib",
                VttPassword = "SecurePass456#",
                VttFullName = "Trần Thị B",
                VttEmail = "tranthib@outlook.com"
            },

            new VttMember
            {
                VttMemberId = Guid.NewGuid().ToString(),
                VttUserName = "levanc",
                VttPassword = "MyPassword789$",
                VttFullName = "Lê Văn C",
                VttEmail = "levanc@company.com"
            }
        };

        // GET: Danh sách thành viên
        public IActionResult Index()
        {
            return View(_members);
        }

        [HttpGet]
        public IActionResult VttCreate()
        {
            var member = new VttMember();
            return View(member);
        }

        [HttpPost]
        public IActionResult VttCreate(VttMember vttMember)
        {
            vttMember.VttMemberId = Guid.NewGuid().ToString();
            _members.Add(vttMember);

            return RedirectToAction("Index");
            // return View(vttMember);
        }

        [HttpGet]
        public IActionResult VttEdit(string id)
        {
            var member = _members
                .Where(x => x.VttMemberId.Equals(id))
                .FirstOrDefault();

            return View(member);
        }

        [HttpPost]
        public IActionResult VttEdit(string id, VttMember vttMember)
        {
            for (int i = 0; i < _members.Count; i++)
            {
                if (_members[i].VttMemberId == id)
                {
                    _members[i].VttUserName = vttMember.VttUserName;
                    _members[i].VttPassword = vttMember.VttPassword;
                    _members[i].VttFullName = vttMember.VttFullName;
                    _members[i].VttEmail = vttMember.VttEmail;

                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult VttDetails(string id)
        {
            var member = _members
                .Where(x => x.VttMemberId.Equals(id))
                .FirstOrDefault();

            return View(member);
        }

        [HttpGet]
        public IActionResult VttDelete(string id)
        {
            var member = _members
                .Where(x => x.VttMemberId.Equals(id))
                .FirstOrDefault();

            return View(member);
        }

        [HttpPost]
        public IActionResult VttDeleted(string id)
        {
            foreach (var item in _members)
            {
                if (item.VttMemberId.Equals(id))
                {
                    _members.Remove(item);
                    return RedirectToAction("Index");
                }
            }

            return View("VttDelete");
        }
    }
}

