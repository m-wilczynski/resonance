using System;
using System.Data;
using System.Threading.Tasks;

namespace Resonance.Outbox.Inbound
{
    /// <summary>
    /// API for passing messages to be send through outbox (inbound traffic to outbox)
    /// </summary>
    public interface ITransactionalOutbox
    {
        /// <summary>
        /// Send message to outbox
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message"></param>
        /// <param name="transaction"></param>
        /// <param name="sendTime"></param>
        /// <returns></returns>
        Task Send<TMessage>(TMessage message, IDbTransaction transaction = null, DateTime? sendTime = null);
    }
}
