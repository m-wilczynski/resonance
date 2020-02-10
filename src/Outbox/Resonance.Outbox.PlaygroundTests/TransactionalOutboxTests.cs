using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Dapper;
using Resonance.Outbox.Inbound;
using Resonance.Outbox.Outbound;
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

            var outboxForwarder = await new OutboxForwarderBuilder()
                .UseSqlServer(connectionString)
                .UseMessagePack()
                .UseMessageForwarders(new[] {new HelloForwarder()})
                .Build();

            var exampleMessages = Enumerable.Range(123, 100).Select(_ => new ExampleMessage
            {
                Id = Guid.NewGuid(),
                Number = _,
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

            Console.WriteLine("Outbox done!");

            await outboxForwarder.ForwardUnsentMessagesFromOutbox();

            Console.WriteLine("Forwarders done!");

            Console.WriteLine("Done!");
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

    public class HelloForwarder : SingleMessageTypeForwarder<ExampleMessage>
    {
        public override Task ForwardCasted(ExampleMessage message)
        {
            Trace.WriteLine($"Hello message no. {message.Number}");
            return Task.CompletedTask;
        }
    }
}
