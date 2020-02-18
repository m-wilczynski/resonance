using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Resonance.Outbox.Outbound;
using Resonance.Outbox.Serialization;

namespace Resonance.Outbox.Storage.SqlServer
{
    public class SqlServerStorageInitializer : IStorageInitializer
    {
        private Func<IDbConnection> _connectionFactory;
        private readonly StorageConfiguration _storageConfiguration;

        public SqlServerStorageInitializer(
            Func<IDbConnection> connectionFactory,
            StorageConfiguration storageConfiguration)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _storageConfiguration = storageConfiguration ?? throw new ArgumentNullException(nameof(storageConfiguration));
        }

        public bool InitializeOnStartup => _storageConfiguration.InitializeTablesOnStartup;

        public async Task InitializeTables()
        {
            if (!_storageConfiguration.IsComplete)
            {
                throw new StorageConfigurationIncompleteException();
            }

            using (var connection = _connectionFactory())
            {
                string sql = $@"
                    IF (NOT EXISTS (SELECT * FROM sys.schemas WHERE name = '{_storageConfiguration.SchemaName}')) 
                    BEGIN
                        EXEC ('CREATE SCHEMA [{_storageConfiguration.SchemaName}] AUTHORIZATION [dbo]');
                    END;

                    IF NOT EXISTS 
                    (
	                    SELECT 1 
	                    FROM sys.tables tables 
	                    JOIN sys.schemas schemas 
	                    ON (tables.schema_id = schemas.schema_id) 
	                    WHERE schemas.name = '{_storageConfiguration.SchemaName}' and tables.name = '{_storageConfiguration.MessageTableName}'
                    )
                    CREATE TABLE {_storageConfiguration.SchemaName}.{_storageConfiguration.MessageTableName}
                    (
	                    Id INT IDENTITY(1,1) PRIMARY KEY,
	                    {nameof(SerializedMessage.Payload)} VARBINARY(MAX) NOT NULL,
	                    {nameof(SerializedMessage.MessageTypeAssemblyQualifiedName)} NVARCHAR(500) NOT NULL,
	                    {nameof(SerializedMessage.ReceiveDateUtc)} DATETIME NOT NULL,
	                    {nameof(SerializedMessage.SuccessfulForwardDateUtc)} DATETIME NULL,
                    );

                    IF NOT EXISTS
                    (
                        SELECT 1 
                        FROM sys.indexes 
                        WHERE name = 'IX_{nameof(SerializedMessage.SuccessfulForwardDateUtc)}' 
                        AND object_id = OBJECT_ID('{_storageConfiguration.SchemaName}.{_storageConfiguration.MessageTableName}')
                    )
                    CREATE NONCLUSTERED INDEX IX_{nameof(SerializedMessage.SuccessfulForwardDateUtc)} 
                    ON {_storageConfiguration.SchemaName}.{_storageConfiguration.MessageTableName}
                    (
	                    {nameof(SerializedMessage.SuccessfulForwardDateUtc)} ASC
                    );

                    IF NOT EXISTS 
                    (
	                    SELECT 1 
	                    FROM sys.tables tables 
	                    JOIN sys.schemas schemas 
	                    ON (tables.schema_id = schemas.schema_id) 
	                    WHERE schemas.name = '{_storageConfiguration.SchemaName}' and tables.name = '{_storageConfiguration.LogTableName}'
                    )
                    CREATE TABLE {_storageConfiguration.SchemaName}.{_storageConfiguration.LogTableName}
                    (
	                    Id INT IDENTITY(1,1) PRIMARY KEY,
	                    {nameof(ForwardLogEntry.CompletionDateUtc)} DATETIME NOT NULL,
	                    {nameof(ForwardLogEntry.Success)} BIT NOT NULL
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
