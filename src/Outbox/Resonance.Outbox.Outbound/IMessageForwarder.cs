using System;
using System.Threading.Tasks;

namespace Resonance.Outbox.Outbound
{
    public interface IMessageForwarder
    {
        bool CanForwardMessage(Type messageType);
        Task Forward(object message);
    }
}
