using System.Threading.Tasks;

namespace Resonance.Outbox.Outbound
{
    public interface IOutboxForwarder
    {
        Task ForwardMessagesFromOutbox();
    }
}
