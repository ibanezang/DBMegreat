using System;
using System.Threading.Tasks;
using DBMegreat.MigrationTools;
using DBMegreat.MigrationTools.Repositories;

namespace DBMegreat.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            test
            var ioHelper = new IOHelper();
            var trackerRepositoryFactory = new MegreatTracksRepositoryFactory();
            var logger = new ConsoleLogger();
            await new DBMigrationTools(ioHelper, trackerRepositoryFactory, logger).ExecuteAsync(args[0]);
        }
    }
}
