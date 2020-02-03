using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Dapper;
using Resonance.Outbox.Serialization;
using Resonance.Outbox.Serialization.MessagePack;
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

            var exampleMessage = new ExampleMessage
            {
                Id = Guid.NewGuid(),
                Number = new Random().Next(200000),
                Text = "Text-" + Guid.NewGuid() + Guid.NewGuid() + Guid.NewGuid(),
                LargeArray = Enumerable.Range(0, 1000).ToArray()
            };

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    //await connection.ExecuteAsync(...);
                    //await connection.QueryAsync(...);

                    await outbox.Send(exampleMessage,
                        transaction);

                    transaction.Commit();
                }
            }

            using (var connection = new SqlConnection(connectionString))
            {
                var message =
                    (await connection.QueryAsync<SerializedMessage>(@"
                        SELECT TOP 1
                            Payload, 
                            MessageTypeAssemblyQualifiedName 
                        FROM dbo.messages
                        ORDER BY Id DESC"))
                    .SingleOrDefault();

                var type = Type.GetType(message.MessageTypeAssemblyQualifiedName);
                var result = (ExampleMessage) new MessagePackSerializer().Deserialize(type, message.Payload);

                Assert.Equal(result.Text, exampleMessage.Text);
                Assert.Equal(result.Number, exampleMessage.Number);
                Assert.True(result.LargeArray.Intersect(exampleMessage.LargeArray).Count() == exampleMessage.LargeArray.Length);
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
