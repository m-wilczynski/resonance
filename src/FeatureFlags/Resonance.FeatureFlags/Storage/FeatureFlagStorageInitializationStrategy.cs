namespace Resonance.FeatureFlags.Storage
{
    public enum FeatureFlagStorageInitializationStrategy
    {
        None = 0,
        CreateIfNotExists = 1,
        AlwaysRecreate = 2
    }
}
