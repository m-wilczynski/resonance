namespace Resonance.Outbox.Serialization.MessagePack
{
    public static class TransactionalOutboxBuilderExtensions
    {
        public static TransactionOutboxBuilder UseMessagePack(
            this TransactionOutboxBuilder builder)
        {
            return builder.UseSerializer(new MessagePackSerializer());
        }
    }
}
