using Resonance.Outbox.Serialization.Utf8Json;

namespace Resonance.Outbox.Serialization.Tests
{
    public class Utf8JsonCommonScenariosTest : CommonScenariosTestBase<Utf8JsonSerializer>
    {
        public Utf8JsonCommonScenariosTest() : base(new Utf8JsonSerializer())
        {
        }
    }
}
