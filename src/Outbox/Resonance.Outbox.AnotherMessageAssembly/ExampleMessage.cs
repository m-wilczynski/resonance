using System;
using System.Runtime.Serialization;

namespace Resonance.Outbox.MessageAssembly
{
    [DataContract]
    public class ExampleMessage
    {
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public int[] LargeArray { get; set; }
    }
}
