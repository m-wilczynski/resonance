using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Resonance.Outbox.Serialization;
using Resonance.Outbox.Storage;

namespace Resonance.Outbox.Outbound
{
    public class OutboxForwarderBuilder
    {
        private ICollection<IMessageForwarder> _messageForwarders;
        private IMessageRepository _messageRepository;
        private Func<IDbConnection> _connectionFactory;
        private ForwardingOptions _forwardingOptions;
        private IMessageSerializer _messageSerializer;
        private IStorageInitializer _storageInitializer;

        public OutboxForwarderBuilder UseRepository(IMessageRepository repository)
        {
            _messageRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            return this;
        }

        public OutboxForwarderBuilder UseSerializer(IMessageSerializer serializer)
        {
            _messageSerializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            return this;
        }

        public OutboxForwarderBuilder UseConnectionFactory(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            return this;
        }

        public OutboxForwarderBuilder UseForwardingOptions(ForwardingOptions options)
        {
            _forwardingOptions = options ?? throw new ArgumentNullException(nameof(options));
            return this;
        }

        public OutboxForwarderBuilder UseMessageForwarders(ICollection<IMessageForwarder> messageForwarders)
        {
            _messageForwarders = messageForwarders ?? throw new ArgumentNullException(nameof(messageForwarders));
            return this;
        }

        public OutboxForwarderBuilder UseStorageInitializer(IStorageInitializer initializer)
        {
            _storageInitializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            return this;
        }

        public async Task<IOutboxForwarder> Build()
        {
            if (_storageInitializer?.InitializeOnStartup == true)
            {
                await _storageInitializer.InitializeTables();
            }

            return new OutboxForwarder(
                _messageRepository, 
                _forwardingOptions ?? new ForwardingOptions(ReadMode.ReadThenMarkAsRead), 
                _connectionFactory, 
                _messageSerializer,
                _messageForwarders);
        }
    }
}
