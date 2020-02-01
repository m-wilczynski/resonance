using System;
using System.Threading.Tasks;

namespace Resonance.Outbox.Outbound
{
    public abstract class SingleMessageTypeForwarder<TMessage> : IMessageForwarder
    {
        public bool CanForwardMessage(Type messageType)
        {
            return messageType == typeof(TMessage);
        }

        public async Task Forward(object message)
        {
            await ForwardCasted((TMessage)message);
        }

        public abstract Task ForwardCasted(TMessage message);
    }
}
