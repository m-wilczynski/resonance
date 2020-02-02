using System.Threading.Tasks;

namespace Resonance.Outbox.Storage
{
    public interface IStorageInitializer
    {
        Task InitializeTables();
    }
}
