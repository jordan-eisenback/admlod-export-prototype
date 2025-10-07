// Logger.cs

using System;
using System.IO;

namespace AdmLodExportSimulator.Logging
{
    public static class Logger
    {
        private static string? _logFilePath;

        public static void Initialize(string? logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public static void Log(string message, string severity)
        {
            try
            {
                var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{severity}] {message}";
                Console.WriteLine(logMessage);

                if (!string.IsNullOrWhiteSpace(_logFilePath))
                {
                    using (var stream = new FileStream(_logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(logMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"?? Logger failed to write: {ex.Message}");
            }
        }

        public static void Info(string message) => Log(message, "INFO");
        public static void Warn(string message) => Log(message, "WARN");
        public static void Error(string message) => Log(message, "ERROR");
        public static void Success(string message) => Log(message, "SUCCESS");
    }
}