using System;
using System.IO;
using System.Windows.Forms;

namespace DefectDetectionSystem
{
    public static class Logger
    {
        private static string logFile = Path.Combine(Application.StartupPath, "debug_log.txt");

        public static void Log(string message)
        {
            try
            {
                string logMessage = $"{DateTime.Now:HH:mm:ss.fff} - {message}";
                File.AppendAllText(logFile, logMessage + Environment.NewLine);
                System.Diagnostics.Debug.WriteLine(logMessage);
            }
            catch { }
        }

        public static void Clear()
        {
            try
            {
                if (File.Exists(logFile))
                    File.Delete(logFile);
            }
            catch { }
        }

        public static void ShowLog()
        {
            try
            {
                if (File.Exists(logFile))
                    System.Diagnostics.Process.Start(logFile);
            }
            catch { }
        }
    }
}