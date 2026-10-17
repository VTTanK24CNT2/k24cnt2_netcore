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
    public class GiaoDichController : Controller
    {
        private readonly QlcamDoNhom4LopK24cnt2Context _context;

        public GiaoDichController(QlcamDoNhom4LopK24cnt2Context context)
        {
            _context = context;
        }

        // GET: /GiaoDich
        public async Task<IActionResult> Index(int? maHd, string? loaiGd, string? nhanVienThu, int page = 1, int pageSize = 10)
        {
            var query = _context.GiaoDiches
                .Include(g => g.MaHdNavigation)
                    .ThenInclude(h => h.MaKhNavigation)
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
                    query = query.Where(g => g.MaHdNavigation.MaKh == userMaKh.Value);
                }
                else
                {
                    query = query.Where(g => false);
                }
            }

            if (maHd.HasValue)
            {
                query = query.Where(g => g.MaHd == maHd.Value);
            }

            // Lọc theo loại giao dịch (đóng lãi, gia hạn, chuộc)
            if (!string.IsNullOrWhiteSpace(loaiGd))
            {
                query = query.Where(g => g.LoaiGd == loaiGd);
            }

            // Lọc theo nhân viên thu tiền
            if (!string.IsNullOrWhiteSpace(nhanVienThu))
            {
                nhanVienThu = nhanVienThu.Trim();
                query = query.Where(g => g.NhanVienThu.Contains(nhanVienThu));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages == 0) totalPages = 1;
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var items = await query
                .OrderByDescending(g => g.NgayGd)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentMaHd = maHd;
            ViewBag.CurrentLoaiGd = loaiGd;
            ViewBag.CurrentNhanVienThu = nhanVienThu;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

            return View(items);
        }

        // GET: /GiaoDich/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var giaoDich = await _context.GiaoDiches
                .Include(g => g.MaHdNavigation)
                    .ThenInclude(h => h.MaKhNavigation)
                .Include(g => g.MaHdNavigation)
                    .ThenInclude(h => h.TaiSans)
                .FirstOrDefaultAsync(m => m.MaGd == id);

            if (giaoDich == null) return NotFound();

            // Khách hàng chỉ xem giao dịch của mình
            if (User.IsInRole("Khách hàng"))
            {
                var userMaKh = HttpContext.Session.GetInt32("MaKH");
                if (userMaKh.HasValue && giaoDich.MaHdNavigation.MaKh != userMaKh.Value)
                {
                    return Forbid();
                }
            }

            return View(giaoDich);
        }

        // GET: /GiaoDich/Create (Tạo giao dịch: thu lãi, gia hạn, chuộc)
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public async Task<IActionResult> Create(int? maHd, string? loaiGd)
        {
            if (!maHd.HasValue)
            {
                ViewBag.HopDongList = new SelectList(await _context.HopDongCams
                    .Where(h => h.TrangThai == "đang cầm" || h.TrangThai == "quá hạn")
                    .Include(h => h.MaKhNavigation)
                    .Select(h => new { h.MaHd, TenHienThi = $"HD-{h.MaHd} - {h.MaKhNavigation.HoTen} ({h.TienCam:N0} VNĐ)" })
                    .ToListAsync(), "MaHd", "TenHienThi");
                return View(new GiaoDichCreateViewModel());
            }

            var hopDong = await _context.HopDongCams
                .Include(h => h.MaKhNavigation)
                .Include(h => h.GiaoDiches)
                .FirstOrDefaultAsync(h => h.MaHd == maHd.Value);

            if (hopDong == null) return NotFound();

            if (hopDong.TrangThai == "đã chuộc")
            {
                TempData["ErrorMessage"] = "Hợp đồng này đã được chuộc hoàn tất, không thể lập giao dịch mới!";
                return RedirectToAction("Details", "HopDongCam", new { id = maHd.Value });
            }

            var tienLaiTamTinh = InterestCalculator.GetCurrentInterest(hopDong);
            var tongLaiDaDong = hopDong.GiaoDiches.Where(g => g.LoaiGd == "đóng lãi" || g.LoaiGd == "gia hạn").Sum(g => g.SoTien);
            var tienLaiConLai = Math.Max(0, tienLaiTamTinh - tongLaiDaDong);

            var defaultAmount = 0m;
            var defaultLoai = loaiGd ?? "đóng lãi";
            if (defaultLoai == "chuộc")
            {
                defaultAmount = hopDong.TienCam + tienLaiConLai;
            }
            else
            {
                defaultAmount = tienLaiConLai > 0 ? tienLaiConLai : InterestCalculator.GetFullTermInterest(hopDong);
            }

            var model = new GiaoDichCreateViewModel
            {
                MaHd = hopDong.MaHd,
                LoaiGd = defaultLoai,
                SoTien = defaultAmount,
                GiaHanDenNgay = hopDong.HanChuoc.AddDays(30)
            };

            ViewBag.HopDong = hopDong;
            ViewBag.TienLaiTamTinh = tienLaiTamTinh;
            ViewBag.TongLaiDaDong = tongLaiDaDong;
            ViewBag.TienLaiConLai = tienLaiConLai;

            return View(model);
        }

        // POST: /GiaoDich/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public async Task<IActionResult> Create(GiaoDichCreateViewModel model)
        {
            var hopDong = await _context.HopDongCams
                .Include(h => h.MaKhNavigation)
                .Include(h => h.TaiSans)
                    .ThenInclude(t => t.MaLoaiNavigation)
                .FirstOrDefaultAsync(h => h.MaHd == model.MaHd);

            if (hopDong == null)
            {
                ModelState.AddModelError("MaHd", "Hợp đồng không tồn tại.");
            }

            if (model.SoTien <= 0)
            {
                ModelState.AddModelError("SoTien", "Số tiền giao dịch phải lớn hơn 0 VNĐ.");
            }

            if (model.LoaiGd == "gia hạn" && model.GiaHanDenNgay.HasValue && hopDong != null)
            {
                if (model.GiaHanDenNgay.Value <= hopDong.HanChuoc)
                {
                    ModelState.AddModelError("GiaHanDenNgay", "Ngày gia hạn mới phải lớn hơn hạn chuộc hiện tại.");
                }
            }

            if (ModelState.IsValid && hopDong != null)
            {
                var staffName = User.Identity?.Name ?? "Nhân viên";

                var giaoDich = new GiaoDich
                {
                    MaHd = model.MaHd,
                    LoaiGd = model.LoaiGd,
                    NgayGd = DateTime.Now,
                    SoTien = model.SoTien,
                    NhanVienThu = staffName
                };

                _context.GiaoDiches.Add(giaoDich);

                // Cập nhật trạng thái hợp đồng theo loại giao dịch
                if (model.LoaiGd == "chuộc")
                {
                    hopDong.TrangThai = "đã chuộc";
                }
                else if (model.LoaiGd == "gia hạn" && model.GiaHanDenNgay.HasValue)
                {
                    hopDong.HanChuoc = model.GiaHanDenNgay.Value;
                    if (hopDong.TrangThai == "quá hạn")
                    {
                        hopDong.TrangThai = "đang cầm";
                    }
                }

                await _context.SaveChangesAsync();

                // Ghi audit log
                AuditLogger.LogMoneyChange(staffName, $"Giao dịch {model.LoaiGd}",
                    $"Thu {model.LoaiGd} cho hợp đồng HD-{hopDong.MaHd}. Mã GD: GD-{giaoDich.MaGd}", null, model.SoTien);

                TempData["SuccessMessage"] = $"Ghi nhận giao dịch {model.LoaiGd} ({model.SoTien:N0} VNĐ) thành công!";
                return RedirectToAction(nameof(InBienNhan), new { id = giaoDich.MaGd });
            }

            if (hopDong != null)
            {
                ViewBag.HopDong = hopDong;
                ViewBag.TienLaiTamTinh = InterestCalculator.GetCurrentInterest(hopDong);
            }

            return View(model);
        }

        // GET: /GiaoDich/InBienNhan/5 (In biên lai / biên nhận thu tiền)
        public async Task<IActionResult> InBienNhan(int? id)
        {
            if (id == null) return NotFound();

            var giaoDich = await _context.GiaoDiches
                .Include(g => g.MaHdNavigation)
                    .ThenInclude(h => h.MaKhNavigation)
                .Include(g => g.MaHdNavigation)
                    .ThenInclude(h => h.TaiSans)
                .FirstOrDefaultAsync(m => m.MaGd == id);

            if (giaoDich == null) return NotFound();

            // Khách hàng chỉ in biên nhận của mình
            if (User.IsInRole("Khách hàng"))
            {
                var userMaKh = HttpContext.Session.GetInt32("MaKH");
                if (userMaKh.HasValue && giaoDich.MaHdNavigation.MaKh != userMaKh.Value)
                {
                    return Forbid();
                }
            }

            return View(giaoDich);
        }

        // POST: /GiaoDich/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Chủ cửa hàng")]
        public async Task<IActionResult> Delete(int id)
        {
            var giaoDich = await _context.GiaoDiches.FindAsync(id);
            if (giaoDich != null)
            {
                AuditLogger.LogMoneyChange(User.Identity?.Name ?? "Admin", "Hủy giao dịch",
                    $"Hủy giao dịch GD-{giaoDich.MaGd} của hợp đồng HD-{giaoDich.MaHd}", giaoDich.SoTien, 0);

                _context.GiaoDiches.Remove(giaoDich);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã xóa giao dịch GD-{id}!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
