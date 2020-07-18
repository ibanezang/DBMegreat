using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBMegreat.MigrationTools.Repositories
{
    public interface IMegreatTracksRepository
    {
        Task<bool> CheckOrCreateMegreatTracksTableExistAsync();
        Task<IDictionary<string, ExecutedScript>> GetExecutedScriptsAsync();
        Task ExecuteNonQueryAsync(string nonQuerySql);
    }

    public class ExecutedScript
    {
        public string Key { get; }
        public DateTime ExecutedTime { get; }

        public ExecutedScript(string key, DateTime executedTime)
        {
            Key = key;
            ExecutedTime = executedTime;
        }
    }
}
