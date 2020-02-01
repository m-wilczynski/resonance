using System;

namespace Resonance.Outbox.Serialization
{
    public class SerializedMessage
    {
        public SerializedMessage(byte[] payload, Type messageTypeQualifiedName, DateTime? sendTimeUtc)
        {
            Payload = payload;
            MessageTypeAssemblyQualifiedName = messageTypeQualifiedName.AssemblyQualifiedName;
            SendTimeUtc = sendTimeUtc?.ToUniversalTime() ?? DateTime.UtcNow;
        }

        public byte[] Payload { get; private set; }
        public string MessageTypeAssemblyQualifiedName { get; private set; }
        public DateTime SendTimeUtc { get; private set; }
    }
}
