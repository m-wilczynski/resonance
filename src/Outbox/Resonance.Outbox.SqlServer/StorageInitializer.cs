using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Resonance.Outbox.Storage;

namespace Resonance.Outbox.SqlServer
{
    public class StorageInitializer : IStorageInitializer
    {
        private Func<IDbConnection> _connectionFactory;
        private readonly StorageConfiguration _storageConfiguration;

        public StorageInitializer(
            Func<IDbConnection> connectionFactory,
            StorageConfiguration storageConfiguration)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _storageConfiguration = storageConfiguration ?? throw new ArgumentNullException(nameof(storageConfiguration));
        }

        public async Task InitializeTables()
        {
            if (!_storageConfiguration.IsComplete)
            {
                throw new StorageConfigurationIncompleteException();
            }

            using (var connection = _connectionFactory())
            {
                string sql = $@"
                    IF NOT EXISTS 
                    (
	                    SELECT 1 
	                    FROM sys.tables tables 
	                    JOIN sys.schemas schemas 
	                    ON (tables.schema_id = schemas.schema_id) 
	                    WHERE schemas.name = '{_storageConfiguration.SchemaName}' and tables.name = '{_storageConfiguration.MessageTableName}'
                    )
                    CREATE TABLE dbo.messages
                    (
	                    Id INT IDENTITY(1,1) PRIMARY KEY,
	                    Payload VARBINARY(MAX) NOT NULL,
	                    MessageTypeAssemblyQualifiedName NVARCHAR(500) NOT NULL,
	                    SendTimeUtc DATETIME NOT NULL
                    );

                    IF NOT EXISTS 
                    (
	                    SELECT 1 
	                    FROM sys.tables tables 
	                    JOIN sys.schemas schemas 
	                    ON (tables.schema_id = schemas.schema_id) 
	                    WHERE schemas.name = '{_storageConfiguration.SchemaName}' and tables.name = '{_storageConfiguration.LogTableName}'
                    )
                    CREATE TABLE dbo.logs
                    (
	                    Id INT IDENTITY(1,1) PRIMARY KEY,
	                    CompletionDate DATETIME NOT NULL,
	                    Success BIT NOT NULL
                    );";

                await connection.ExecuteAsync(sql,
                    param: _storageConfiguration,
                    commandTimeout: _storageConfiguration.OperationTimeoutInSeconds);
            }
        }
    }

    public class StorageConfigurationIncompleteException : Exception
    {
        public StorageConfigurationIncompleteException() : base($"{nameof(StorageConfiguration)} values cannot be null/empty!")
        {
        }
    }
}
