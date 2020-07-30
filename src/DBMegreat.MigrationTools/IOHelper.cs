using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBMegreat.MigrationTools
{
    public interface IIOHelper
    {
        IEnumerable<string> GetFilesFromDirectory(string directoryPath);
        Task<string> LoadFileContentAsync(string filePath);
    }

    public class IOException : Exception
    {
        public IOException(string message) : base(message) { }
        public IOException(string message, Exception inner) : base(message, inner) { }
    }
}
