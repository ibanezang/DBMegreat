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
            try
            {
                return Directory.GetFiles(directoryPath, "*.sql", SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failure on getting file list in {directoryPath}", ex);
            }
        }

        public string LoadFileContent(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failure on reading file {filePath}", ex);
            }
        }
    }

    public class IOException : Exception
    {
        public IOException(string message) : base(message) { }
        public IOException(string message, Exception inner) : base(message, inner) { }
    }
}
