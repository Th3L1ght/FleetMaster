using System;
using System.IO;
using FleetMaster.Core.Interfaces;

namespace FleetMaster.Infrastructure.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string _logDirectory = "logs";
        private readonly string _filePath;

        public FileLogger()
        {
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            string fileName = $"log_{DateTime.Now:yyyy_MM_dd}.txt";
            _filePath = Path.Combine(_logDirectory, fileName);
        }

        public void LogInfo(string message)
        {
            WriteToFile("INFO", message);
        }

        public void LogWarning(string message)
        {
            WriteToFile("WARNING", message);
        }

        public void LogError(string message, Exception ex = null)
        {
            string logMsg = message;
            if (ex != null)
            {
                logMsg += $" | Exception: {ex.Message}";
            }
            WriteToFile("ERROR", logMsg);
        }

        private void WriteToFile(string level, string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:HH:mm:ss}] [{level}] {message}";
                File.AppendAllText(_filePath, logEntry + Environment.NewLine);
            }
            catch
            {
            }
        }
    }
}