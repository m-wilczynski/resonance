using System;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Resonance.Outbox.Serialization.Utf8Json
{
    public class Utf8JsonSerializer : IMessageSerializer
    {
        private IJsonFormatterResolver _resolver = StandardResolver.AllowPrivate;

        public byte[] Serialize<TMessage>(TMessage message)
        {
            //TODO: Compare with performance using MemoryStream && SerializeAsync
            return JsonSerializer.Serialize(message, _resolver);
        }

        public object Deserialize(Type messageType, byte[] message)
        {
            return JsonSerializer.NonGeneric.Deserialize(messageType, message, _resolver);
        }
    }
}
