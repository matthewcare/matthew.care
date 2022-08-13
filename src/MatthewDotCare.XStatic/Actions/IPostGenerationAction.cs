namespace MatthewDotCare.XStatic.Actions
{
    public interface IPostGenerationAction
    {
        Task<XStaticResult> RunAction(int staticSiteId, Dictionary<string, string> parameters);
    }
}