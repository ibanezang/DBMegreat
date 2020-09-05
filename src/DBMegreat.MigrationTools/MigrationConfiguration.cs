using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace DBMegreat.MigrationTools
{
    public class MigrationConfiguration
    {
        [JsonPropertyName("sql_files_directories")]
        public IReadOnlyList<string> SqlFilesDirectories { get; set; }

        [JsonPropertyName("db_connection")]
        public ConnectionConfiguration DbConnection { get; set; }

        [JsonPropertyName("log_output_directory")]
        public string LogOutputDirectory { get; set; }

        public static MigrationConfiguration ParseConfiguration(string jsonContent)
        {
            MigrationConfiguration config;
            try
            {
                config = JsonSerializer.Deserialize<MigrationConfiguration>(jsonContent, new JsonSerializerOptions());
            }
            catch (Exception ex)
            {
                throw new InvalidConfigurationException("Invalid configuration file.", ex);
            }

            if (config.DbConnection == null)
            {
                throw new InvalidConfigurationException("db_connection configuration was not found.");
            }

            if (string.IsNullOrWhiteSpace(config.DbConnection.ConnectionString))
            {
                throw new InvalidConfigurationException("db_connection.connection_string configuration was not found or has invalid value.");
            }

            if (config.SqlFilesDirectories == null)
            {
                throw new InvalidConfigurationException("sql_files_directories configuration was not found.");
            }

            if (config.SqlFilesDirectories.Count == 0)
            {
                throw new InvalidConfigurationException("sql_files_directories configuration was empty.");
            }

            foreach (var directory in config.SqlFilesDirectories)
            {
                if (string.IsNullOrWhiteSpace(directory))
                {
                    throw new InvalidConfigurationException("sql_files_directories has invalid value.");
                }
            }

            return config;
        }
    }

    public class ConnectionConfiguration
    {
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SqlType Type { get; set; }

        [JsonPropertyName("connection_string")]
        public string ConnectionString { get; set; }
    }

    public enum SqlType
    {
        MySql,
        SqlServer
    }

    public class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException(string message) : base(message) { }
        public InvalidConfigurationException(string message, Exception inner) : base(message, inner) { }
    }
}

