using System;
using QLCamDo_Nhom4_LopK24CNT2.Models;

namespace QLCamDo_Nhom4_LopK24CNT2.Helpers
{
    public static class InterestCalculator
    {
        /// <summary>
        /// Tính số ngày giữa 2 ngày. Tối thiểu 1 ngày nếu cùng ngày.
        /// </summary>
        public static int CalculateDays(DateOnly fromDate, DateOnly toDate)
        {
            var days = toDate.DayNumber - fromDate.DayNumber;
            return days > 0 ? days : 1;
        }

        /// <summary>
        /// Tính số ngày cầm cố đến hiện tại (hoặc đến hạn chuộc nếu hợp đồng đã hoàn tất)
        /// </summary>
        public static int GetDaysToDate(DateOnly startDate, DateOnly? endDate = null)
        {
            var targetDate = endDate ?? DateOnly.FromDateTime(DateTime.Now);
            return CalculateDays(startDate, targetDate);
        }

        /// <summary>
        /// Tính tiền lãi tự động theo số ngày và lãi suất %/ngày
        /// Công thức: TienCam * (LaiSuat / 100) * SoNgay
        /// </summary>
        public static decimal CalculateInterest(decimal principal, decimal interestRatePerDay, int days)
        {
            if (principal <= 0 || interestRatePerDay <= 0 || days <= 0) return 0;
            return Math.Round(principal * (interestRatePerDay / 100m) * days, 0);
        }

        /// <summary>
        /// Tính tiền lãi tạm tính cho một hợp đồng cầm cố đến ngày hiện tại
        /// </summary>
        public static decimal GetCurrentInterest(HopDongCam hopDong)
        {
            if (hopDong == null) return 0;
            var today = DateOnly.FromDateTime(DateTime.Now);
            var days = CalculateDays(hopDong.NgayCam, today);
            return CalculateInterest(hopDong.TienCam, hopDong.LaiSuat, days);
        }

        /// <summary>
        /// Tính tiền lãi cả kỳ hợp đồng (từ ngày cầm đến hạn chuộc)
        /// </summary>
        public static decimal GetFullTermInterest(HopDongCam hopDong)
        {
            if (hopDong == null) return 0;
            var days = CalculateDays(hopDong.NgayCam, hopDong.HanChuoc);
            return CalculateInterest(hopDong.TienCam, hopDong.LaiSuat, days);
        }
    }
}
