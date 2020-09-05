using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBMegreat.MigrationTools.Repositories;
using Moq;
using Shouldly;
using Xunit;

namespace DBMegreat.MigrationTools.Test
{
    public class DBMigrationToolsTest
    {
        private readonly Mock<IIOHelper> _ioHelper;
        private readonly Mock<ITrackerRepositoryFactory> _trackerRepositoryFactory;
        private readonly Mock<ILogger> _logger;
        private readonly DBMigrationTools _migrationTools;

        private const string errorLogMessage = "There is a problem during updating the database schema.";
        private const string configFilePath = "config_file_path.json";
        private const string validConfigContent = @"
                {
                    ""sql_files_directories"": [
                        ""/your/directory/contains/sql"",
                        ""../another/directory/contains/sql""
                    ],
                    ""db_connection"": {
                        ""type"": ""mysql"",
                        ""connection_string"": ""Server=HOST_NAME;Database=DB_NAME;Uid=USER_ID;Pwd=PASSWORD""
                    },
                    ""log_output"": ""../directory/output""
                }
            ";

        private readonly IOException ioException = new IOException("An IO exception");

        public DBMigrationToolsTest()
        {
            _ioHelper = new Mock<IIOHelper>();
            _ioHelper.Setup(x => x.LoadFileContent(configFilePath)).Returns(validConfigContent);

            _trackerRepositoryFactory = new Mock<ITrackerRepositoryFactory>();
            _logger = new Mock<ILogger>();

            _migrationTools = new DBMigrationTools(_ioHelper.Object, _trackerRepositoryFactory.Object, _logger.Object);
        }

        [Fact]
        public async Task ExecuteAsync_CanNotLoadConfigurationContent_ShouldLogIOException()
        {
            _ioHelper.Setup(x => x.LoadFileContent(configFilePath)).Throws(ioException);
            await _migrationTools.ExecuteAsync(configFilePath);
            _logger.Verify(x =>
                x.Error(It.Is<string>(s => s == errorLogMessage),
                        It.Is<Exception>(ex => ex.Message == ioException.Message && ex.GetType() == typeof(IOException))));
        }

        [Fact]
        public async Task ExecuteAsync_ConfigurationContentNotValid_ShouldLogInvalidConfigurationException()
        {
            _ioHelper.Setup(x => x.LoadFileContent(configFilePath)).Returns("invalid content");
            await _migrationTools.ExecuteAsync(configFilePath);
            _logger.Verify(x =>
                x.Error(It.Is<string>(s => s == errorLogMessage),
                        It.Is<Exception>(ex => ex.GetType() == typeof(InvalidConfigurationException))));
        }

        [Fact]
        public async Task ExecuteAsync_TrackerRepositoryThrowAnException_ShouldLogTrackerRepositoryException()
        {
            _trackerRepositoryFactory.Setup(x => x.GetTrackerRepository(It.IsAny<ConnectionConfiguration>())).Throws(new TrackerRepositoryException("error"));
            await _migrationTools.ExecuteAsync(configFilePath);
            _logger.Verify(x =>
                x.Error(It.Is<string>(s => s == errorLogMessage),
                        It.Is<Exception>(ex => ex.GetType() == typeof(TrackerRepositoryException))));
        }
    }
}
