using System;

namespace Resonance.Outbox.Outbound
{
    public class ForwardLogEntry
    {
        public ForwardLogEntry(bool success, DateTime? completionDate = null)
        {
            CompletionDateUtc = completionDate ?? DateTime.UtcNow;
            Success = success;
        }

        public DateTime CompletionDateUtc  { get; }
        public bool Success { get; }
    }
}
