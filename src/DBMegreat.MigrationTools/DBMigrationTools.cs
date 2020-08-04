using System.Threading.Tasks;
using DBMegreat.MigrationTools.Repositories;

namespace DBMegreat.MigrationTools
{
    public class DBMigrationTools
    {
        private readonly IIOHelper _ioHelper;
        private readonly ITrackerRepositoryFactory _trackerRepositoryFactory;

        public DBMigrationTools(IIOHelper ioHelper, ITrackerRepositoryFactory trackerRepositoryFactory)
        {
            _ioHelper = ioHelper;
            _trackerRepositoryFactory = trackerRepositoryFactory;
        }

        public async Task Execute(string configFilePath)
        {
            var content = await _ioHelper.LoadFileContentAsync(configFilePath);
            var configuration = MigrationConfiguration.ParseConfiguration(content);
            var trackerRepository = _trackerRepositoryFactory.GetTrackerRepository(configuration.DbConnection);
            var executedScript = await trackerRepository.GetExecutedScriptsAsync();

            foreach (var directory in configuration.SqlFilesDirectories)
            {
                var directoryPath = directory.EndsWith("/") ? directory : $"{directory}/";
                var files = _ioHelper.GetFilesFromDirectory(directoryPath);
                foreach (var file in files)
                {
                    var key = $"{directoryPath}{file}";
                    if (!executedScript.ContainsKey(key))
                    {
                        var scriptContent = await _ioHelper.LoadFileContentAsync(key);
                        await trackerRepository.ExecuteNonQueryAsync(scriptContent);
                        await trackerRepository.InsertTrackingAsync(key);
                    }
                }
            }
        }
    }
}
