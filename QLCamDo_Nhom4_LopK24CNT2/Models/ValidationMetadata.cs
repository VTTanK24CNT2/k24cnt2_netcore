using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace QLCamDo_Nhom4_LopK24CNT2.Models
{
    [ModelMetadataType(typeof(KhachHangMetadata))]
    public partial class KhachHang
    {
    }

    public class KhachHangMetadata
    {
        [Display(Name = "Mã khách hàng")]
        public int MaKh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [StringLength(100, ErrorMessage = "Họ tên không vượt quá 100 ký tự")]
        [Display(Name = "Họ và tên")]
        public string HoTen { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập số CCCD")]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Số CCCD phải gồm đúng 12 chữ số")]
        [Display(Name = "Số CCCD")]
        public string SoCccd { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Số điện thoại phải gồm 10 chữ số và bắt đầu bằng số 0")]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; } = null!;

        [StringLength(255, ErrorMessage = "Địa chỉ không vượt quá 255 ký tự")]
        [Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }

        [Display(Name = "Ảnh CCCD")]
        public string? AnhCccd { get; set; }
    }

    [ModelMetadataType(typeof(LoaiTaiSanMetadata))]
    public partial class LoaiTaiSan
    {
    }

    public class LoaiTaiSanMetadata
    {
        [Display(Name = "Mã loại")]
        public int MaLoai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên loại tài sản")]
        [StringLength(100, ErrorMessage = "Tên loại không vượt quá 100 ký tự")]
        [Display(Name = "Tên loại tài sản")]
        public string TenLoai { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập lãi suất theo ngày")]
        [Range(0.01, 10.0, ErrorMessage = "Lãi suất ngày phải từ 0.01% đến 10%")]
        [Display(Name = "Lãi suất (% / ngày)")]
        public decimal LaiSuatNgay { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số ngày tối đa")]
        [Range(1, 365, ErrorMessage = "Số ngày tối đa từ 1 đến 365 ngày")]
        [Display(Name = "Số ngày tối đa")]
        public int SoNgayToiDa { get; set; }
    }

    [ModelMetadataType(typeof(HopDongCamMetadata))]
    public partial class HopDongCam
    {
    }

    public class HopDongCamMetadata
    {
        [Display(Name = "Mã hợp đồng")]
        public int MaHd { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn khách hàng")]
        [Display(Name = "Khách hàng")]
        public int MaKh { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày cầm")]
        [Display(Name = "Ngày cầm")]
        public DateOnly NgayCam { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hạn chuộc")]
        [Display(Name = "Hạn chuộc")]
        public DateOnly HanChuoc { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số tiền cầm")]
        [Range(100000, 10000000000, ErrorMessage = "Số tiền cầm tối thiểu 100.000 VNĐ")]
        [Display(Name = "Số tiền cầm (VNĐ)")]
        public decimal TienCam { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lãi suất")]
        [Range(0.01, 10.0, ErrorMessage = "Lãi suất phải từ 0.01% đến 10%/ngày")]
        [Display(Name = "Lãi suất (%/ngày)")]
        public decimal LaiSuat { get; set; }

        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = null!;
    }

    [ModelMetadataType(typeof(TaiSanMetadata))]
    public partial class TaiSan
    {
    }

    public class TaiSanMetadata
    {
        [Display(Name = "Mã tài sản")]
        public int MaTs { get; set; }

        [Display(Name = "Hợp đồng")]
        public int MaHd { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại tài sản")]
        [Display(Name = "Loại tài sản")]
        public int MaLoai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tài sản")]
        [StringLength(150, ErrorMessage = "Tên tài sản không vượt quá 150 ký tự")]
        [Display(Name = "Tên tài sản")]
        public string TenTaiSan { get; set; } = null!;

        [Display(Name = "Mô tả tình trạng")]
        public string? MoTaTinhTrang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá trị định giá")]
        [Range(100000, 10000000000, ErrorMessage = "Giá trị định giá tối thiểu 100.000 VNĐ")]
        [Display(Name = "Giá trị định giá (VNĐ)")]
        public decimal? GiaTriDinhGia { get; set; }

        [Display(Name = "Ảnh tài sản")]
        public string? AnhTaiSan { get; set; }
    }

    [ModelMetadataType(typeof(GiaoDichMetadata))]
    public partial class GiaoDich
    {
    }

    public class GiaoDichMetadata
    {
        [Display(Name = "Mã giao dịch")]
        public int MaGd { get; set; }

        [Display(Name = "Mã hợp đồng")]
        public int MaHd { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại giao dịch")]
        [Display(Name = "Loại giao dịch")]
        public string LoaiGd { get; set; } = null!;

        [Display(Name = "Ngày giao dịch")]
        public DateTime NgayGd { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số tiền giao dịch")]
        [Range(1000, 10000000000, ErrorMessage = "Số tiền giao dịch phải lớn hơn 0 VNĐ")]
        [Display(Name = "Số tiền (VNĐ)")]
        public decimal SoTien { get; set; }

        [Display(Name = "Nhân viên thu")]
        public string NhanVienThu { get; set; } = null!;
    }
}
