using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCamDo_Nhom4_LopK24CNT2.Helpers;
using QLCamDo_Nhom4_LopK24CNT2.Models;
using QLCamDo_Nhom4_LopK24CNT2.Models.ViewModels;

namespace QLCamDo_Nhom4_LopK24CNT2.Controllers
{
    [Authorize]
    public class TaiSanController : Controller
    {
        private readonly QlcamDoNhom4LopK24cnt2Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TaiSanController(QlcamDoNhom4LopK24cnt2Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /TaiSan
        public async Task<IActionResult> Index(string? search, int? maLoai, int page = 1, int pageSize = 10)
        {
            var query = _context.TaiSans
                .Include(t => t.MaHdNavigation)
                    .ThenInclude(h => h.MaKhNavigation)
                .Include(t => t.MaLoaiNavigation)
                .AsQueryable();

            if (User.IsInRole("Khách hàng"))
            {
                var userMaKh = HttpContext.Session.GetInt32("MaKH");
                if (userMaKh == null)
                {
                    var username = User.Identity?.Name;
                    var acc = await _context.TaiKhoans.FirstOrDefaultAsync(t => t.TenDangNhap == username);
                    userMaKh = acc?.MaKh;
                }
                if (userMaKh.HasValue)
                {
                    query = query.Where(t => t.MaHdNavigation.MaKh == userMaKh.Value);
                }
                else
                {
                    query = query.Where(t => false);
                }
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(t => t.TenTaiSan.Contains(search) || t.MaHdNavigation.MaKhNavigation.HoTen.Contains(search));
            }

            if (maLoai.HasValue && maLoai.Value > 0)
            {
                query = query.Where(t => t.MaLoai == maLoai.Value);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages == 0) totalPages = 1;
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var items = await query
                .OrderByDescending(t => t.MaTs)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentMaLoai = maLoai;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.LoaiTaiSans = new SelectList(await _context.LoaiTaiSans.ToListAsync(), "MaLoai", "TenLoai", maLoai);

            return View(items);
        }

        // GET: /TaiSan/ThanhLy (Danh sách tài sản thanh lý đang bán công khai cho mọi người / khách hàng)
        [AllowAnonymous]
        public async Task<IActionResult> ThanhLy(string? search, int? maLoai)
        {
            var query = _context.TaiSans
                .Include(t => t.MaHdNavigation)
                .Include(t => t.MaLoaiNavigation)
                .Where(t => t.MaHdNavigation.TrangThai == "thanh lý")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(t => t.TenTaiSan.Contains(search) || (t.MoTaTinhTrang != null && t.MoTaTinhTrang.Contains(search)));
            }

            if (maLoai.HasValue && maLoai.Value > 0)
            {
                query = query.Where(t => t.MaLoai == maLoai.Value);
            }

            var items = await query.OrderByDescending(t => t.MaTs).ToListAsync();
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentMaLoai = maLoai;
            ViewBag.LoaiTaiSans = new SelectList(await _context.LoaiTaiSans.ToListAsync(), "MaLoai", "TenLoai", maLoai);

            return View(items);
        }

        // POST: /TaiSan/DangKyMua
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKyMua(DangKyMuaThanhLyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taiSan = await _context.TaiSans.FindAsync(model.MaTs);
                if (taiSan != null)
                {
                    AuditLogger.LogAction(model.HoTen, "Đăng ký mua thanh lý", 
                        $"Khách hàng {model.HoTen} (ĐT: {model.SoDienThoai}) đăng ký mua tài sản TS-{taiSan.MaTs} ({taiSan.TenTaiSan}) với giá đề xuất {model.GiaDeXuat:N0} VNĐ. Lời nhắn: {model.GhiChu}");
                    
                    TempData["SuccessMessage"] = $"Đăng ký quan tâm mua tài sản '{taiSan.TenTaiSan}' thành công! Nhân viên cửa hàng sẽ liên hệ với bạn trong thời gian sớm nhất.";
                    return RedirectToAction(nameof(ThanhLy));
                }
            }

            TempData["ErrorMessage"] = "Thông tin đăng ký chưa hợp lệ, vui lòng kiểm tra lại.";
            return RedirectToAction(nameof(ThanhLy));
        }

        // GET: /TaiSan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var taiSan = await _context.TaiSans
                .Include(t => t.MaHdNavigation)
                    .ThenInclude(h => h.MaKhNavigation)
                .Include(t => t.MaLoaiNavigation)
                .FirstOrDefaultAsync(m => m.MaTs == id);

            if (taiSan == null) return NotFound();

            return View(taiSan);
        }

        // GET: /TaiSan/Edit/5
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var taiSan = await _context.TaiSans.FindAsync(id);
            if (taiSan == null) return NotFound();

            ViewBag.MaLoai = new SelectList(await _context.LoaiTaiSans.ToListAsync(), "MaLoai", "TenLoai", taiSan.MaLoai);
            return View(taiSan);
        }

        // POST: /TaiSan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public async Task<IActionResult> Edit(int id, [Bind("MaTs,MaHd,MaLoai,TenTaiSan,MoTaTinhTrang,GiaTriDinhGia,AnhTaiSan")] TaiSan taiSan, List<IFormFile>? anhTaiSanFiles)
        {
            if (id != taiSan.MaTs) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.TaiSans.AsNoTracking().FirstOrDefaultAsync(t => t.MaTs == id);
                    if (existing == null) return NotFound();

                    if (anhTaiSanFiles != null && anhTaiSanFiles.Any(f => f.Length > 0))
                    {
                        var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "taisan");
                        if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                        var uploadedList = new List<string>();
                        foreach (var file in anhTaiSanFiles)
                        {
                            if (file.Length > 0)
                            {
                                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                                var filePath = Path.Combine(uploadDir, uniqueFileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }
                                uploadedList.Add($"/uploads/taisan/{uniqueFileName}");
                            }
                        }
                        taiSan.AnhTaiSan = string.Join(";", uploadedList);
                    }
                    else
                    {
                        taiSan.AnhTaiSan = existing.AnhTaiSan;
                    }

                    _context.Update(taiSan);
                    await _context.SaveChangesAsync();

                    AuditLogger.LogAction(User.Identity?.Name ?? "Staff", "Cập nhật tài sản", $"Cập nhật thông tin tài sản TS-{taiSan.MaTs} ({taiSan.TenTaiSan})");
                    TempData["SuccessMessage"] = "Cập nhật thông tin tài sản thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiSanExists(taiSan.MaTs)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Details), new { id = taiSan.MaTs });
            }

            ViewBag.MaLoai = new SelectList(await _context.LoaiTaiSans.ToListAsync(), "MaLoai", "TenLoai", taiSan.MaLoai);
            return View(taiSan);
        }

        // GET: /TaiSan/Delete/5
        [Authorize(Roles = "Chủ cửa hàng")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var taiSan = await _context.TaiSans
                .Include(t => t.MaHdNavigation)
                .Include(t => t.MaLoaiNavigation)
                .FirstOrDefaultAsync(m => m.MaTs == id);
            if (taiSan == null) return NotFound();

            return View(taiSan);
        }

        // POST: /TaiSan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Chủ cửa hàng")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taiSan = await _context.TaiSans.FindAsync(id);
            if (taiSan != null)
            {
                _context.TaiSans.Remove(taiSan);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa tài sản thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TaiSanExists(int id)
        {
            return _context.TaiSans.Any(e => e.MaTs == id);
        }
    }
}
