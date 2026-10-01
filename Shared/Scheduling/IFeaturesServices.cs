namespace WorkLifeBalance.Shared.Scheduling
{
    public interface IFeaturesServices
    {
        public bool IsFeaturePresent<TFeature>() where TFeature : FeatureBase;
        public void AddFeature<TFeature>() where TFeature : FeatureBase;
        public void RemoveFeature<TFeature>() where TFeature : FeatureBase;
    }
}
