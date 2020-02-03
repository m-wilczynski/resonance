namespace Resonance.Outbox.Serialization.Utf8Json
{
    public static class TransactionalOutboxBuilderExtensions
    {
        public static TransactionOutboxBuilder UseUtf8Json(
            this TransactionOutboxBuilder builder)
        {
            return builder.UseSerializer(new Utf8JsonSerializer());
        }
    }
}
