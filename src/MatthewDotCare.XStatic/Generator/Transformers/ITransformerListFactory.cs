namespace MatthewDotCare.XStatic.Generator.Transformers
{
    public interface ITransformerListFactory
    {
        IEnumerable<ITransformer> BuildTransformers(ISiteConfig siteConfig);
    }
}