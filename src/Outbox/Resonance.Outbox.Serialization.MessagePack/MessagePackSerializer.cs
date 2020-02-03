using System;

namespace Resonance.Outbox.Serialization.MessagePack
{
    public class MessagePackSerializer : IMessageSerializer
    {
        public byte[] Serialize<TMessage>(TMessage message)
        {
            return global::MessagePack.MessagePackSerializer.Serialize(message);
        }

        public object Deserialize(Type messageType, byte[] message)
        {
            return global::MessagePack.MessagePackSerializer.Deserialize(messageType, message);
        }
    }
}
