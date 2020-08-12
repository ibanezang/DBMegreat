using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBMegreat.MigrationTools.Repositories
{
    public interface ITrackerRepository
    {
        Task<bool> CheckTrackTableExistAsync();
        Task CreateTrackTableAsync();
        Task<IDictionary<string, SchemaHistoryRecord>> GetSchemaHistoryRecordsAsync();
        Task InsertTrackingAsync(string key);
        Task<int> ExecuteNonQueryAsync(string nonQuerySql, object parameters = null);
        Task<T> QueryFirstAsync<T>(string query, object parameters = null);
        Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters = null);
    }


    public class SchemaHistoryRecord
    {
        public string Key { get; }
        public DateTime ExecutedTime { get; }

        public SchemaHistoryRecord(string key, DateTime executedTime)
        {
            Key = key;
            ExecutedTime = executedTime;
        }
    }

    public class DBException : Exception
    {
        public DBException(string message) : base(message) { }
        public DBException(string message, Exception inner) : base(message, inner) { }
    }
}
