using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace QLCamDo_Nhom4_LopK24CNT2.Helpers
{
    public static class SessionExtensions
    {
        // Ghi đối tượng vào Session dưới dạng chuỗi JSON
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Đọc đối tượng từ Session và chuyển đổi ngược về kiểu dữ liệu T
        public static T? GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
