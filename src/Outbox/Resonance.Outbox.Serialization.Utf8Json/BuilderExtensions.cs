using Resonance.Outbox.Inbound;
using Resonance.Outbox.Outbound;

namespace Resonance.Outbox.Serialization.Utf8Json
{
    public static class BuilderExtensions
    {
        public static TransactionOutboxBuilder UseUtf8Json(
            this TransactionOutboxBuilder builder)
        {
            return builder.UseSerializer(new Utf8JsonSerializer());
        }

        public static OutboxForwarderBuilder UseUtf8Json(
            this OutboxForwarderBuilder builder)
        {
            return builder.UseSerializer(new Utf8JsonSerializer());
        }
    }
}
