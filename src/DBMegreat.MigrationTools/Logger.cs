using System;
using System.IO;

namespace DBMegreat.MigrationTools
{
    public interface ILogger
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message, Exception ex);
    }

    public class BaseLogger
    {
        private string _currentDateTime => DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");
        protected string FormatInfo(string message) => $"[INFO][{_currentDateTime}] {message}";
        protected string FormatWarning(string message) => $"[WARNING][{_currentDateTime}] {message}";
        protected string FormatError(string message, Exception ex) => $"[ERROR][{_currentDateTime}] {message} - {ex.Message} - {ex.StackTrace}";
    }

    public class EmptyLogger : BaseLogger, ILogger
    {
        public void Error(string message, Exception ex)
        {
        }

        public void Info(string message)
        {
        }

        public void Warning(string message)
        {
        }
    }

    public class ConsoleLogger : BaseLogger, ILogger
    {
        private readonly ILogger _decorated;

        public ConsoleLogger()
        {
            _decorated = new EmptyLogger();
        }

        public ConsoleLogger(ILogger decorated)
        {
            _decorated = decorated;
        }

        public void Error(string message, Exception ex)
        {
            _decorated.Error(message, ex);
            Console.Out.WriteLine(FormatError(message, ex));
        }

        public void Info(string message)
        {
            _decorated.Info(message);
            Console.Out.WriteLine(FormatInfo(message));
        }

        public void Warning(string message)
        {
            _decorated.Warning(message);
            Console.Out.WriteLine(FormatWarning(message));
        }
    }

    public class FileLogger : BaseLogger, ILogger
    {
        private string _filePath;
        private ILogger _decorated;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
            _decorated = new EmptyLogger();
        }

        public FileLogger(string filePath, ILogger decorated)
        {
            _filePath = filePath;
            _decorated = decorated;
        }

        public void Error(string message, Exception ex)
        {
            _decorated.Error(message, ex);
            OpenFileAndWrite(_filePath, FormatError(message, ex));

        }

        public void Info(string message)
        {
            _decorated.Info(message);
            OpenFileAndWrite(_filePath, FormatInfo(message));
        }

        public void Warning(string message)
        {
            _decorated.Warning(message);
            OpenFileAndWrite(_filePath, FormatWarning(message));
        }

        private void OpenFileAndWrite(string filePath, string message)
        {
            var file = new StreamWriter(File.OpenWrite(filePath));
            file.WriteLine(message);
            file.Close();
        }
    }
}
