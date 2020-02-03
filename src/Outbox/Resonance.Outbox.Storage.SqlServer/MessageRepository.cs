using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Resonance.Outbox.Serialization;

namespace Resonance.Outbox.Storage.SqlServer
{
    public class MessageRepository : IMessageRepository
    {
        private Func<IDbConnection> _connectionFactory;
        private StorageConfiguration _storageConfiguration;

        public MessageRepository(
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
                        @{nameof(serializedMessage.Payload)},
                        @{nameof(serializedMessage.MessageTypeAssemblyQualifiedName)},
                        @{nameof(serializedMessage.SendTimeUtc)}
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

        public Task<ICollection<SerializedMessage>> GetMessagesAsMarkedSent(IDbTransaction transaction, uint? howManyMessages = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<ICollection<SerializedMessage>> GetMessagesAsRemoved(IDbTransaction transaction, uint? howManyMessages = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
