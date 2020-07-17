using System;
using System.Collections.Generic;

namespace DBMigreat.MigrationTools.Repositories
{
    public interface IMegreatTracksRepositories
    {
        bool CheckOrCreateMegreatTracksTableExist();
        IDictionary<string, ExecutedScript> GetExecutedScripts();
    }

    public class MegreatTracksRepositories : IMegreatTracksRepositories
    {
        public MegreatTracksRepositories()
        {
        }

        public bool CheckOrCreateMegreatTracksTableExist()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, ExecutedScript> GetExecutedScripts()
        {
            throw new NotImplementedException();
        }
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
