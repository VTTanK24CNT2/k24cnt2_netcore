using System.Text.Json;
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
    public class HopDongCamController : Controller
    {
        private readonly QlcamDoNhom4LopK24cnt2Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HopDongCamController(QlcamDoNhom4LopK24cnt2Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /HopDongCam
        public async Task<IActionResult> Index(
            string? search,
            string? trangThai,
            int? maLoai,
            DateOnly? tuNgay,
            DateOnly? denNgay,
            string? sapDenHan,
            int page = 1,
            int pageSize = 10,
            bool resetFilter = false)
        {
            const string CookieFilterKey = "QLCamDo_ContractFilter";

            // Nếu người dùng yêu cầu reset bộ lọc
            if (resetFilter)
            {
                Response.Cookies.Delete(CookieFilterKey);
                return RedirectToAction(nameof(Index));
            }

            // Nếu không có tham số query, thử đọc lại bộ lọc đã lưu trong Cookie
            if (string.IsNullOrEmpty(search) && string.IsNullOrEmpty(trangThai) && !maLoai.HasValue 
                && !tuNgay.HasValue && !denNgay.HasValue && string.IsNullOrEmpty(sapDenHan) && page == 1)
            {
                if (Request.Cookies.TryGetValue(CookieFilterKey, out var savedFilterJson) && !string.IsNullOrEmpty(savedFilterJson))
                {
                    try
                    {
                        var filterData = JsonSerializer.Deserialize<Dictionary<string, string>>(savedFilterJson);
                        if (filterData != null)
                        {
                            if (filterData.TryGetValue("search", out var s) && !string.IsNullOrEmpty(s)) search = s;
                            if (filterData.TryGetValue("trangThai", out var tt) && !string.IsNullOrEmpty(tt)) trangThai = tt;
                            if (filterData.TryGetValue("maLoai", out var ml) && int.TryParse(ml, out var mlVal)) maLoai = mlVal;
                            if (filterData.TryGetValue("tuNgay", out var tn) && DateOnly.TryParse(tn, out var tnVal)) tuNgay = tnVal;
                            if (filterData.TryGetValue("denNgay", out var dn) && DateOnly.TryParse(dn, out var dnVal)) denNgay = dnVal;
                            if (filterData.TryGetValue("sapDenHan", out var sdh) && !string.IsNullOrEmpty(sdh)) sapDenHan = sdh;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                // Lưu bộ lọc hiện tại vào Cookie
                var filterData = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(search)) filterData["search"] = search;
                if (!string.IsNullOrEmpty(trangThai)) filterData["trangThai"] = trangThai;
                if (maLoai.HasValue) filterData["maLoai"] = maLoai.Value.ToString();
                if (tuNgay.HasValue) filterData["tuNgay"] = tuNgay.Value.ToString("yyyy-MM-dd");
                if (denNgay.HasValue) filterData["denNgay"] = denNgay.Value.ToString("yyyy-MM-dd");
                if (!string.IsNullOrEmpty(sapDenHan)) filterData["sapDenHan"] = sapDenHan;

                Response.Cookies.Append(CookieFilterKey, JsonSerializer.Serialize(filterData), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax
                });
            }

            var query = _context.HopDongCams
                .Include(h => h.MaKhNavigation)
                .Include(h => h.TaiSans)
                    .ThenInclude(t => t.MaLoaiNavigation)
                .Include(h => h.GiaoDiches)
                .AsQueryable();

            // Nếu người dùng là Khách hàng, chỉ được xem hợp đồng của chính mình
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
                    query = query.Where(h => h.MaKh == userMaKh.Value);
                }
                else
                {
                    query = query.Where(h => false);
                }
            }

            // Tìm kiếm theo tên khách, CCCD, hoặc mã hợp đồng
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                bool isNumeric = int.TryParse(search, out int searchId);

                query = query.Where(h =>
                    (isNumeric && h.MaHd == searchId) ||
                    h.MaKhNavigation.HoTen.Contains(search) ||
                    h.MaKhNavigation.SoCccd.Contains(search) ||
                    h.TaiSans.Any(t => t.TenTaiSan.Contains(search)));
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(h => h.TrangThai == trangThai);
            }

            // Lọc theo loại tài sản
            if (maLoai.HasValue && maLoai.Value > 0)
            {
                query = query.Where(h => h.TaiSans.Any(t => t.MaLoai == maLoai.Value));
            }

            // Lọc theo khoảng ngày cầm
            if (tuNgay.HasValue)
            {
                query = query.Where(h => h.NgayCam >= tuNgay.Value);
            }
            if (denNgay.HasValue)
            {
                query = query.Where(h => h.NgayCam <= denNgay.Value);
            }

            // Lọc sắp đến hạn (trong vòng 3 ngày tới và đang cầm)
            var today = DateOnly.FromDateTime(DateTime.Now);
            var threeDaysLater = today.AddDays(3);

            if (sapDenHan == "true" || sapDenHan == "1")
            {
                query = query.Where(h => h.TrangThai == "đang cầm" && h.HanChuoc >= today && h.HanChuoc <= threeDaysLater);
            }
            else if (trangThai == "quá hạn")
            {
                query = query.Where(h => h.TrangThai == "quá hạn" || (h.TrangThai == "đang cầm" && h.HanChuoc < today));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages == 0) totalPages = 1;
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var items = await query
                .OrderByDescending(h => h.MaHd)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentTrangThai = trangThai;
            ViewBag.CurrentMaLoai = maLoai;
            ViewBag.CurrentTuNgay = tuNgay;
            ViewBag.CurrentDenNgay = denNgay;
            ViewBag.CurrentSapDenHan = sapDenHan;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = pageSize;

            ViewBag.LoaiTaiSans = new SelectList(await _context.LoaiTaiSans.ToListAsync(), "MaLoai", "TenLoai", maLoai);

            return View(items);
        }

        // GET: /HopDongCam/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var hopDong = await _context.HopDongCams
                .Include(h => h.MaKhNavigation)
                .Include(h => h.TaiSans)
                    .ThenInclude(t => t.MaLoaiNavigation)
                .Include(h => h.GiaoDiches.OrderByDescending(g => g.NgayGd))
                .FirstOrDefaultAsync(m => m.MaHd == id);

            if (hopDong == null) return NotFound();

            // Khách hàng chỉ xem hợp đồng của mình
            if (User.IsInRole("Khách hàng"))
            {
                var userMaKh = HttpContext.Session.GetInt32("MaKH");
                if (userMaKh.HasValue && hopDong.MaKh != userMaKh.Value)
                {
                    return Forbid();
                }
            }

            // Tính tiền lãi tự động theo số ngày
            var today = DateOnly.FromDateTime(DateTime.Now);
            var soNgayCam = InterestCalculator.CalculateDays(hopDong.NgayCam, hopDong.HanChuoc);
            var soNgayThucTe = InterestCalculator.CalculateDays(hopDong.NgayCam, today);
            var tienLaiDuKien = InterestCalculator.GetFullTermInterest(hopDong);
            var tienLaiTamTinh = InterestCalculator.GetCurrentInterest(hopDong);

            var tongTienDaDong = hopDong.GiaoDiches.Where(g => g.LoaiGd == "đóng lãi").Sum(g => g.SoTien);
            var tongTienGiaHan = hopDong.GiaoDiches.Where(g => g.LoaiGd == "gia hạn").Sum(g => g.SoTien);
            var tongTienChuoc = hopDong.GiaoDiches.Where(g => g.LoaiGd == "chuộc").Sum(g => g.SoTien);

            var tienLaiConLai = Math.Max(0, tienLaiTamTinh - (tongTienDaDong + tongTienGiaHan));
            var tongTienDeChuoc = hopDong.TienCam + tienLaiConLai;

            ViewBag.SoNgayCam = soNgayCam;
            ViewBag.SoNgayThucTe = soNgayThucTe;
            ViewBag.TienLaiDuKien = tienLaiDuKien;
            ViewBag.TienLaiTamTinh = tienLaiTamTinh;
            ViewBag.TongTienDaDong = tongTienDaDong;
            ViewBag.TienLaiConLai = tienLaiConLai;
            ViewBag.TongTienDeChuoc = tongTienDeChuoc;

            return View(hopDong);
        }

        // GET: /HopDongCam/Create
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public async Task<IActionResult> Create()
        {
            ViewBag.KhachHangs = await _context.KhachHangs
                .OrderBy(k => k.HoTen)
                .Select(k => new SelectListItem
                {
                    Value = k.MaKh.ToString(),
                    Text = $"{k.HoTen} - CCCD: {k.SoCccd} - ĐT: {k.SoDienThoai}"
                }).ToListAsync();

            var loaiList = await _context.LoaiTaiSans.ToListAsync();
            ViewBag.LoaiTaiSans = new SelectList(loaiList, "MaLoai", "TenLoai");
            ViewBag.LoaiTaiSanData = loaiList;

            var model = new HopDongCreateViewModel
            {
                NgayCam = DateOnly.FromDateTime(DateTime.Now),
                HanChuoc = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                LaiSuat = loaiList.FirstOrDefault()?.LaiSuatNgay ?? 0.25m
            };

            // Kiểm tra xem trong session có hợp đồng đang soạn dở không (Sử dụng SessionExtensions)
            var draft = HttpContext.Session.GetObject<HopDongCreateViewModel>("DraftHopDong");
            if (draft != null)
            {
                ViewBag.HasDraft = true;
            }

            return View(model);
        }

        // POST: /HopDongCam/SaveDraft (Lưu nháp đối tượng vào Session)
        [HttpPost]
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public IActionResult SaveDraft([FromBody] HopDongCreateViewModel model)
        {
            // Session lưu object (Helpers/SessionExtensions.cs)
            HttpContext.Session.SetObject("DraftHopDong", model);
            return Json(new { success = true, message = "Đã lưu nháp hợp đồng vào Session thành công!" });
        }

        // GET: /HopDongCam/LoadDraft
        [HttpGet]
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public IActionResult LoadDraft()
        {
            var draft = HttpContext.Session.GetObject<HopDongCreateViewModel>("DraftHopDong");
            if (draft == null)
            {
                return Json(new { success = false, message = "Không có bản nháp nào được lưu." });
            }
            return Json(draft);
        }

        // POST: /HopDongCam/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public async Task<IActionResult> Create(HopDongCreateViewModel model)
        {
            var loaiTaiSan = await _context.LoaiTaiSans.FindAsync(model.MaLoai);
            if (loaiTaiSan == null)
            {
                ModelState.AddModelError("MaLoai", "Loại tài sản không hợp lệ.");
            }

            // Ràng buộc 1: Hạn chuộc phải lớn hơn ngày cầm
            if (model.HanChuoc <= model.NgayCam)
            {
                ModelState.AddModelError("HanChuoc", "Hạn chuộc phải sau ngày cầm đồ ít nhất 1 ngày.");
            }

            // Ràng buộc 2: Thời hạn không vượt quá số ngày tối đa của loại tài sản
            if (loaiTaiSan != null)
            {
                var soNgay = (model.HanChuoc.DayNumber - model.NgayCam.DayNumber);
                if (soNgay > loaiTaiSan.SoNgayToiDa)
                {
                    ModelState.AddModelError("HanChuoc", $"Thời hạn cầm ({soNgay} ngày) vượt quá số ngày tối đa cho phép của {loaiTaiSan.TenLoai} ({loaiTaiSan.SoNgayToiDa} ngày).");
                }
            }

            // Ràng buộc 3: Tiền cầm <= 80% giá trị định giá
            var maxTienCam = model.GiaTriDinhGia * 0.8m;
            if (model.TienCam > maxTienCam)
            {
                ModelState.AddModelError("TienCam", $"Số tiền cầm ({model.TienCam:N0} VNĐ) không được vượt quá 80% giá trị định giá ({maxTienCam:N0} VNĐ).");
            }

            if (ModelState.IsValid)
            {
                // 1. Tạo Hợp đồng cầm
                var hopDong = new HopDongCam
                {
                    MaKh = model.MaKh,
                    NgayCam = model.NgayCam,
                    HanChuoc = model.HanChuoc,
                    TienCam = model.TienCam,
                    LaiSuat = model.LaiSuat,
                    TrangThai = "đang cầm"
                };

                _context.HopDongCams.Add(hopDong);
                await _context.SaveChangesAsync();

                // 2. Upload ảnh tài sản (hỗ trợ nhiều ảnh)
                string? anhTaiSanPaths = null;
                if (model.AnhTaiSanFiles != null && model.AnhTaiSanFiles.Any())
                {
                    var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "taisan");
                    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                    var uploadedList = new List<string>();
                    foreach (var file in model.AnhTaiSanFiles)
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
                    if (uploadedList.Any())
                    {
                        anhTaiSanPaths = string.Join(";", uploadedList);
                    }
                }

                // 3. Tạo Tài sản liên kết với hợp đồng
                var taiSan = new TaiSan
                {
                    MaHd = hopDong.MaHd,
                    MaLoai = model.MaLoai,
                    TenTaiSan = model.TenTaiSan,
                    MoTaTinhTrang = model.MoTaTinhTrang,
                    GiaTriDinhGia = model.GiaTriDinhGia,
                    AnhTaiSan = anhTaiSanPaths
                };

                _context.TaiSans.Add(taiSan);
                await _context.SaveChangesAsync();

                // Lưu Session hợp đồng vừa tạo và xoá bản nháp
                HttpContext.Session.SetInt32("LastCreatedHopDongId", hopDong.MaHd);
                HttpContext.Session.Remove("DraftHopDong");

                AuditLogger.LogMoneyChange(User.Identity?.Name ?? "Staff", "Lập hợp đồng cầm đồ", 
                    $"Tạo hợp đồng mã HD-{hopDong.MaHd} cho khách hàng {model.MaKh}", null, hopDong.TienCam);

                TempData["SuccessMessage"] = $"Lập hợp đồng HD-{hopDong.MaHd} thành công! Tiền cầm: {hopDong.TienCam:N0} VNĐ";
                return RedirectToAction(nameof(Details), new { id = hopDong.MaHd });
            }

            // Reload danh sách chọn nếu lỗi
            ViewBag.KhachHangs = await _context.KhachHangs
                .OrderBy(k => k.HoTen)
                .Select(k => new SelectListItem
                {
                    Value = k.MaKh.ToString(),
                    Text = $"{k.HoTen} - CCCD: {k.SoCccd} - ĐT: {k.SoDienThoai}"
                }).ToListAsync();

            var loais = await _context.LoaiTaiSans.ToListAsync();
            ViewBag.LoaiTaiSans = new SelectList(loais, "MaLoai", "TenLoai", model.MaLoai);
            ViewBag.LoaiTaiSanData = loais;

            return View(model);
        }

        // GET: /HopDongCam/Edit/5
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var hopDong = await _context.HopDongCams
                .Include(h => h.MaKhNavigation)
                .Include(h => h.TaiSans)
                .FirstOrDefaultAsync(h => h.MaHd == id);

            if (hopDong == null) return NotFound();

            // QUY TẮC NGHIỆP VỤ: Chỉ được phép chỉnh sửa hợp đồng khi còn trạng thái "đang cầm"
            if (hopDong.TrangThai != "đang cầm")
            {
                TempData["ErrorMessage"] = $"Không thể chỉnh sửa! Hợp đồng HD-{hopDong.MaHd} đang ở trạng thái '{hopDong.TrangThai}'. Quy định: Chỉ được phép chỉnh sửa hợp đồng khi còn trạng thái 'đang cầm'.";
                return RedirectToAction(nameof(Details), new { id = hopDong.MaHd });
            }

            ViewBag.KhachHangs = new SelectList(await _context.KhachHangs.ToListAsync(), "MaKh", "HoTen", hopDong.MaKh);
            ViewBag.TrangThaiList = new SelectList(new[] { "đang cầm", "quá hạn", "thanh lý", "đã chuộc" }, hopDong.TrangThai);

            return View(hopDong);
        }

        // POST: /HopDongCam/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Chủ cửa hàng,Nhân viên")]
        public async Task<IActionResult> Edit(int id, [Bind("MaHd,MaKh,NgayCam,HanChuoc,TienCam,LaiSuat,TrangThai")] HopDongCam hopDong)
        {
            if (id != hopDong.MaHd) return NotFound();

            var existing = await _context.HopDongCams.AsNoTracking().FirstOrDefaultAsync(h => h.MaHd == id);
            if (existing == null) return NotFound();

            // QUY TẮC NGHIỆP VỤ: Chỉ được phép chỉnh sửa hợp đồng khi còn trạng thái "đang cầm"
            if (existing.TrangThai != "đang cầm")
            {
                TempData["ErrorMessage"] = $"Không thể lưu chỉnh sửa! Hợp đồng HD-{existing.MaHd} đang ở trạng thái '{existing.TrangThai}'. Quy định: Chỉ được phép chỉnh sửa khi còn trạng thái 'đang cầm'.";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            if (hopDong.HanChuoc <= hopDong.NgayCam)
            {
                ModelState.AddModelError("HanChuoc", "Hạn chuộc phải sau ngày cầm đồ ít nhất 1 ngày.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hopDong);
                    await _context.SaveChangesAsync();

                    if (existing.TienCam != hopDong.TienCam)
                    {
                        AuditLogger.LogMoneyChange(User.Identity?.Name ?? "Staff", "Chỉnh sửa tiền cầm hợp đồng",
                            $"Cập nhật tiền cầm hợp đồng HD-{hopDong.MaHd}", existing.TienCam, hopDong.TienCam);
                    }

                    AuditLogger.LogAction(User.Identity?.Name ?? "Staff", "Cập nhật hợp đồng", $"Cập nhật thông tin hợp đồng HD-{hopDong.MaHd}");
                    TempData["SuccessMessage"] = "Cập nhật thông tin hợp đồng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HopDongCamExists(hopDong.MaHd)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Details), new { id = hopDong.MaHd });
            }

            ViewBag.KhachHangs = new SelectList(await _context.KhachHangs.ToListAsync(), "MaKh", "HoTen", hopDong.MaKh);
            ViewBag.TrangThaiList = new SelectList(new[] { "đang cầm", "quá hạn", "thanh lý", "đã chuộc" }, hopDong.TrangThai);
            return View(hopDong);
        }

        // GET: /HopDongCam/Delete/5
        [Authorize(Roles = "Chủ cửa hàng")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var hopDong = await _context.HopDongCams
                .Include(h => h.MaKhNavigation)
                .Include(h => h.TaiSans)
                .Include(h => h.GiaoDiches)
                .FirstOrDefaultAsync(m => m.MaHd == id);

            if (hopDong == null) return NotFound();

            return View(hopDong);
        }

        // POST: /HopDongCam/Delete/5
        // Yêu cầu: "Hợp đồng đã chuộc hoặc đã thanh lý không được xóa" & "Hợp đồng đã có giao dịch không được xóa"
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Chủ cửa hàng")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hopDong = await _context.HopDongCams
                .Include(h => h.GiaoDiches)
                .Include(h => h.TaiSans)
                .FirstOrDefaultAsync(m => m.MaHd == id);

            if (hopDong != null)
            {
                // QUY TẮC NGHIỆP VỤ: Hợp đồng đã chuộc hoặc đã thanh lý không được xóa
                if (hopDong.TrangThai == "đã chuộc" || hopDong.TrangThai == "thanh lý")
                {
                    TempData["ErrorMessage"] = $"KHÔNG THỂ XÓA! Hợp đồng HD-{hopDong.MaHd} đang ở trạng thái '{hopDong.TrangThai}'. Theo quy định nghiệp vụ, hợp đồng đã chuộc hoặc đã thanh lý không được phép xóa!";
                    return RedirectToAction(nameof(Details), new { id = hopDong.MaHd });
                }

                // Hợp đồng đã có phát sinh giao dịch tài chính không được xóa
                if (hopDong.GiaoDiches.Any())
                {
                    TempData["ErrorMessage"] = $"KHÔNG THỂ XÓA! Hợp đồng HD-{hopDong.MaHd} đã phát sinh {hopDong.GiaoDiches.Count} giao dịch tài chính. Theo quy định kế toán, hợp đồng có giao dịch không được phép xóa!";
                    return RedirectToAction(nameof(Details), new { id = hopDong.MaHd });
                }

                // Xóa tài sản liên kết
                if (hopDong.TaiSans.Any())
                {
                    _context.TaiSans.RemoveRange(hopDong.TaiSans);
                }

                _context.HopDongCams.Remove(hopDong);
                await _context.SaveChangesAsync();

                AuditLogger.LogMoneyChange(User.Identity?.Name ?? "Admin", "Xóa hợp đồng cầm",
                    $"Xóa hợp đồng HD-{id} không có giao dịch", hopDong.TienCam, 0);

                TempData["SuccessMessage"] = $"Đã xóa hợp đồng HD-{id} thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /HopDongCam/InHopDong/5 (In hợp đồng ra mẫu PDF / Print Preview)
        public async Task<IActionResult> InHopDong(int? id)
        {
            if (id == null) return NotFound();

            var hopDong = await _context.HopDongCams
                .Include(h => h.MaKhNavigation)
                .Include(h => h.TaiSans)
                    .ThenInclude(t => t.MaLoaiNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);

            if (hopDong == null) return NotFound();

            // Khách hàng chỉ in hợp đồng của mình
            if (User.IsInRole("Khách hàng"))
            {
                var userMaKh = HttpContext.Session.GetInt32("MaKH");
                if (userMaKh.HasValue && hopDong.MaKh != userMaKh.Value)
                {
                    return Forbid();
                }
            }

            return View(hopDong);
        }

        // POST: /HopDongCam/DuyetThanhLy/5 (Chủ cửa hàng duyệt chuyển trạng thái sang thanh lý)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Chủ cửa hàng")]
        public async Task<IActionResult> DuyetThanhLy(int id)
        {
            var hopDong = await _context.HopDongCams.FindAsync(id);
            if (hopDong == null) return NotFound();

            hopDong.TrangThai = "thanh lý";
            await _context.SaveChangesAsync();

            AuditLogger.LogAction(User.Identity?.Name ?? "Admin", "Duyệt thanh lý", $"Duyệt thanh lý tài sản của hợp đồng HD-{id}");
            TempData["SuccessMessage"] = $"Đã duyệt thanh lý tài sản của hợp đồng HD-{id} thành công!";
            return RedirectToAction(nameof(Details), new { id = id });
        }

        private bool HopDongCamExists(int id)
        {
            return _context.HopDongCams.Any(e => e.MaHd == id);
        }
    }
}
