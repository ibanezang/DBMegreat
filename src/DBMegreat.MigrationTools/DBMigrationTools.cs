using System.Threading.Tasks;
using DBMegreat.MigrationTools.Repositories;

namespace DBMegreat.MigrationTools
{
    public class DBMigrationTools
    {
        private readonly IIOHelper _ioHelper;
        private readonly ITrackerRepositoryFactory _megreatTracksRepositoryFactory;

        public DBMigrationTools(IIOHelper ioHelper, ITrackerRepositoryFactory megreatTracksRepositoryFactory)
        {
            _ioHelper = ioHelper;
            _megreatTracksRepositoryFactory = megreatTracksRepositoryFactory;
        }

        public async Task Execute(string configFilePath)
        {
            var content = await _ioHelper.LoadFileContentAsync(configFilePath);
            var configuration = MigrationConfiguration.ParseConfiguration(content);
            var trackRepository = _megreatTracksRepositoryFactory.GetTrackerRepository(configuration.DbConnection);
            var executedScript = await trackRepository.GetExecutedScriptsAsync();

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
                        await trackRepository.ExecuteNonQueryAsync(scriptContent);
                        await trackRepository.InsertTrackingAsync(key);
                    }
                }
            }
        }
    }
}
