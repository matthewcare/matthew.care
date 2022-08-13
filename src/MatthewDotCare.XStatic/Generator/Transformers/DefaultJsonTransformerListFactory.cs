namespace MatthewDotCare.XStatic.Generator.Transformers
{
    public class DefaultJsonTransformerListFactory : ITransformerListFactory
    {
        public virtual IEnumerable<ITransformer> BuildTransformers(ISiteConfig siteConfig)
        {
            yield return new UmbracoContentUdiToJsonUrlTransformer();
        }
    }
}