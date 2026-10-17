using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCamDo_Nhom4_LopK24CNT2.Helpers;
using QLCamDo_Nhom4_LopK24CNT2.Models;

namespace QLCamDo_Nhom4_LopK24CNT2.Controllers
{
    [Authorize]
    public class KhachHangController : Controller
    {
        private readonly QlcamDoNhom4LopK24cnt2Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public KhachHangController(QlcamDoNhom4LopK24cnt2Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /KhachHang
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            // Nếu là khách hàng thông thường, chỉ xem được thông tin của chính mình
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
                    return RedirectToAction(nameof(Details), new { id = userMaKh.Value });
                }
            }

            var query = _context.KhachHangs
                .Include(k => k.HopDongCams)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(k => k.HoTen.Contains(search) 
                                      || k.SoCccd.Contains(search) 
                                      || k.SoDienThoai.Contains(search));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages == 0) totalPages = 1;
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var items = await query
                .OrderByDescending(k => k.MaKh)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = pageSize;

            return View(items);
        }

        // GET: /KhachHang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Khách hàng chỉ xem được hồ sơ của mình
            if (User.IsInRole("Khách hàng"))
            {
                var userMaKh = HttpContext.Session.GetInt32("MaKH");
                if (userMaKh.HasValue && userMaKh.Value != id)
                {
                    return Forbid();
                }
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.HopDongCams)
                    .ThenInclude(h => h.TaiSans)
                .Include(k => k.HopDongCams)
                    .ThenInclude(h => h.GiaoDiches)
                .Include(k => k.TaiKhoans)
                .FirstOrDefaultAsync(m => m.MaKh == id);

            if (khachHang == null) return NotFound();

            return View(khachHang);
        }

        // GET: /KhachHang/Create
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /KhachHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public async Task<IActionResult> Create(KhachHang khachHang, IFormFile? anhCccdFile)
        {
            // Kiểm tra trùng CCCD
            if (await _context.KhachHangs.AnyAsync(k => k.SoCccd == khachHang.SoCccd))
            {
                ModelState.AddModelError("SoCccd", "Số CCCD này đã tồn tại trong hệ thống.");
            }

            if (ModelState.IsValid)
            {
                if (anhCccdFile != null && anhCccdFile.Length > 0)
                {
                    var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "cccd");
                    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(anhCccdFile.FileName)}";
                    var filePath = Path.Combine(uploadDir, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await anhCccdFile.CopyToAsync(stream);
                    }
                    khachHang.AnhCccd = $"/uploads/cccd/{uniqueFileName}";
                }

                _context.Add(khachHang);
                await _context.SaveChangesAsync();

                AuditLogger.LogAction(User.Identity?.Name ?? "Staff", "Thêm khách hàng", $"Thêm khách hàng {khachHang.HoTen}, CCCD: {khachHang.SoCccd}");
                TempData["SuccessMessage"] = $"Thêm khách hàng '{khachHang.HoTen}' thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(khachHang);
        }

        // GET: /KhachHang/Edit/5
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null) return NotFound();

            return View(khachHang);
        }

        // POST: /KhachHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public async Task<IActionResult> Edit(int id, KhachHang khachHang, IFormFile? anhCccdFile)
        {
            if (id != khachHang.MaKh) return NotFound();

            // Kiểm tra trùng CCCD với khách hàng khác
            if (await _context.KhachHangs.AnyAsync(k => k.SoCccd == khachHang.SoCccd && k.MaKh != id))
            {
                ModelState.AddModelError("SoCccd", "Số CCCD này đã tồn tại ở khách hàng khác.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.KhachHangs.AsNoTracking().FirstOrDefaultAsync(k => k.MaKh == id);
                    if (existing == null) return NotFound();

                    if (anhCccdFile != null && anhCccdFile.Length > 0)
                    {
                        var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "cccd");
                        if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(anhCccdFile.FileName)}";
                        var filePath = Path.Combine(uploadDir, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await anhCccdFile.CopyToAsync(stream);
                        }
                        khachHang.AnhCccd = $"/uploads/cccd/{uniqueFileName}";
                    }
                    else
                    {
                        khachHang.AnhCccd = existing.AnhCccd;
                    }

                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();

                    AuditLogger.LogAction(User.Identity?.Name ?? "Staff", "Cập nhật khách hàng", $"Cập nhật khách hàng {khachHang.HoTen}");
                    TempData["SuccessMessage"] = "Cập nhật khách hàng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.MaKh)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: /KhachHang/Delete/5
        [Authorize(Roles = "Chủ cửa hàng")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var khachHang = await _context.KhachHangs
                .Include(k => k.HopDongCams)
                .FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null) return NotFound();

            return View(khachHang);
        }

        // POST: /KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Chủ cửa hàng")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khachHang = await _context.KhachHangs
                .Include(k => k.HopDongCams)
                .Include(k => k.TaiKhoans)
                .FirstOrDefaultAsync(m => m.MaKh == id);

            if (khachHang != null)
            {
                if (khachHang.HopDongCams.Any())
                {
                    TempData["ErrorMessage"] = $"Không thể xóa khách hàng '{khachHang.HoTen}' vì đang có {khachHang.HopDongCams.Count} hợp đồng cầm đồ liên kết!";
                    return RedirectToAction(nameof(Index));
                }

                // Xóa tài khoản liên kết nếu có
                if (khachHang.TaiKhoans.Any())
                {
                    _context.TaiKhoans.RemoveRange(khachHang.TaiKhoans);
                }

                _context.KhachHangs.Remove(khachHang);
                await _context.SaveChangesAsync();

                AuditLogger.LogAction(User.Identity?.Name ?? "Admin", "Xóa khách hàng", $"Xóa khách hàng {khachHang.HoTen}");
                TempData["SuccessMessage"] = "Xóa khách hàng thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
            return _context.KhachHangs.Any(e => e.MaKh == id);
        }
    }
}
