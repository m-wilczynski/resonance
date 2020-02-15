using Resonance.Outbox.Serialization.MessagePack;

namespace Resonance.Outbox.Serialization.Tests
{
    public class MessagePackCommonScenariosTest : CommonScenariosTestBase<MessagePackSerializer>
    {
        public MessagePackCommonScenariosTest() : base(new MessagePackSerializer())
        {
        }
    }
}
