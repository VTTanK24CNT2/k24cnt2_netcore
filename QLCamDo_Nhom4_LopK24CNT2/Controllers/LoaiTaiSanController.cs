using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCamDo_Nhom4_LopK24CNT2.Helpers;
using QLCamDo_Nhom4_LopK24CNT2.Models;

namespace QLCamDo_Nhom4_LopK24CNT2.Controllers
{
    [Authorize(Roles = "Chủ cửa hàng")]
    public class LoaiTaiSanController : Controller
    {
        private readonly QlcamDoNhom4LopK24cnt2Context _context;

        public LoaiTaiSanController(QlcamDoNhom4LopK24cnt2Context context)
        {
            _context = context;
        }

        // GET: /LoaiTaiSan
        public async Task<IActionResult> Index()
        {
            var list = await _context.LoaiTaiSans
                .Include(l => l.TaiSans)
                .OrderBy(l => l.TenLoai)
                .ToListAsync();
            return View(list);
        }

        // GET: /LoaiTaiSan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var loaiTaiSan = await _context.LoaiTaiSans
                .Include(l => l.TaiSans)
                .ThenInclude(t => t.MaHdNavigation)
                .FirstOrDefaultAsync(m => m.MaLoai == id);

            if (loaiTaiSan == null) return NotFound();

            return View(loaiTaiSan);
        }

        // GET: /LoaiTaiSan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /LoaiTaiSan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoai,TenLoai,LaiSuatNgay,SoNgayToiDa")] LoaiTaiSan loaiTaiSan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiTaiSan);
                await _context.SaveChangesAsync();
                AuditLogger.LogAction(User.Identity?.Name ?? "Admin", "Thêm loại tài sản", $"Thêm loại tài sản {loaiTaiSan.TenLoai}, lãi suất {loaiTaiSan.LaiSuatNgay}%/ngày");
                TempData["SuccessMessage"] = "Thêm loại tài sản thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(loaiTaiSan);
        }

        // GET: /LoaiTaiSan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var loaiTaiSan = await _context.LoaiTaiSans.FindAsync(id);
            if (loaiTaiSan == null) return NotFound();
            return View(loaiTaiSan);
        }

        // POST: /LoaiTaiSan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLoai,TenLoai,LaiSuatNgay,SoNgayToiDa")] LoaiTaiSan loaiTaiSan)
        {
            if (id != loaiTaiSan.MaLoai) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiTaiSan);
                    await _context.SaveChangesAsync();
                    AuditLogger.LogAction(User.Identity?.Name ?? "Admin", "Cập nhật loại tài sản", $"Cập nhật loại tài sản {loaiTaiSan.TenLoai}, lãi suất {loaiTaiSan.LaiSuatNgay}%/ngày");
                    TempData["SuccessMessage"] = "Cập nhật loại tài sản thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiTaiSanExists(loaiTaiSan.MaLoai))
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
            return View(loaiTaiSan);
        }

        // GET: /LoaiTaiSan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var loaiTaiSan = await _context.LoaiTaiSans
                .Include(l => l.TaiSans)
                .FirstOrDefaultAsync(m => m.MaLoai == id);
            if (loaiTaiSan == null) return NotFound();

            return View(loaiTaiSan);
        }

        // POST: /LoaiTaiSan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiTaiSan = await _context.LoaiTaiSans
                .Include(l => l.TaiSans)
                .FirstOrDefaultAsync(m => m.MaLoai == id);

            if (loaiTaiSan != null)
            {
                if (loaiTaiSan.TaiSans.Any())
                {
                    TempData["ErrorMessage"] = $"Không thể xóa loại tài sản '{loaiTaiSan.TenLoai}' vì đã có tài sản sử dụng loại này!";
                    return RedirectToAction(nameof(Index));
                }

                _context.LoaiTaiSans.Remove(loaiTaiSan);
                await _context.SaveChangesAsync();
                AuditLogger.LogAction(User.Identity?.Name ?? "Admin", "Xóa loại tài sản", $"Xóa loại tài sản {loaiTaiSan.TenLoai}");
                TempData["SuccessMessage"] = "Đã xóa loại tài sản thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LoaiTaiSanExists(int id)
        {
            return _context.LoaiTaiSans.Any(e => e.MaLoai == id);
        }
    }
}
