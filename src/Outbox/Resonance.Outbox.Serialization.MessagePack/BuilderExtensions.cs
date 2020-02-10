using Resonance.Outbox.Inbound;
using Resonance.Outbox.Outbound;

namespace Resonance.Outbox.Serialization.MessagePack
{
    public static class BuilderExtensions
    {
        public static TransactionOutboxBuilder UseMessagePack(
            this TransactionOutboxBuilder builder)
        {
            return builder.UseSerializer(new MessagePackSerializer());
        }

        public static OutboxForwarderBuilder UseMessagePack(
            this OutboxForwarderBuilder builder)
        {
            return builder.UseSerializer(new MessagePackSerializer());
        }
    }
}
