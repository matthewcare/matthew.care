using MatthewDotCare.XStatic.Generator.Storage;

namespace MatthewDotCare.XStatic.Generator.Transformers
{
    public class DefaultHtmlTransformerListFactory : ITransformerListFactory
    {
        public virtual IEnumerable<ITransformer> BuildTransformers(ISiteConfig siteConfig)
        {
            yield return new CachedByTransformer();

            if (!string.IsNullOrEmpty(siteConfig.ImageCrops))
            {
                var crops = Crop.GetCropsFromCommaDelimitedString(siteConfig.ImageCrops);
                yield return new CroppedImageUrlTransformer(new ImageCropNameGenerator(), crops);
            }

            if (!string.IsNullOrEmpty(siteConfig.TargetHostname))
            {
                yield return new HostnameTransformer(siteConfig.TargetHostname);
            }
        }
    }
}