using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCamDo_Nhom4_LopK24CNT2.Helpers;
using QLCamDo_Nhom4_LopK24CNT2.Models;
using QLCamDo_Nhom4_LopK24CNT2.Models.ViewModels;

namespace QLCamDo_Nhom4_LopK24CNT2.Controllers
{
    public class HomeController : Controller
    {
        private readonly QlcamDoNhom4LopK24cnt2Context _context;

        public HomeController(QlcamDoNhom4LopK24cnt2Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var threeDaysLater = today.AddDays(3);

            // Cập nhật ngầm các hợp đồng quá hạn nếu cần
            var expiredContracts = await _context.HopDongCams
                .Where(h => h.TrangThai == "đang cầm" && h.HanChuoc < today)
                .ToListAsync();

            if (expiredContracts.Any())
            {
                foreach (var item in expiredContracts)
                {
                    item.TrangThai = "quá hạn";
                }
                await _context.SaveChangesAsync();
            }

            // Nếu người dùng chưa đăng nhập, chuyển tới trang đăng nhập hoặc hiển thị landing page
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            // Nếu là Khách hàng: Chuyển hướng tới giao diện dành riêng cho khách hàng
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
                    var khachHang = await _context.KhachHangs
                        .Include(k => k.HopDongCams)
                            .ThenInclude(h => h.TaiSans)
                        .Include(k => k.HopDongCams)
                            .ThenInclude(h => h.GiaoDiches)
                        .FirstOrDefaultAsync(k => k.MaKh == userMaKh.Value);

                    return View("CustomerDashboard", khachHang);
                }
            }

            // Đối với Chủ cửa hàng & Nhân viên: Hiển thị Thống kê Dashboard
            var model = new ThongKeViewModel
            {
                TongTienChoVay = await _context.HopDongCams
                    .Where(h => h.TrangThai == "đang cầm" || h.TrangThai == "quá hạn")
                    .SumAsync(h => h.TienCam),

                TongLaiDaThu = await _context.GiaoDiches
                    .Where(g => g.LoaiGd == "đóng lãi" || g.LoaiGd == "gia hạn")
                    .SumAsync(g => g.SoTien),

                SoHopDongDangCam = await _context.HopDongCams.CountAsync(h => h.TrangThai == "đang cầm"),
                SoHopDongSapDenHan = await _context.HopDongCams.CountAsync(h => h.TrangThai == "đang cầm" && h.HanChuoc >= today && h.HanChuoc <= threeDaysLater),
                SoHopDongQuaHan = await _context.HopDongCams.CountAsync(h => h.TrangThai == "quá hạn"),
                SoHopDongThanhLy = await _context.HopDongCams.CountAsync(h => h.TrangThai == "thanh lý"),
                TongSoKhachHang = await _context.KhachHangs.CountAsync(),

                HopDongSapDenHanList = await _context.HopDongCams
                    .Include(h => h.MaKhNavigation)
                    .Include(h => h.TaiSans)
                    .Where(h => h.TrangThai == "đang cầm" && h.HanChuoc >= today && h.HanChuoc <= threeDaysLater)
                    .OrderBy(h => h.HanChuoc)
                    .Take(5)
                    .ToListAsync(),

                HopDongQuaHanList = await _context.HopDongCams
                    .Include(h => h.MaKhNavigation)
                    .Include(h => h.TaiSans)
                    .Where(h => h.TrangThai == "quá hạn")
                    .OrderBy(h => h.HanChuoc)
                    .Take(5)
                    .ToListAsync()
            };

            // Dữ liệu biểu đồ dòng tiền 6 tháng gần nhất
            var months = new List<string>();
            var choVayList = new List<decimal>();
            var thuLaiList = new List<decimal>();

            for (int i = 5; i >= 0; i--)
            {
                var dt = DateTime.Now.AddMonths(-i);
                var monthStr = $"T{dt.Month}/{dt.Year}";
                months.Add(monthStr);

                var startOfMonth = new DateOnly(dt.Year, dt.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                var startOfMonthDt = new DateTime(dt.Year, dt.Month, 1);
                var endOfMonthDt = startOfMonthDt.AddMonths(1).AddTicks(-1);

                var choVay = await _context.HopDongCams
                    .Where(h => h.NgayCam >= startOfMonth && h.NgayCam <= endOfMonth)
                    .SumAsync(h => (decimal?)h.TienCam) ?? 0;

                var thuLai = await _context.GiaoDiches
                    .Where(g => g.NgayGd >= startOfMonthDt && g.NgayGd <= endOfMonthDt)
                    .SumAsync(g => (decimal?)g.SoTien) ?? 0;

                choVayList.Add(choVay);
                thuLaiList.Add(thuLai);
            }

            model.ChartLabels = months;
            model.ChartTienChoVay = choVayList;
            model.ChartLaiThuVe = thuLaiList;

            ViewBag.FeaturedAssets = await _context.TaiSans
                .Include(t => t.MaLoaiNavigation)
                .Include(t => t.MaHdNavigation)
                .OrderByDescending(t => t.MaTs)
                .Take(6)
                .ToListAsync();

            ViewBag.RecentAuditLogs = AuditLogger.GetRecentLogs(10);

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
