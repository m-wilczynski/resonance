using System;
using MessagePack;
using MessagePack.Resolvers;

namespace Resonance.Outbox.Serialization.MessagePack
{
    public class MessagePackSerializer : IMessageSerializer
    {
        private readonly MessagePackSerializerOptions _serializerOptions = 
            MessagePackSerializerOptions.Standard
                .WithCompression(MessagePackCompression.Lz4Block)
                .WithAllowAssemblyVersionMismatch(true)
                .WithResolver(DynamicContractlessObjectResolverAllowPrivate.Instance);

        public byte[] Serialize<TMessage>(TMessage message)
        {
            return global::MessagePack.MessagePackSerializer.Serialize(message, _serializerOptions);
        }

        public object Deserialize(Type messageType, byte[] message)
        {
            return global::MessagePack.MessagePackSerializer.Deserialize(messageType, message, _serializerOptions);
        }
    }
}
