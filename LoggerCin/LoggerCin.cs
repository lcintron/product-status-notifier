using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cintron.LoggerCin
{
    public class LoggerCin
    {
        public enum LogType { Information, Error, Fatal, Warning, Debug}

        private String filePath;
        private bool append;
        public LoggerCin(String FullFilePath, bool Append = true)
        {
            this.filePath = FullFilePath;
            this.append = Append;
        }

        public void Debug(String message )
        {
            WriteLine(FormatErrorMessage(message, LogType.Debug));
        }

        public void Fatal(String message)
        {
            WriteLine(FormatErrorMessage(message, LogType.Fatal));
        }

        public void Info(String message)
        {
            WriteLine(FormatErrorMessage(message, LogType.Information));
        }

        public void Warning(String message)
        {
            WriteLine(FormatErrorMessage(message, LogType.Warning));
        }

        public void Error(String message)
        {
            WriteLine(FormatErrorMessage(message, LogType.Error));
        }

        private String FormatErrorMessage(String message, LogType logType)
        {
            message = message.Trim();
            switch(logType)
            {
                case LogType.Debug: message = "[DEBUG] " + message; break;
                case LogType.Information: message = "[INFO] " + message; break;
                case LogType.Warning: message = "[WARNING] " + message; break;
                case LogType.Error: message = "[ERROR] " + message; break;
                case LogType.Fatal: message = "[FATAL] " + message; break;
                default: break;
            }
            return message;
        }

        private void WriteLine(String text)
        {
            if(String.IsNullOrEmpty(this.filePath))
            {
                this.filePath = DateTime.Now.ToLocalTime().ToString("ddMMMyyyy_HHMM") + "-LoggerCin.log";
            }
            using (StreamWriter streamWriter = new StreamWriter(this.filePath,this.append, Encoding.UTF8))
            {
                if (!String.IsNullOrEmpty(text))
                {
                    streamWriter.WriteLine(DateTime.Now.ToLocalTime().ToString("ddMMMyyyy HH:MM:ss.fff"));
                    streamWriter.WriteLine(text);
                    streamWriter.WriteLine();
                }
                streamWriter.Close();
            }
        }
    }
}
