using MatthewDotCare.Core.Extensions;
using MatthewDotCare.Umbraco.SiteMap.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace MatthewDotCare.Umbraco.SiteMap;

internal class UmbracoSiteMapProvider : ISiteMapProvider
{
    private readonly IPublishedUrlProvider _urlProvider;

    public UmbracoSiteMapProvider(IPublishedUrlProvider urlProvider)
    {
        _urlProvider = urlProvider;
    }

    public Task<IEnumerable<SiteMapUrl>> GetUrlsAsync(IPublishedContent rootContent)
    {
        var siteMapUrls = MapContentToSiteMapUrl(new[] { rootContent });

        return Task.FromResult(siteMapUrls);
    }

    private IEnumerable<SiteMapUrl> MapContentToSiteMapUrl(IEnumerable<IPublishedContent> pages)
    {
        foreach (var page in pages.OfType<ISiteMapComposition>())
        {
            Enum.TryParse(page.SiteMapDisplay.Replace(" ", ""), out SiteMapDisplayType siteMapDisplay);

            if (siteMapDisplay == SiteMapDisplayType.ExcludeSelfAndChildren || siteMapDisplay == SiteMapDisplayType.ExcludeSelf && page.Children.IsNullOrEmpty())
            {
                continue;
            }

            var children = page.Children ?? Array.Empty<IPublishedContent>();

            if (siteMapDisplay == SiteMapDisplayType.ExcludeSelf)
            {
                yield return new SiteMapUrl(children: MapContentToSiteMapUrl(children));
                continue;
            }

            yield return new SiteMapUrl(
                name: string.IsNullOrEmpty(page.PageTitle) ? page.Name : page.PageTitle,
                url: page.Url(_urlProvider, mode: UrlMode.Absolute),
                lastModified: page.UpdateDate,
                changeFrequency: page.SeoSiteMapChangeFrequency,
                priority: page.SeoSiteMapPriority,
                children: siteMapDisplay == SiteMapDisplayType.ExcludeChildren
                    ? Array.Empty<SiteMapUrl>()
                    : MapContentToSiteMapUrl(children)
            );
        }
    }
}