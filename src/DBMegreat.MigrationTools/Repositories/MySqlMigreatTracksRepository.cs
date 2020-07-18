using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBMegreat.MigrationTools.Repositories
{
    public class MySqlMigreatTracksRepository : IMegreatTracksRepository
    {
        public MySqlMigreatTracksRepository()
        {
        }

        public Task<bool> CheckOrCreateMegreatTracksTableExistAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, ExecutedScript>> GetExecutedScriptsAsync()
        {
            throw new NotImplementedException();
        }

        public Task ExecuteNonQueryAsync(string nonQuerySql)
        {
            throw new NotImplementedException();
        }
    }
}
