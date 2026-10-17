using System;
using System.Collections.Generic;

namespace QLCamDo_Nhom4_LopK24CNT2.Models;

public partial class GiaoDich
{
    public int MaGd { get; set; }

    public int MaHd { get; set; }

    public string LoaiGd { get; set; } = null!;

    public DateTime NgayGd { get; set; }

    public decimal SoTien { get; set; }

    public string NhanVienThu { get; set; } = null!;

    public virtual HopDongCam MaHdNavigation { get; set; } = null!;
}
