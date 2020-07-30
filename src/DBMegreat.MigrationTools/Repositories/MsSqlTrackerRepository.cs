using System.Data;
using System.Data.SqlClient;

namespace DBMegreat.MigrationTools.Repositories
{
    public class MsSqlTrackerRepository : BaseSqlTrackerRepository
    {
        public MsSqlTrackerRepository(ConnectionConfiguration connectionConfiguration) : base(connectionConfiguration)
        {
        }

        public override IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionConfiguration.ConnectionString);
        }
    }
}
