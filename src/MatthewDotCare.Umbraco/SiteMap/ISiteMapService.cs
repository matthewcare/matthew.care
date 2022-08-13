using MatthewDotCare.Umbraco.SiteMap.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace MatthewDotCare.Umbraco.SiteMap;

public interface ISiteMapService
{
    Task<SiteMapModel> GetSiteMapAsync(IPublishedContent? rootContent);
    Task<Stream> GetXmlSiteMapAsync(IPublishedContent? rootContent);
}