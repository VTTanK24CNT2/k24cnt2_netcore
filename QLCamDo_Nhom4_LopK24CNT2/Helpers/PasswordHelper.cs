using System.Security.Cryptography;
using System.Text;

namespace QLCamDo_Nhom4_LopK24CNT2.Helpers
{
    public static class PasswordHelper
    {
        private const string Salt = "_QLCamDoSalt_2026";

        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty;
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password + Salt);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash)) return false;
            return HashPassword(password) == storedHash;
        }
    }
}
