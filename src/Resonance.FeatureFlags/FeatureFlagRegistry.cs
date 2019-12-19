using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Resonance.FeatureFlags.Storage;

namespace Resonance.FeatureFlags
{
    public class FeatureFlagRegistry : IFeatureFlagRegistry
    {
        private readonly Dictionary<string, FeatureFlag> _featureFlagByCode = new Dictionary<string, FeatureFlag>();

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

            _featureFlagByCode.Clear();

            foreach (var flag in await _flagsRepository.GetAllFlags())
            {
                _featureFlagByCode.Add(flag.Code, flag);
            }

            _initialized = true;
        }

        public Task<bool> IsFlagActive(string code)
        {
            if (!_featureFlagByCode.ContainsKey(code))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(_featureFlagByCode[code].IsActive);
        }

        public Task<FeatureFlag> GetFlagByCode(string code)
        {
            _featureFlagByCode.TryGetValue(code, out var flag);
            return Task.FromResult(flag);
        }

        public Task<ICollection<FeatureFlag>> GetAllFlags()
        {
            return Task.FromResult((ICollection<FeatureFlag>)_featureFlagByCode.Values.ToList());
        }

        public Task<ICollection<FeatureFlag>> GetActiveFlagsToShow()
        {
            return Task.FromResult((ICollection<FeatureFlag>)_featureFlagByCode.Values
                .Where(flag => flag.IsActive && flag.ShowToEndUser)
                .ToList());
        }
    }
}
