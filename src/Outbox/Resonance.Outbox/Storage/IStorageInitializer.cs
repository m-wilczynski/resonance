using System.Threading.Tasks;

namespace Resonance.Outbox.Storage
{
    public interface IStorageInitializer
    {
        bool InitializeOnStartup { get; }
        Task InitializeTables();
    }
}
