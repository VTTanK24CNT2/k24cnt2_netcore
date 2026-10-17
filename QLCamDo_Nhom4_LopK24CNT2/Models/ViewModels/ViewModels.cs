using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLCamDo_Nhom4_LopK24CNT2.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = string.Empty;

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool GhiNho { get; set; } = false;
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Tên đăng nhập từ 4 đến 50 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        [Display(Name = "Xác nhận mật khẩu")]
        public string XacNhanMatKhau { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số CCCD")]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Số CCCD phải gồm đúng 12 chữ số")]
        [Display(Name = "Số CCCD")]
        public string SoCccd { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Số điện thoại phải gồm 10 chữ số và bắt đầu bằng số 0")]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; } = string.Empty;

        [Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }

        [Display(Name = "Ảnh CCCD")]
        public IFormFile? AnhCccdFile { get; set; }
    }

    public class HopDongCreateViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn khách hàng")]
        [Display(Name = "Khách hàng")]
        public int MaKh { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại tài sản")]
        [Display(Name = "Loại tài sản")]
        public int MaLoai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tài sản")]
        [StringLength(150, ErrorMessage = "Tên tài sản không quá 150 ký tự")]
        [Display(Name = "Tên tài sản cầm cố")]
        public string TenTaiSan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập giá trị định giá")]
        [Range(100000, 10000000000, ErrorMessage = "Giá trị định giá tối thiểu 100.000 VNĐ")]
        [Display(Name = "Giá trị định giá (VNĐ)")]
        public decimal GiaTriDinhGia { get; set; }

        [Display(Name = "Mô tả tình trạng tài sản")]
        public string? MoTaTinhTrang { get; set; }

        [Display(Name = "Ảnh chụp tài sản (nhiều ảnh)")]
        public List<IFormFile>? AnhTaiSanFiles { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày cầm")]
        [Display(Name = "Ngày cầm")]
        public DateOnly NgayCam { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Required(ErrorMessage = "Vui lòng chọn hạn chuộc")]
        [Display(Name = "Hạn chuộc")]
        public DateOnly HanChuoc { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(30));

        [Required(ErrorMessage = "Vui lòng nhập số tiền cầm")]
        [Range(100000, 10000000000, ErrorMessage = "Số tiền cầm tối thiểu 100.000 VNĐ")]
        [Display(Name = "Số tiền cầm (VNĐ)")]
        public decimal TienCam { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lãi suất")]
        [Range(0.01, 10.0, ErrorMessage = "Lãi suất phải từ 0.01% đến 10%/ngày")]
        [Display(Name = "Lãi suất (%/ngày)")]
        public decimal LaiSuat { get; set; } = 0.25m;
    }

    public class GiaoDichCreateViewModel
    {
        [Required]
        [Display(Name = "Mã hợp đồng")]
        public int MaHd { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại giao dịch")]
        [Display(Name = "Loại giao dịch")]
        public string LoaiGd { get; set; } = "đóng lãi"; // đóng lãi, gia hạn, chuộc

        [Required(ErrorMessage = "Vui lòng nhập số tiền giao dịch")]
        [Range(1000, 10000000000, ErrorMessage = "Số tiền giao dịch phải lớn hơn 0")]
        [Display(Name = "Số tiền thanh toán (VNĐ)")]
        public decimal SoTien { get; set; }

        [Display(Name = "Gia hạn đến ngày (nếu gia hạn)")]
        public DateOnly? GiaHanDenNgay { get; set; }

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }
    }

    public class ThongKeViewModel
    {
        public decimal TongTienChoVay { get; set; }
        public decimal TongLaiDaThu { get; set; }
        public int SoHopDongDangCam { get; set; }
        public int SoHopDongSapDenHan { get; set; }
        public int SoHopDongQuaHan { get; set; }
        public int SoHopDongThanhLy { get; set; }
        public int TongSoKhachHang { get; set; }

        // Dữ liệu biểu đồ doanh thu 6 hoặc 12 tháng gần nhất
        public List<string> ChartLabels { get; set; } = new();
        public List<decimal> ChartTienChoVay { get; set; } = new();
        public List<decimal> ChartLaiThuVe { get; set; } = new();

        public List<HopDongCam> HopDongSapDenHanList { get; set; } = new();
        public List<HopDongCam> HopDongQuaHanList { get; set; } = new();
    }

    public class DangKyMuaThanhLyViewModel
    {
        [Required]
        public int MaTs { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [Display(Name = "Họ và tên")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; } = string.Empty;

        [Display(Name = "Giá đề xuất mua (VNĐ)")]
        public decimal? GiaDeXuat { get; set; }

        [Display(Name = "Ghi chú / Lời nhắn")]
        public string? GhiChu { get; set; }
    }
}
