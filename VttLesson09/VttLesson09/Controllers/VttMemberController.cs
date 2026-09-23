using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VttLesson09.Models.DataModels;
using VttLesson09.Models.DataViewModels;

namespace VttLesson09.Controllers
{
    public class VttMemberController : Controller
    {
        private static List<VttMember> _vttMembers = new List<VttMember>(); 
        // GET: VttMemberController
        public ActionResult Index()
        {
            return View();
        }

        // GET: VttMemberController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VttMemberController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VttMemberController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VttMemberRegister vttMember)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vttMember);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VttMemberController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VttMemberController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VttMemberController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VttMemberController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
