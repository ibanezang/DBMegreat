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
            var ioHelper = new IOHelper();
            var trackerRepositoryFactory = new MegreatTracksRepositoryFactory();
            try
            {
                await new DBMigrationTools(ioHelper, trackerRepositoryFactory).ExecuteAsync(args[0]);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }

        }
    }
}
