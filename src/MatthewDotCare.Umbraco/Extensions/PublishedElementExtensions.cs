using Umbraco.Cms.Core.Models.PublishedContent;

namespace MatthewDotCare.Umbraco.Extensions;

public static class PublishedElementExtensions
{
    /// <summary>
    /// Tries to cast an IPublishedElement to a specified Published Element type
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static TDocument As<TDocument>(this IPublishedElement content) where TDocument : class, IPublishedElement
    {
        return content as TDocument ??
               throw new ArgumentException(
                   $"The IPublishedElement instance is a document of type {content.GetType()}, which does not match the requested type {typeof(TDocument)}.",
                   nameof(content));
    }
}