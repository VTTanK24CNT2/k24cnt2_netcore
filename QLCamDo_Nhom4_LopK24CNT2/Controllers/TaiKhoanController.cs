using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCamDo_Nhom4_LopK24CNT2.Helpers;
using QLCamDo_Nhom4_LopK24CNT2.Models;
using QLCamDo_Nhom4_LopK24CNT2.Models.ViewModels;

namespace QLCamDo_Nhom4_LopK24CNT2.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly QlcamDoNhom4LopK24cnt2Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TaiKhoanController(QlcamDoNhom4LopK24cnt2Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /TaiKhoan/DangNhap
        [HttpGet]
        public IActionResult DangNhap(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /TaiKhoan/DangNhap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangNhap(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.MaKhNavigation)
                .FirstOrDefaultAsync(t => t.TenDangNhap == model.TenDangNhap);

            if (taiKhoan == null || !PasswordHelper.VerifyPassword(model.MatKhau, taiKhoan.MatKhauHash))
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
                return View(model);
            }

            // Tạo claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, taiKhoan.MaTk.ToString()),
                new Claim(ClaimTypes.Name, taiKhoan.TenDangNhap),
                new Claim(ClaimTypes.Role, taiKhoan.VaiTro)
            };

            if (taiKhoan.MaKh.HasValue)
            {
                claims.Add(new Claim("MaKH", taiKhoan.MaKh.Value.ToString()));
                if (taiKhoan.MaKhNavigation != null)
                {
                    claims.Add(new Claim("HoTen", taiKhoan.MaKhNavigation.HoTen));
                }
            }
            else
            {
                claims.Add(new Claim("HoTen", taiKhoan.TenDangNhap));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.GhiNho,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(model.GhiNho ? 72 : 4)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            // Lưu thông tin vào Session
            HttpContext.Session.SetInt32("UserId", taiKhoan.MaTk);
            HttpContext.Session.SetString("Username", taiKhoan.TenDangNhap);
            HttpContext.Session.SetString("Role", taiKhoan.VaiTro);
            if (taiKhoan.MaKh.HasValue)
            {
                HttpContext.Session.SetInt32("MaKH", taiKhoan.MaKh.Value);
            }

            AuditLogger.LogAction(taiKhoan.TenDangNhap, "Đăng nhập", $"Người dùng [{taiKhoan.TenDangNhap}] đăng nhập vai trò [{taiKhoan.VaiTro}]");

            TempData["SuccessMessage"] = $"Đăng nhập thành công! Chào mừng {taiKhoan.TenDangNhap}";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: /TaiKhoan/DangKy
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        // POST: /TaiKhoan/DangKy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKy(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra username đã tồn tại chưa
            if (await _context.TaiKhoans.AnyAsync(t => t.TenDangNhap == model.TenDangNhap))
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã được sử dụng. Vui lòng chọn tên khác.");
                return View(model);
            }

            // Kiểm tra CCCD
            var existingKhachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.SoCccd == model.SoCccd);
            KhachHang khachHang;

            if (existingKhachHang != null)
            {
                // Kiểm tra xem khách hàng này đã có tài khoản chưa
                if (await _context.TaiKhoans.AnyAsync(t => t.MaKh == existingKhachHang.MaKh))
                {
                    ModelState.AddModelError("SoCccd", "Số CCCD này đã được liên kết với một tài khoản khác.");
                    return View(model);
                }
                khachHang = existingKhachHang;
            }
            else
            {
                // Tạo mới Khách hàng
                string? cccdFileName = null;
                if (model.AnhCccdFile != null && model.AnhCccdFile.Length > 0)
                {
                    var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "cccd");
                    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.AnhCccdFile.FileName)}";
                    var filePath = Path.Combine(uploadDir, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.AnhCccdFile.CopyToAsync(stream);
                    }
                    cccdFileName = $"/uploads/cccd/{uniqueFileName}";
                }

                khachHang = new KhachHang
                {
                    HoTen = model.HoTen,
                    SoCccd = model.SoCccd,
                    SoDienThoai = model.SoDienThoai,
                    DiaChi = model.DiaChi,
                    AnhCccd = cccdFileName
                };
                _context.KhachHangs.Add(khachHang);
                await _context.SaveChangesAsync();
            }

            // Tạo tài khoản
            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = model.TenDangNhap,
                MatKhauHash = PasswordHelper.HashPassword(model.MatKhau),
                VaiTro = "Khách hàng",
                MaKh = khachHang.MaKh
            };

            _context.TaiKhoans.Add(taiKhoan);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đăng ký tài khoản thành công! Vui lòng đăng nhập.";
            return RedirectToAction(nameof(DangNhap));
        }

        // GET: /TaiKhoan/DangXuat
        [HttpGet]
        public async Task<IActionResult> DangXuat()
        {
            var username = User.Identity?.Name ?? "Unknown";
            AuditLogger.LogAction(username, "Đăng xuất", "Người dùng đăng xuất hệ thống");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            TempData["InfoMessage"] = "Bạn đã đăng xuất khỏi hệ thống.";
            return RedirectToAction(nameof(DangNhap));
        }

        // GET: /TaiKhoan/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: /TaiKhoan (Quản lý danh sách tài khoản - Chỉ Chủ cửa hàng)
        [Authorize(Roles = "Chủ cửa hàng")]
        public async Task<IActionResult> Index()
        {
            var taiKhoans = await _context.TaiKhoans
                .Include(t => t.MaKhNavigation)
                .OrderBy(t => t.VaiTro)
                .ThenBy(t => t.TenDangNhap)
                .ToListAsync();
            return View(taiKhoans);
        }

        // GET: /TaiKhoan/Create (Chỉ Chủ cửa hàng)
        [Authorize(Roles = "Chủ cửa hàng")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /TaiKhoan/Create
        [Authorize(Roles = "Chủ cửa hàng")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string tenDangNhap, string matKhau, string vaiTro)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhau))
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập và mật khẩu không được để trống.");
                return View();
            }

            if (await _context.TaiKhoans.AnyAsync(t => t.TenDangNhap == tenDangNhap))
            {
                ModelState.AddModelError("tenDangNhap", "Tên đăng nhập đã tồn tại.");
                return View();
            }

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = tenDangNhap,
                MatKhauHash = PasswordHelper.HashPassword(matKhau),
                VaiTro = vaiTro
            };

            _context.TaiKhoans.Add(taiKhoan);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Tạo tài khoản {tenDangNhap} ({vaiTro}) thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /TaiKhoan/DoiMatKhau
        [Authorize]
        public IActionResult DoiMatKhau()
        {
            return View();
        }

        // POST: /TaiKhoan/DoiMatKhau
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiMatKhau(string matKhauCu, string matKhauMoi, string xacNhanMatKhau)
        {
            if (string.IsNullOrWhiteSpace(matKhauMoi) || matKhauMoi.Length < 6)
            {
                ModelState.AddModelError("matKhauMoi", "Mật khẩu mới phải có tối thiểu 6 ký tự.");
                return View();
            }

            if (matKhauMoi != xacNhanMatKhau)
            {
                ModelState.AddModelError("xacNhanMatKhau", "Xác nhận mật khẩu mới không khớp.");
                return View();
            }

            var username = User.Identity?.Name;
            var taiKhoan = await _context.TaiKhoans.FirstOrDefaultAsync(t => t.TenDangNhap == username);
            if (taiKhoan == null || !PasswordHelper.VerifyPassword(matKhauCu, taiKhoan.MatKhauHash))
            {
                ModelState.AddModelError("matKhauCu", "Mật khẩu hiện tại không chính xác.");
                return View();
            }

            taiKhoan.MatKhauHash = PasswordHelper.HashPassword(matKhauMoi);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("Index", "Home");
        }
    }
}
