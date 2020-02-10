using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Resonance.Outbox.Serialization;
using Resonance.Outbox.Storage;

namespace Resonance.Outbox.Outbound
{
    public class OutboxForwarder : IOutboxForwarder
    {
        private readonly ConcurrentDictionary<Type, List<IMessageForwarder>> _cachedMessageForwarders = new ConcurrentDictionary<Type, List<IMessageForwarder>>();

        private ICollection<IMessageForwarder> _messageForwarders;
        private IMessageRepository _messageRepository;
        private Func<IDbConnection> _connectionFactory;
        private ForwardingOptions _forwardingOptions;
        private IMessageSerializer _messageSerializer;

        public OutboxForwarder(
            IMessageRepository messageRepository,
            ForwardingOptions forwardingOptions, 
            Func<IDbConnection> connectionFactory, 
            IMessageSerializer messageSerializer, 
            ICollection<IMessageForwarder> messageForwarders)
        {
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
            _forwardingOptions = forwardingOptions ?? throw new ArgumentNullException(nameof(forwardingOptions));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
            _messageForwarders = messageForwarders ?? throw new ArgumentNullException(nameof(messageForwarders));
            if (_messageForwarders.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(messageForwarders));
            }
        }

        public async Task ForwardUnsentMessagesFromOutbox()
        {
            using (var connection = _connectionFactory())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead))
                {
                    var forwardStart = DateTime.UtcNow;
                    try
                    {
                        var messagesToForward = _forwardingOptions.ReadMode == ReadMode.ReadThenMarkAsRead
                            ? await _messageRepository.GetMessagesAsMarkedSent(transaction,
                                _forwardingOptions.BatchSize)
                            : await _messageRepository.GetMessagesAsRemoved(transaction, _forwardingOptions.BatchSize);

                        //TODO: Introduce parallelism (ie. Parallel.ForEach) ?
                        foreach (var message in messagesToForward)
                        {
                            //TODO: Cache resolved types
                            var messageType = Type.GetType(message.MessageTypeAssemblyQualifiedName);
                            if (messageType == null)
                            {
                                throw new UnknownMessageTypeException(message.MessageTypeAssemblyQualifiedName);
                            }

                            var deserializedMessage = _messageSerializer.Deserialize(messageType, message.Payload);

                            //Alternative - introduce marker interface for 'outbox-able' messages and prepare such map on startup?
                            if (!_cachedMessageForwarders.TryGetValue(messageType, out var forwarders))
                            {
                                forwarders = _messageForwarders
                                    .Where(forwarder => forwarder.CanForwardMessage(messageType)).ToList();
                                _cachedMessageForwarders.TryAdd(messageType, forwarders);
                            }

                            if (forwarders == null || forwarders.Count == 0)
                            {
                                throw new NoForwarderForMessageTypeException(messageType);
                            }

                            foreach (var forwarder in forwarders)
                            {
                                await forwarder.Forward(deserializedMessage);
                            }
                        }

                        if (_forwardingOptions.LogCompletionInsideTransaction)
                        {
                            await LogForwardSuccess(forwardStart, DateTime.UtcNow);
                        }

                        transaction.Commit();

                        if (!_forwardingOptions.LogCompletionInsideTransaction)
                        {
                            await LogForwardSuccess(forwardStart, DateTime.UtcNow);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        await LogForwardFailure(ex, forwardStart, DateTime.UtcNow);
                    }
                }
            }


        }

        private async Task LogForwardSuccess(DateTime forwardStartUtc, DateTime forwardEndUtc)
        {
            //TODO
        }

        private async Task LogForwardFailure(Exception ex, DateTime forwardStartUtc, DateTime forwardEndUtc)
        {
            //TODO
        }
    }

    public class UnknownMessageTypeException : Exception
    {
        public UnknownMessageTypeException(string typeAssemblyFullyQualifiedName) 
            : base($"Could not find message type based on its fully qualified name: {typeAssemblyFullyQualifiedName}!")
        {
        }
    }

    public class NoForwarderForMessageTypeException : Exception
    {
        public NoForwarderForMessageTypeException(Type type)
            : base($"Could not find {nameof(IMessageForwarder)} for type {type.AssemblyQualifiedName}!")
        {
        }
    }
}
