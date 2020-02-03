using System;
using Utf8Json;

namespace Resonance.Outbox.Serialization.Utf8Json
{
    public class Utf8JsonSerializer : IMessageSerializer
    {
        public byte[] Serialize<TMessage>(TMessage message)
        {
            //TODO: Compare with performance using MemoryStream && SerializeAsync
            return JsonSerializer.Serialize(message);
        }

        public object Deserialize(Type messageType, byte[] message)
        {
            return JsonSerializer.NonGeneric.Deserialize(messageType, message);
        }
    }
}
