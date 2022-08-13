namespace MatthewDotCare.Umbraco.SiteMap.Models;

public struct SiteMapUrl
{
    public SiteMapUrl(IEnumerable<SiteMapUrl> children)
    {
        ExcludedSelf = true;
        Name = string.Empty;
        Url = string.Empty;
        LastModified = DateTime.MinValue;
        ChangeFrequency = string.Empty;
        Priority = 0;
        Children = children;
    }

    public SiteMapUrl(string? name, string? url, DateTime lastModified, string? changeFrequency, decimal priority, IEnumerable<SiteMapUrl>? children)
    {
        ExcludedSelf = false;
        Name = name;
        Url = url;
        LastModified = lastModified;
        ChangeFrequency = changeFrequency;
        Priority = priority;
        Children = children;
    }

    public bool ExcludedSelf { get; }

    public string? Name { get; }

    public string? Url { get; }

    public DateTime LastModified { get; }

    public string? ChangeFrequency { get; }

    public decimal Priority { get; }

    public IEnumerable<SiteMapUrl>? Children { get; }
}