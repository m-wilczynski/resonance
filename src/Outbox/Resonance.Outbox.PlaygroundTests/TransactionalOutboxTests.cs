using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Dapper;
using Resonance.Outbox.Serialization;
using Resonance.Outbox.Serialization.MessagePack;
using Resonance.Outbox.Storage;
using Resonance.Outbox.Storage.SqlServer;
using Xunit;

namespace Resonance.Outbox.PlaygroundTests
{
    public class TransactionalOutboxTests
    {
        private static readonly string connectionString =
            @"Server=localhost\MSSQLSERVER01;Database=test;Integrated Security=true;";

        [Fact]
        public async Task SaveAndRetrieve()
        {
            var outbox = await new TransactionOutboxBuilder()
                .UseSqlServer(connectionString)
                .UseMessagePack()
                .Build();

            var exampleMessages = Enumerable.Range(0, 10000).Select(_ => new ExampleMessage
            {
                Id = Guid.NewGuid(),
                Number = new Random().Next(200000),
                Text = "Text-" + Guid.NewGuid() + Guid.NewGuid() + Guid.NewGuid(),
                LargeArray = Enumerable.Range(0, 1000).ToArray()
            });

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    //await connection.ExecuteAsync(...);
                    //await connection.QueryAsync(...);

                    foreach (var message in exampleMessages)
                    {
                        await outbox.Send(message,
                            transaction);
                    }

                    transaction.Commit();
                }
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {

                    var messages = (await new SqlServerMessageRepository(() => new SqlConnection(connectionString),
                            new StorageConfiguration())
                        .GetMessagesAsMarkedSent(transaction))
                        .Select(message =>
                        {
                            var type = Type.GetType(message.MessageTypeAssemblyQualifiedName);
                            return (ExampleMessage)new MessagePackSerializer().Deserialize(type, message.Payload);
                        })
                        .ToList();

                    //Assert.Equal(result.Text, exampleMessage.Text);
                    //Assert.Equal(result.Number, exampleMessage.Number);
                    //Assert.True(result.LargeArray.Intersect(exampleMessage.LargeArray).Count() == exampleMessage.LargeArray.Length);

                    transaction.Commit();
                }
            }
        }
    }

    [DataContract]
    public class ExampleMessage
    {
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public int[] LargeArray { get; set; }
    }
}
