namespace MatthewDotCare.Umbraco.SiteMap.Models;

public class SiteMapModel
{
    public SiteMapModel(IEnumerable<SiteMapUrl> urls)
    {
        Urls = urls;
    }

    public IEnumerable<SiteMapUrl> Urls { get; }
}