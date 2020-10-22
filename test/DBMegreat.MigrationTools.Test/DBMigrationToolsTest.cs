using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBMegreat.MigrationTools.Repositories;
using DBMegreat.MigrationTools.Test.Utils;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace DBMegreat.MigrationTools.Test
{
    public class DBMigrationToolsTest
    {
        private readonly Mock<IIOHelper> _ioHelper;
        private readonly Mock<ITrackerRepositoryFactory> _trackerRepositoryFactory;
        private readonly Mock<ITrackerRepository> _trackerRepository;
        private readonly Mock<ILogger> _logger;
        private readonly DBMigrationTools _migrationTools;

        private const string errorLogMessage = "There is a problem during updating the database schema.";
        private const string configFilePath = "config_file_path.json";
        private const string validConfigContent = @"
                {
                    ""sql_files_directories"": [
                        ""/directory1/"",
                        ""../directory2/""
                    ],
                    ""db_connection"": {
                        ""type"": ""mysql"",
                        ""connection_string"": ""Server=HOST_NAME;Database=DB_NAME;Uid=USER_ID;Pwd=PASSWORD""
                    },
                    ""log_output"": ""../directory/output""
                }
            ";

        private readonly IOException ioException = new IOException("An IO exception");
        private readonly Dictionary<string, SchemaHistoryRecord> schemaHistory = new Dictionary<string, SchemaHistoryRecord>
        {
                { "/directory1/1.sql", new SchemaHistoryRecord("/directory1/1.sql", DateTime.Now) },
                { "/directory1/2.sql", new SchemaHistoryRecord("/directory1/2.sql", DateTime.Now) },
                { "/directory1/3.sql", new SchemaHistoryRecord("/directory1/3.sql", DateTime.Now) }
                { "../directory2/1.sql", new SchemaHistoryRecord("../directory2/1.sql", DateTime.Now) }
        };


        public DBMigrationToolsTest(ITestOutputHelper output)
        {
            _ioHelper = new Mock<IIOHelper>();
            _ioHelper.Setup(x => x.LoadFileContent(configFilePath)).Returns(validConfigContent);
            _ioHelper.
            _trackerRepository = new Mock<ITrackerRepository>();
            _trackerRepository.Setup(x => x.CheckTrackTableExistAsync()).ReturnsAsync(true);
            _trackerRepository.Setup(x => x.GetSchemaHistoryRecordsAsync()).ReturnsAsync(schemaHistory);

            _trackerRepositoryFactory = new Mock<ITrackerRepositoryFactory>();
            _trackerRepositoryFactory.Setup(x => x.GetTrackerRepository(It.IsAny<ConnectionConfiguration>()))
                .Returns(_trackerRepository.Object);

            _logger = new Mock<ILogger>();

            _migrationTools = new DBMigrationTools(_ioHelper.Object, _trackerRepositoryFactory.Object, new TestLogger(output, _logger.Object));
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

        [Fact]
        public async Task ExecuteAsync_TrackTableDoesNotExist_ShouldCallCreateTrackTableAsync()
        {
            _trackerRepositoryFactory.Setup(x => x.GetTrackerRepository(It.IsAny<ConnectionConfiguration>())).Throws(new TrackerRepositoryException("error"));
            await _migrationTools.ExecuteAsync(configFilePath);
            _logger.Verify(x =>
                x.Error(It.Is<string>(s => s == errorLogMessage),
                        It.Is<Exception>(ex => ex.GetType() == typeof(TrackerRepositoryException))));
        }
    }
}
