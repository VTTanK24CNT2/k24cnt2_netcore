using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VttLesson10EFDbFirst.Models;

namespace VttLesson10EFDbFirst.Controllers
{
    public class VttMembersController : Controller
    {
        private readonly VttLesson10EfdbContext _context;

        public VttMembersController(VttLesson10EfdbContext context)
        {
            _context = context;
        }

        // GET: VttMembers
        // Sử dụng AsNoTracking() để tối ưu hiệu năng truy vấn chỉ đọc (như trong bài giảng Slide 24)
        public async Task<IActionResult> Index(string? searchString, string? statusFilter)
        {
            var query = _context.VttMembers.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim();
                query = query.Where(m => (m.VttFullName != null && m.VttFullName.Contains(searchString))
                                      || (m.VttUserName != null && m.VttUserName.Contains(searchString))
                                      || (m.VttEmail != null && m.VttEmail.Contains(searchString))
                                      || (m.VttPhone != null && m.VttPhone.Contains(searchString)));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                if (statusFilter == "active")
                {
                    query = query.Where(m => m.VttStatus == true);
                }
                else if (statusFilter == "inactive")
                {
                    query = query.Where(m => m.VttStatus == false);
                }
            }

            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentStatus = statusFilter;

            var members = await query.OrderByDescending(m => m.Id).ToListAsync();
            return View(members);
        }

        // GET: VttMembers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vttMember = await _context.VttMembers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vttMember == null)
            {
                return NotFound();
            }

            return View(vttMember);
        }

        // GET: VttMembers/Create
        public IActionResult Create()
        {
            var newMember = new VttMember
            {
                VttStatus = true // Mặc định kích hoạt
            };
            return View(newMember);
        }

        // POST: VttMembers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VttUserName,VttPassword,VttFullName,VttEmail,VttPhone,VttStatus")] VttMember vttMember)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng username nếu cần
                if (!string.IsNullOrEmpty(vttMember.VttUserName))
                {
                    bool exists = await _context.VttMembers.AnyAsync(m => m.VttUserName == vttMember.VttUserName);
                    if (exists)
                    {
                        ModelState.AddModelError("VttUserName", "Tên đăng nhập này đã tồn tại trong hệ thống.");
                        return View(vttMember);
                    }
                }

                _context.Add(vttMember);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới thành viên thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(vttMember);
        }

        // GET: VttMembers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vttMember = await _context.VttMembers.FindAsync(id);
            if (vttMember == null)
            {
                return NotFound();
            }
            return View(vttMember);
        }

        // POST: VttMembers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,VttUserName,VttPassword,VttFullName,VttEmail,VttPhone,VttStatus")] VttMember vttMember)
        {
            if (id != vttMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra trùng username với thành viên khác
                    if (!string.IsNullOrEmpty(vttMember.VttUserName))
                    {
                        bool exists = await _context.VttMembers.AnyAsync(m => m.VttUserName == vttMember.VttUserName && m.Id != vttMember.Id);
                        if (exists)
                        {
                            ModelState.AddModelError("VttUserName", "Tên đăng nhập này đã được sử dụng bởi người khác.");
                            return View(vttMember);
                        }
                    }

                    _context.Update(vttMember);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật thông tin thành viên thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VttMemberExists(vttMember.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vttMember);
        }

        // GET: VttMembers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vttMember = await _context.VttMembers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vttMember == null)
            {
                return NotFound();
            }

            return View(vttMember);
        }

        // POST: VttMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var vttMember = await _context.VttMembers.FindAsync(id);
            if (vttMember != null)
            {
                _context.VttMembers.Remove(vttMember);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa thành viên thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VttMemberExists(long id)
        {
            return _context.VttMembers.Any(e => e.Id == id);
        }
    }
}
