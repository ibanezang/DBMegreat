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

        public async Task ExecuteAsync(string configFilePath)
        {
            var configContent = _ioHelper.LoadFileContent(configFilePath);
            var configuration = MigrationConfiguration.ParseConfiguration(configContent);
            var trackerRepository = _trackerRepositoryFactory.GetTrackerRepository(configuration.DbConnection);

            if (!await trackerRepository.CheckTrackTableExistAsync())
            {
                await trackerRepository.CreateTrackTableAsync();
            }

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
                        var scriptContent = _ioHelper.LoadFileContent(key);
                        await trackerRepository.ExecuteNonQueryAsync(scriptContent);
                        await trackerRepository.InsertTrackingAsync(key);
                    }
                }
            }
        }
    }
}
