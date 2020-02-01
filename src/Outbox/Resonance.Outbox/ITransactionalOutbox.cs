using System;
using System.Data;
using System.Threading.Tasks;

namespace Resonance.Outbox
{
    public interface ITransactionalOutbox
    {
        Task Send<TMessage>(TMessage message, IDbTransaction transaction = null, DateTime? sendTime = null);
    }
}
