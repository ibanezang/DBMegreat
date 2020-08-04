using System;
using System.Collections.Generic;
using System.IO;

namespace DBMegreat.MigrationTools
{
    public interface IIOHelper
    {
        IEnumerable<string> GetFilesFromDirectory(string directoryPath);
        string LoadFileContent(string filePath);
    }

    public class IOHelper : IIOHelper
    {
        public IEnumerable<string> GetFilesFromDirectory(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*.sql", SearchOption.TopDirectoryOnly);
        }

        public string LoadFileContent(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }

    public class IOException : Exception
    {
        public IOException(string message) : base(message) { }
        public IOException(string message, Exception inner) : base(message, inner) { }
    }
}
