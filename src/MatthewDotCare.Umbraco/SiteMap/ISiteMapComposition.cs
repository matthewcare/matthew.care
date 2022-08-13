using Umbraco.Cms.Core.Models.PublishedContent;

namespace MatthewDotCare.Umbraco.SiteMap;

public interface ISiteMapComposition : IPublishedContent
{
    /// <summary>Page Title</summary>
    string? PageTitle { get; }

    /// <summary>Site Map Priority</summary>
    decimal SeoSiteMapPriority { get; }

    /// <summary>Site Map Change Frequency</summary>
    string SeoSiteMapChangeFrequency { get; }

    /// <summary>Site Map Display</summary>
    string SiteMapDisplay { get; }
}