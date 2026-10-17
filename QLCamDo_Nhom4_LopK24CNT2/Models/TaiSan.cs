using System;
using System.Collections.Generic;

namespace QLCamDo_Nhom4_LopK24CNT2.Models;

public partial class TaiSan
{
    public int MaTs { get; set; }

    public int MaHd { get; set; }

    public int MaLoai { get; set; }

    public string TenTaiSan { get; set; } = null!;

    public string? MoTaTinhTrang { get; set; }

    public decimal? GiaTriDinhGia { get; set; }

    public string? AnhTaiSan { get; set; }

    public virtual HopDongCam MaHdNavigation { get; set; } = null!;

    public virtual LoaiTaiSan MaLoaiNavigation { get; set; } = null!;
}
