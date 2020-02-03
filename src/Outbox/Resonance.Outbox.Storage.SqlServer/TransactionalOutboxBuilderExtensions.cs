using System;
using System.Data;
using System.Data.SqlClient;

namespace Resonance.Outbox.Storage.SqlServer
{
    public static class TransactionalOutboxBuilderExtensions
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
    }
}
