using System.Data;
using MySql.Data.MySqlClient;

namespace DBMegreat.MigrationTools.Repositories
{
    public class MySqlTrackerRepository : BaseSqlTrackerRepository
    {

        public MySqlTrackerRepository(ConnectionConfiguration connectionConfiguration) : base(connectionConfiguration)
        {
        }

        public override IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionConfiguration.ConnectionString);
        }
    }
}
