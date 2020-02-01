using System;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using Resonance.Outbox.Serialization;
using Resonance.Outbox.Storage;

namespace Resonance.Outbox
{
    public class TransactionalOutbox : ITransactionalOutbox
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageSerializer _messageSerializer;

        public TransactionalOutbox(IMessageRepository messageRepository, IMessageSerializer messageSerializer)
        {
            _messageRepository = messageRepository;
            _messageSerializer = messageSerializer;
        }

        public async Task Send<TMessage>(TMessage message, IDbTransaction transaction = null, DateTime? sendTime = null)
        {
            if (transaction == null)
            {
                EnsureThereIsActiveAmbientTransaction();
            }

            var payload = await _messageSerializer.Serialize(message);
            var serializedMessage = new SerializedMessage(payload, message.GetType(), sendTime);
            await _messageRepository.SaveMessage(serializedMessage, transaction);
        }

        private void EnsureThereIsActiveAmbientTransaction()
        {
            try
            {
                if (Transaction.Current == null)
                {
                    throw new NoActiveAmbientTransactionException();
                }
            }
            catch
            {
                throw new NoActiveAmbientTransactionException();
            }
        }
    }

    public class NoActiveAmbientTransactionException : Exception
    {
        public NoActiveAmbientTransactionException() : base("There is no active, ambient transaction and no IDbTransaction was used!")
        {
        }
    }
}
