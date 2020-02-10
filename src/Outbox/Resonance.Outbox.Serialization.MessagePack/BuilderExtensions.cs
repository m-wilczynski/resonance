namespace Resonance.Outbox.Serialization.MessagePack
{
    public static class BuilderExtensions
    {
        public static TransactionOutboxBuilder UseMessagePack(
            this TransactionOutboxBuilder builder)
        {
            return builder.UseSerializer(new MessagePackSerializer());
        }
    }
}
