using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBMigreat.MigrationTools
{
    public interface IIoHelper
    {
        IEnumerable<string> GetFilesFromDirectory(string directoryPath);
        Task<string> LoadFileContentAsync(string filePath);
    }
}
