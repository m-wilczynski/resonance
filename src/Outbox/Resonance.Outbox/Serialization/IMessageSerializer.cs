using System;
using System.Threading.Tasks;

namespace Resonance.Outbox.Serialization
{
    public interface IMessageSerializer
    {
        Task<byte[]> Serialize<TMessage>(TMessage message);
        Task<object> Deserialize(Type messageType, byte[] message);
    }
}
