using System;

namespace Resonance.Outbox.Serialization
{
    public class SerializedMessage
    {
        private SerializedMessage() { }

        public SerializedMessage(
            byte[] payload, 
            Type messageTypeQualifiedName, 
            DateTime? sendTimeUtc)
        {
            if (messageTypeQualifiedName == null) throw new ArgumentNullException(nameof(messageTypeQualifiedName));
            Payload = payload ?? throw new ArgumentNullException(nameof(payload));
            MessageTypeAssemblyQualifiedName = messageTypeQualifiedName.AssemblyQualifiedName;
            SendTimeUtc = sendTimeUtc?.ToUniversalTime() ?? DateTime.UtcNow;
        }

        public byte[] Payload { get; private set; }
        public string MessageTypeAssemblyQualifiedName { get; private set; }
        public DateTime SendTimeUtc { get; private set; }
    }
}
