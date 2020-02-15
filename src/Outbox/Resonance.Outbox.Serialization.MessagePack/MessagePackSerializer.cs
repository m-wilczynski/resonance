using System;
using MessagePack;
using MessagePack.Resolvers;

namespace Resonance.Outbox.Serialization.MessagePack
{
    public class MessagePackSerializer : IMessageSerializer
    {
        static MessagePackSerializer()
        {
            CompositeResolver.RegisterAndSetAsDefault(StandardResolverAllowPrivate.Instance,
                BuiltinResolver.Instance,
                AttributeFormatterResolver.Instance,
                // replace enum resolver
                DynamicEnumAsStringResolver.Instance,
                DynamicGenericResolver.Instance,
                DynamicUnionResolver.Instance,
                DynamicObjectResolver.Instance,
                PrimitiveObjectResolver.Instance,
                // final fallback(last priority)
                StandardResolver.Instance);
        }

        public byte[] Serialize<TMessage>(TMessage message)
        {
            return global::MessagePack.MessagePackSerializer.NonGeneric.Serialize(message.GetType(), message);
        }

        public object Deserialize(Type messageType, byte[] message)
        {
            return global::MessagePack.MessagePackSerializer.NonGeneric.Deserialize(messageType, message);
        }
    }
}
