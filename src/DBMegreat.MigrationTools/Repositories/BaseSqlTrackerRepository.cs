using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace DBMegreat.MigrationTools.Repositories
{

    public abstract class BaseSqlTrackerRepository : ITrackerRepository
    {
        private protected readonly ConnectionConfiguration _connectionConfiguration;

        public BaseSqlTrackerRepository(ConnectionConfiguration connectionConfiguration)
        {
            _connectionConfiguration = connectionConfiguration;
        }

        public abstract IDbConnection CreateConnection();

        public virtual string CommandCheckTracksTableExist
        {
            get
            {
                return @"SELECT table_name
                            FROM information_schema.tables
                            WHERE table_name = 'db_megreat_track'";
            }
        }
        public virtual string CommandCreateTracksTable
        {
            get
            {
                return @"CREATE TABLE db_megreat_track (
                            tracking_key varchar(2048) NOT NULL,
                            executed_time date DEFAULT NULL
                            PRIMARY KEY (tracking_key)
                        )";
            }
        }
        public virtual string CommandInsertTrackRecord
        {
            get
            {
                return @"INSERT INTO db_megreat_track(tracking_key, executed_time) 
                            VALUES(@TrackingKey, GETDATE())";
            }
        }
        public virtual string CommandSelectTrackingRecords
        {
            get
            {
                return "SELECT tracking_key, executed_time FROM db_megreat_track";
            }
        }

        public async Task<bool> CheckTrackTableExistAsync()
        {
            string tableName;
            try
            {
                tableName = await QueryFirstAsync<string>(CommandCheckTracksTableExist);
                return !string.IsNullOrWhiteSpace(tableName);
            }
            catch (Exception ex)
            {
                throw new DBException($"Failure on checking 'db_megreat_track' table: {ex.Message}.", ex);
            }
        }

        public async Task CreateTrackTableAsync()
        {
            try
            {
                await ExecuteNonQueryAsync(CommandCreateTracksTable);

            }
            catch (Exception ex)
            {
                throw new DBException($"Failure on creating 'db_megreat_track' table: {ex.Message}.", ex);
            }
        }

        public async Task<IDictionary<string, ExecutedScript>> GetExecutedScriptsAsync()
        {
            try
            {
                return (await QueryAsync<dynamic>(CommandCreateTracksTable))
                    .Select(r => new ExecutedScript(r.key, r.executed_time))
                    .ToDictionary(r => r.Key);

            }
            catch (Exception ex)
            {
                throw new DBException($"Failure on getting 'db_megreat_track' records.", ex);
            }
        }

        public async Task InsertTrackingAsync(string key)
        {
            try
            {
                await ExecuteNonQueryAsync(CommandInsertTrackRecord, new { TrackingKey = key });
            }
            catch (Exception ex)
            {
                throw new DBException($"Failure on inserting new track record for {key}: {ex.Message}", ex);
            }
        }

        public async Task<T> QueryFirstAsync<T>(string query, object parameters = null)
        {
            T result;
            using (var con = CreateConnection())
            {
                con.Open();
                result = await con.QueryFirstAsync<T>(query, parameters);
            }
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters = null)
        {
            IEnumerable<T> result;
            using (var con = CreateConnection())
            {
                con.Open();
                result = await con.QueryAsync<T>(query, parameters);
            }
            return result;
        }

        public async Task<int> ExecuteNonQueryAsync(string nonQuerySql, object param = null)
        {
            using var con = CreateConnection();
            con.Open();
            return await con.ExecuteAsync(nonQuerySql, param);
        }
    }
}
