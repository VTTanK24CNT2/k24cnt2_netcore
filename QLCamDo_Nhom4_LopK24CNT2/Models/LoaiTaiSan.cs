using System;
using System.Collections.Generic;

namespace QLCamDo_Nhom4_LopK24CNT2.Models;

public partial class LoaiTaiSan
{
    public int MaLoai { get; set; }

    public string TenLoai { get; set; } = null!;

    public decimal LaiSuatNgay { get; set; }

    public int SoNgayToiDa { get; set; }

    public virtual ICollection<TaiSan> TaiSans { get; set; } = new List<TaiSan>();
}
