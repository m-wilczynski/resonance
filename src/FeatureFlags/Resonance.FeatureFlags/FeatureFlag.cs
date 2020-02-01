using System;

namespace Resonance.FeatureFlags
{
    public class FeatureFlag
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool ShowToEndUser { get; private set; }
        public bool IsActive { get; private set; }
    }
}
