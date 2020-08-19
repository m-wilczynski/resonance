using System;
using System.Threading.Tasks;
using Resonance.Outbox.Serialization;
using Resonance.Outbox.Storage;

namespace Resonance.Outbox.Inbound
{
    public class TransactionOutboxBuilder
    {
        private IMessageRepository _repository;
        private IMessageSerializer _serializer;
        private IStorageInitializer _storageInitializer;

        public TransactionOutboxBuilder UseRepository(IMessageRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            return this;
        }

        public TransactionOutboxBuilder UseSerializer(IMessageSerializer serializer)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            return this;
        }

        public TransactionOutboxBuilder UseStorageInitializer(IStorageInitializer initializer)
        {
            _storageInitializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            return this;
        }

        public async Task<ITransactionalOutbox> Build()
        {
            if (_storageInitializer?.InitializeOnStartup == true)
            {
                await _storageInitializer.InitializeTables().ConfigureAwait(false);
            }
            return new TransactionalOutbox(_repository, _serializer);
        }
    }
}
