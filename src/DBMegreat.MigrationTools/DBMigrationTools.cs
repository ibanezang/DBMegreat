using System;
using System.Linq;
using System.Threading.Tasks;
using DBMegreat.MigrationTools.Repositories;

namespace DBMegreat.MigrationTools
{
    public class DBMigrationTools
    {
        private readonly IIOHelper _ioHelper;
        private readonly ITrackerRepositoryFactory _trackerRepositoryFactory;
        private readonly ILogger _logger;

        public DBMigrationTools(IIOHelper ioHelper, ITrackerRepositoryFactory trackerRepositoryFactory, ILogger logger)
        {
            _ioHelper = ioHelper;
            _trackerRepositoryFactory = trackerRepositoryFactory;
            _logger = logger;
        }

        public async Task ExecuteAsync(string configFilePath)
        {
            var totalNewExecutedScript = 0;
            try
            {
                _logger.Info($"Loading configuration file {configFilePath}.");
                var configContent = _ioHelper.LoadFileContent(configFilePath);
                var configuration = MigrationConfiguration.ParseConfiguration(configContent);
                _logger.Info($"Configuration file {configFilePath} loaded successfully.");

                var trackerRepository = _trackerRepositoryFactory.GetTrackerRepository(configuration.DbConnection);

                _logger.Info($"Checking if db_megreat_track table available in the database.");
                if (!await trackerRepository.CheckTrackTableExistAsync())
                {
                    _logger.Info($"db_megreat_track table is not available in the database. Creating the table.");
                    await trackerRepository.CreateTrackTableAsync();
                    _logger.Info($"db_megreat_track table successfully created.");
                }
                else
                {
                    _logger.Info("db_megreat_track table is available in the database.");
                }

                _logger.Info("Loading schema history records from db_megreat_track table.");
                var schemaHistoryRecords = await trackerRepository.GetSchemaHistoryRecordsAsync();
                _logger.Info("Schema history records loaded successfully.");

                foreach (var directory in configuration.SqlFilesDirectories)
                {
                    _logger.Info($"Searching {directory} directory for *.sql files.");
                    var directoryPath = directory.EndsWith("/") ? directory : $"{directory}/";
                    var files = _ioHelper.GetFilesFromDirectory(directoryPath);
                    _logger.Info($"{files.Count()} *.sql file(s) have been found in {directory}.");

                    foreach (var file in files.OrderBy(x => x))
                    {
                        if (!schemaHistoryRecords.ContainsKey(file))
                        {
                            _logger.Info($"Executing {file}.");
                            var scriptContent = _ioHelper.LoadFileContent(file);
                            await trackerRepository.ExecuteNonQueryAsync(scriptContent);
                            await trackerRepository.InsertTrackingAsync(file);
                            _logger.Info($"{file} has been executed successfully.");

                            totalNewExecutedScript++;
                        }
                    }
                }

                if (totalNewExecutedScript > 0)
                {
                    _logger.Info($"{totalNewExecutedScript} script(s) have been executed. Your database shcema is no up to date.");
                }
                else
                {
                    _logger.Info("No new script has been found. Your database schema is up to date.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("There is a problem during updating the database schema.", ex);
            }
        }
    }
}
