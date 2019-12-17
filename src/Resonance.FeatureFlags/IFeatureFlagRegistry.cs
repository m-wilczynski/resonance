using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resonance.FeatureFlags
{
    public interface IFeatureFlagRegistry
    {
        Task<FeatureFlag> IsFlagActive(string code);
        Task<FeatureFlag> GetFlagByCode(string code);
        Task<FeatureFlag> GetFlagById(Guid id);
        Task<ICollection<FeatureFlag>> GetAllFlags();
        Task<ICollection<FeatureFlag>> GetActiveFlagsToShow();
    }
}
