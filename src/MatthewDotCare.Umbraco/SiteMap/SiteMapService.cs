using System.Globalization;
using System.Text;
using System.Xml;
using MatthewDotCare.Umbraco.SiteMap.Models;
using Microsoft.Extensions.Caching.Memory;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace MatthewDotCare.Umbraco.SiteMap;

internal class SiteMapService : ISiteMapService
{
    private const string DefaultChangeFrequency = "weekly";
    private const decimal DefaultPriority = 0.5m;

    private const string CacheKey = "siteMap";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    private readonly IEnumerable<ISiteMapProvider> _siteMapProviders;
    private readonly IMemoryCache _cache;

    public SiteMapService(IEnumerable<ISiteMapProvider> siteMapProviders, IMemoryCache cache)
    {
        _siteMapProviders = siteMapProviders;
        _cache = cache;
    }

    public async Task<SiteMapModel> GetSiteMapAsync(IPublishedContent? rootContent)
    {
        if (rootContent == null)
        {
            throw new ArgumentNullException(nameof(rootContent));
        }

        var cacheKey = string.Join(":", CacheKey, rootContent.Id);

        if (_cache.TryGetValue(cacheKey, out SiteMapModel model))
        {
            return model;
        }

        var siteMapTasks = _siteMapProviders.Select(p => p.GetUrlsAsync(rootContent));
        var siteMapUrls = await Task.WhenAll(siteMapTasks);

        var urls = siteMapUrls.SelectMany(x => x).ToArray();
        model = new SiteMapModel(urls);

        return _cache.Set(cacheKey, model, CacheDuration);
    }

    public async Task<Stream> GetXmlSiteMapAsync(IPublishedContent? rootContent)
    {
        var siteMap = await GetSiteMapAsync(rootContent);

        var settings = new XmlWriterSettings
        {
            Indent = true,
            Encoding = Encoding.UTF8,
            Async = true
        };

        var stream = new MemoryStream();
        await using (var writer = XmlWriter.Create(stream, settings))
        {
            writer.WriteStartElement("urlset", "http://www.siteMaps.org/schemas/siteMap/0.9");

            WriteUrl(writer, siteMap.Urls);

            await writer.WriteEndElementAsync();
            await writer.FlushAsync();
        }

        stream.Position = 0;

        return stream;
    }

    private static void WriteUrl(XmlWriter writer, IEnumerable<SiteMapUrl> siteMapUrls)
    {
        foreach (var siteMapUrl in siteMapUrls)
        {
            if (!siteMapUrl.ExcludedSelf)
            {
                var changeFrequency = string.IsNullOrWhiteSpace(siteMapUrl.ChangeFrequency) ? DefaultChangeFrequency : siteMapUrl.ChangeFrequency;
                var priority = siteMapUrl.Priority <= 0 ? DefaultPriority : siteMapUrl.Priority;

                writer.WriteStartElement("url");

                writer.WriteElementString("loc", siteMapUrl.Url);
                writer.WriteElementString("lastmod", $"{siteMapUrl.LastModified:s}");
                writer.WriteElementString("changefreq", changeFrequency);
                writer.WriteElementString("priority", priority.ToString(CultureInfo.InvariantCulture));

                writer.WriteEndElement();
            }

            if (siteMapUrl.Children != null)
            {
                WriteUrl(writer, siteMapUrl.Children);
            }
        }
    }
}