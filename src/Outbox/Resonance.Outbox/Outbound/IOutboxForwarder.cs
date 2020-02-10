using System.Threading.Tasks;

namespace Resonance.Outbox.Outbound
{
    /// <summary>
    /// API for pushing/forwarding messages that were sent to outbox (outbound traffic from outbox)
    /// </summary>
    public interface IOutboxForwarder
    {
        /// <summary>
        /// Forward/push out unsent messages from outbox
        /// </summary>
        /// <returns></returns>
        Task ForwardUnsentMessagesFromOutbox();
    }
}
