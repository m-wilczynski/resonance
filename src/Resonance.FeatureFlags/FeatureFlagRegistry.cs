using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Resonance.FeatureFlags.Storage;

namespace Resonance.FeatureFlags
{
    public class FeatureFlagRegistry : IFeatureFlagRegistry
    {
        private readonly IFeatureFlagRepository _flagsRepository;
        private readonly FeatureFlagsRegistryOptions _options;
        private bool _initialized;

        public FeatureFlagRegistry(
            IFeatureFlagRepository flagsRepository,
            FeatureFlagsRegistryOptions options)
        {
            _flagsRepository = flagsRepository ?? throw new ArgumentNullException(nameof(flagsRepository));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task InitializeStorage()
        {
            if (_initialized) { return; }

            await _flagsRepository.Initialize(_options.StorageOptions);
            _initialized = true;
        }

        public Task<FeatureFlag> IsFlagActive(string code)
        {
            throw new NotImplementedException();
        }

        public Task<FeatureFlag> GetFlagByCode(string code)
        {
            throw new NotImplementedException();
        }

        public Task<FeatureFlag> GetFlagById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<FeatureFlag>> GetAllFlags()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<FeatureFlag>> GetActiveFlagsToShow()
        {
            throw new NotImplementedException();
        }
    }
}
