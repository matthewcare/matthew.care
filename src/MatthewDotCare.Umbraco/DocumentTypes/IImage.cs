using Umbraco.Cms.Core.Models.PublishedContent;

namespace MatthewDotCare.Umbraco.DocumentTypes;

public interface IImage : IPublishedContent
{
    string AlternativeText { get; }
    string PictureElementBase64 { get; }
}