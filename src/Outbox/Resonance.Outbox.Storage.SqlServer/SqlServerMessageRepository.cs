using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Resonance.Outbox.Serialization;

namespace Resonance.Outbox.Storage.SqlServer
{
    public class SqlServerMessageRepository : IMessageRepository
    {
        private Func<IDbConnection> _connectionFactory;
        private StorageConfiguration _storageConfiguration;

        public SqlServerMessageRepository(
            Func<IDbConnection> connectionFactory, 
            StorageConfiguration storageConfiguration)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _storageConfiguration = storageConfiguration ?? throw new ArgumentNullException(nameof(storageConfiguration));
        }
        
        public async Task SaveMessage(SerializedMessage serializedMessage, IDbTransaction transaction = null)
        {
            string sql =
                $@"
                    INSERT INTO {_storageConfiguration.SchemaName}.{_storageConfiguration.MessageTableName}
                    VALUES 
                    (
                        @{nameof(SerializedMessage.Payload)},
                        @{nameof(SerializedMessage.MessageTypeAssemblyQualifiedName)},
                        @{nameof(SerializedMessage.ReceiveDateUtc)},
                        @{nameof(SerializedMessage.SuccessfulForwardDateUtc)}
                    )";

            if (transaction == null)
            {
                using (var connection = _connectionFactory())
                {
                    await connection.ExecuteAsync(sql, serializedMessage,
                        commandTimeout: _storageConfiguration.OperationTimeoutInSeconds);
                }
            }
            else
            {
                await transaction.Connection.ExecuteAsync(sql, 
                    param: serializedMessage,
                    commandTimeout: _storageConfiguration.OperationTimeoutInSeconds,
                    transaction: transaction);
            }
        }

        public async Task<ICollection<SerializedMessage>> GetMessagesAsMarkedSent(IDbTransaction transaction, uint? howManyMessages = null)
        {
            string batchSizeSql = howManyMessages.HasValue && howManyMessages.Value > 0
                ? $"TOP {howManyMessages}"
                : "TOP 100 PERCENT";

            string sql = $@"
                DECLARE @date DATETIME = GETDATE();

                UPDATE msg
                SET {nameof(SerializedMessage.SuccessfulForwardDateUtc)} = @date
                OUTPUT 
                    inserted.{nameof(SerializedMessage.Payload)}, 
                    inserted.{nameof(SerializedMessage.MessageTypeAssemblyQualifiedName)}
                FROM (SELECT {batchSizeSql} * FROM {_storageConfiguration.SchemaName}.{_storageConfiguration.MessageTableName}) msg
                WHERE {nameof(SerializedMessage.SuccessfulForwardDateUtc)} IS NULL;";

            if (transaction == null)
            {
                using (var connection = _connectionFactory())
                {
                    return (await connection.QueryAsync<SerializedMessage>(sql,
                        param: null,
                        commandTimeout: _storageConfiguration.OperationTimeoutInSeconds)
                    ).ToList();
                }
            }
            else
            {
                return (await transaction.Connection.QueryAsync<SerializedMessage>(sql,
                        param: null,
                        commandTimeout: _storageConfiguration.OperationTimeoutInSeconds,
                        transaction: transaction)).ToList();
            }

        }

        public Task<ICollection<SerializedMessage>> GetMessagesAsRemoved(IDbTransaction transaction, uint? howManyMessages = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
