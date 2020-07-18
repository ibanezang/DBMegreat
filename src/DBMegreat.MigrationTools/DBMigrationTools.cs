using System;
using System.Threading.Tasks;

namespace DBMegreat.MigrationTools
{
    public class DBMigrationTools
    {
        private readonly IIoHelper _ioHelper;

        public DBMigrationTools(IIoHelper ioHelper)
        {
            _ioHelper = ioHelper;
        }

        public async Task Execute(string configFilePath)
        {
            var content = await _ioHelper.LoadFileContentAsync(configFilePath);
            var configuration = MigrationConfiguration.ParseConfiguration(content);

            foreach (var directory in configuration.SqlFilesDirectories)
            {
                var files = _ioHelper.GetFilesFromDirectory(directory);

            }
        }
    }
}
