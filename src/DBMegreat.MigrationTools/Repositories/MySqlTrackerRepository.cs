using System.Data;
using MySql.Data.MySqlClient;

namespace DBMegreat.MigrationTools.Repositories
{
    public class MySqlTrackerRepository : BaseSqlTrackerRepository
    {
        public override string CommandInsertTrackRecord
        {
            get
            {
                return @"INSERT INTO db_megreat_track(tracking_key, executed_time) 
                            VALUES(@TrackingKey, CURRENT_TIMESTAMP)";
            }
        }

        public MySqlTrackerRepository(ConnectionConfiguration connectionConfiguration) : base(connectionConfiguration)
        {
        }

        public override IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionConfiguration.ConnectionString);
        }
    }
}
