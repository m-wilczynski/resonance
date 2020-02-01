using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Resonance.Outbox.Serialization;

namespace Resonance.Outbox.Storage
{
    public interface IMessageRepository
    {
        /// <summary>
        /// Persist message sent to outbox
        /// </summary>
        /// <param name="serializedMessage">Message to persist</param>
        /// <param name="transaction">Transaction in which message persisting will take place;
        /// please note that it should be left NULL only if you're inside ambient transaction (ie. TranscationScope) </param>
        /// <returns></returns>
        Task SaveMessage(SerializedMessage serializedMessage, IDbTransaction transaction = null);

        /// <summary>
        /// Get messages for sending and marks them with current time (UTC) on read;
        /// </summary>
        /// <param name="howManyMessages">How many messages to take (ie. SELECT TOP <paramref name="howManyMessages"/>); if NULL - takes all</param>
        /// <returns></returns>
        Task<ICollection<SerializedMessage>> GetMessagesAsMarkedSent(uint? howManyMessages = null);

        /// <summary>
        /// Get messages for sending and removes them from storage on read;
        /// </summary>
        /// <param name="howManyMessages">How many messages to take (ie. SELECT TOP <paramref name="howManyMessages"/>); if NULL - takes all</param>
        /// <returns></returns>
        Task<ICollection<SerializedMessage>> GetMessagesAsRemoved(uint? howManyMessages = null);
    }
}
