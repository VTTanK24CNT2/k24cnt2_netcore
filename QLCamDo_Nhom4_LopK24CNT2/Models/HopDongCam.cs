using System;
using System.Collections.Generic;

namespace QLCamDo_Nhom4_LopK24CNT2.Models;

public partial class HopDongCam
{
    public int MaHd { get; set; }

    public int MaKh { get; set; }

    public DateOnly NgayCam { get; set; }

    public DateOnly HanChuoc { get; set; }

    public decimal TienCam { get; set; }

    public decimal LaiSuat { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual ICollection<GiaoDich> GiaoDiches { get; set; } = new List<GiaoDich>();

    public virtual KhachHang MaKhNavigation { get; set; } = null!;

    public virtual ICollection<TaiSan> TaiSans { get; set; } = new List<TaiSan>();
}
