using System;
using System.Threading.Tasks;

namespace DBMigreat.MigrationTools
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
        }

        public void LoadConfigFile(string configFilePath)
        {

        }
    }
}
