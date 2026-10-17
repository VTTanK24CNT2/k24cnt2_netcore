using System;
using System.IO;

namespace QLCamDo_Nhom4_LopK24CNT2.Helpers
{
    public static class AuditLogger
    {
        private static readonly object _lock = new();
        private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly string LogFilePath = Path.Combine(LogDirectory, "audit_log.txt");

        public static void LogMoneyChange(string user, string action, string details, decimal? oldAmount, decimal? newAmount)
        {
            try
            {
                lock (_lock)
                {
                    if (!Directory.Exists(LogDirectory))
                    {
                        Directory.CreateDirectory(LogDirectory);
                    }

                    var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] USER: {user} | ACTION: {action} | DETAILS: {details} | OLD_AMOUNT: {oldAmount:N0} VNĐ | NEW_AMOUNT: {newAmount:N0} VNĐ{Environment.NewLine}";
                    File.AppendAllText(LogFilePath, logEntry);
                }
            }
            catch
            {
                // Ignore file write exceptions in production
            }
        }

        public static void LogAction(string user, string action, string details)
        {
            try
            {
                lock (_lock)
                {
                    if (!Directory.Exists(LogDirectory))
                    {
                        Directory.CreateDirectory(LogDirectory);
                    }

                    var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] USER: {user} | ACTION: {action} | DETAILS: {details}{Environment.NewLine}";
                    File.AppendAllText(LogFilePath, logEntry);
                }
            }
            catch
            {
            }
        }

        public static string[] GetRecentLogs(int maxLines = 50)
        {
            try
            {
                lock (_lock)
                {
                    if (File.Exists(LogFilePath))
                    {
                        var lines = File.ReadAllLines(LogFilePath);
                        return lines.Length <= maxLines ? lines : lines[^maxLines..];
                    }
                }
            }
            catch
            {
            }
            return Array.Empty<string>();
        }
    }
}
