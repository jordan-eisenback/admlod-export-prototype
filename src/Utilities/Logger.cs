// Logger.cs

using System;
using System.IO;

namespace AdmLodExportSimulator.Logging
{
    public static class Logger
    {
        private static string? _logFile;

        public static void Initialize(string? logFile)
        {
            _logFile = logFile;
        }

        public static void Log(string message, string severity = "INFO")
        {
            string timestamped = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{severity}] {message}";
            Console.WriteLine(timestamped);

            if (!string.IsNullOrEmpty(_logFile))
            {
                File.AppendAllText(_logFile, timestamped + Environment.NewLine);
            }
        }

        public static void Info(string message) => Log(message, "INFO");
        public static void Warn(string message) => Log(message, "WARN");
        public static void Error(string message) => Log(message, "ERROR");
        public static void Success(string message) => Log(message, "SUCCESS");
    }
}