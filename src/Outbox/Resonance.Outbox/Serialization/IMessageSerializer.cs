using System;

namespace Resonance.Outbox.Serialization
{
    public interface IMessageSerializer
    {
        //TODO: Add API to write to Stream directly?
        byte[] Serialize<TMessage>(TMessage message);
        object Deserialize(Type messageType, byte[] message);
    }
}
