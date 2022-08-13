using MatthewDotCare.Contracts.DocumentTypes;
using MatthewDotCare.Umbraco.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace MatthewDotCare.Contracts.Extensions;

public static class PublishedContentExtensions
{
    public static HomePage GetHomePage(this IPublishedContent content) => content.Root().As<HomePage>();
    public static OuterTemplate GetOuterTemplate(this IPublishedContent content) => content.GetHomePage().FirstChild<OuterTemplate>();
}