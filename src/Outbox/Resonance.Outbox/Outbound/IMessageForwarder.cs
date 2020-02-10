using System;
using System.Threading.Tasks;

namespace Resonance.Outbox.Outbound
{
    /// <summary>
    /// API for pushing/forwarding particular type (or types) of messages out from outbox
    /// </summary>
    public interface IMessageForwarder
    {
        /// <summary>
        /// Can messages of given <paramref name="messageType"/> be processed?
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        bool CanForwardMessage(Type messageType);

        /// <summary>
        /// Push/forward message out from outbox
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task Forward(object message);
    }
}
