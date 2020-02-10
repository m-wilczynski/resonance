using System;
using System.Data;
using System.Data.SqlClient;
using Resonance.Outbox.Inbound;
using Resonance.Outbox.Outbound;

namespace Resonance.Outbox.Storage.SqlServer
{
    public static class BuilderExtensions
    {
        public static TransactionOutboxBuilder UseSqlServer(
            this TransactionOutboxBuilder builder,
            string connectionString,
            StorageConfiguration storageConfiguration = null)
        {
            Func<IDbConnection> connectionFactory = () => new SqlConnection(connectionString);

            if (storageConfiguration == null)
            {
                storageConfiguration = new StorageConfiguration();
            }

            return builder
                .UseRepository(new SqlServerMessageRepository(connectionFactory, storageConfiguration))
                .UseStorageInitializer(new SqlServerStorageInitializer(connectionFactory, storageConfiguration));
        }

        public static OutboxForwarderBuilder UseSqlServer(
            this OutboxForwarderBuilder builder,
            string connectionString,
            StorageConfiguration storageConfiguration = null)
        {
            Func<IDbConnection> connectionFactory = () => new SqlConnection(connectionString);

            if (storageConfiguration == null)
            {
                storageConfiguration = new StorageConfiguration();
            }

            return builder
                .UseConnectionFactory(connectionFactory)
                .UseRepository(new SqlServerMessageRepository(connectionFactory, storageConfiguration))
                .UseStorageInitializer(new SqlServerStorageInitializer(connectionFactory, storageConfiguration));
        }
    }
}
