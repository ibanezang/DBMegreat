using System;

namespace DBMegreat.MigrationTools.Repositories
{
    public interface ITrackerRepositoryFactory
    {
        ITrackerRepository GetTrackerRepository(ConnectionConfiguration connectionConfiguration);
    }

    public class MegreatTracksRepositoryFactory : ITrackerRepositoryFactory
    {
        public ITrackerRepository GetTrackerRepository(ConnectionConfiguration connectionConfiguration)
        {
            return connectionConfiguration.Type switch
            {
                SqlType.MySql => new MySqlTrackerRepository(connectionConfiguration),
                SqlType.SqlServer => new MsSqlTrackerRepository(connectionConfiguration),
                _ => throw new TrackerRepositoryException($"Not supported SqlType {connectionConfiguration.Type}"),
            };
        }
    }

    public class TrackerRepositoryException : Exception
    {
        public TrackerRepositoryException(string message) : base(message) { }
    }
}
