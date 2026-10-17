using System;
using System.Collections.Generic;

namespace QLCamDo_Nhom4_LopK24CNT2.Models;

public partial class TaiKhoan
{
    public int MaTk { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhauHash { get; set; } = null!;

    public string VaiTro { get; set; } = null!;

    public int? MaKh { get; set; }

    public virtual KhachHang? MaKhNavigation { get; set; }
}
