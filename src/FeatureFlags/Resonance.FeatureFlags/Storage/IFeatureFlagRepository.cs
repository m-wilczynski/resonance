using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resonance.FeatureFlags.Storage
{
    public interface IFeatureFlagRepository
    {
        Task Initialize(FeatureFlagStorageOptions storageOptions);
        Task<ICollection<FeatureFlag>> GetAllFlags();
    }
}
