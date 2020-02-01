using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resonance.FeatureFlags
{
    public interface IFeatureFlagRegistry
    {
        Task<bool> IsFlagActive(string code);
        Task<FeatureFlag> GetFlagByCode(string code);
        Task<ICollection<FeatureFlag>> GetAllFlags();
        Task<ICollection<FeatureFlag>> GetActiveFlagsToShow();
    }
}
