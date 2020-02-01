using System.Threading.Tasks;

namespace Resonance.Outbox.Serialization
{
    public interface IMessageSerializer
    {
        Task<byte[]> Serialize<TMessage>(TMessage message);
        Task<TMessage> Deserialize<TMessage>(byte[] message);
    }
}
