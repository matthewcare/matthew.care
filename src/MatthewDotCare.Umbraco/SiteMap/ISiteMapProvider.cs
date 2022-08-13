using MatthewDotCare.Umbraco.SiteMap.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace MatthewDotCare.Umbraco.SiteMap;

public interface ISiteMapProvider
{
    Task<IEnumerable<SiteMapUrl>> GetUrlsAsync(IPublishedContent rootContent);
}