using MatthewDotCare.Umbraco.DocumentTypes;
using MatthewDotCare.Umbraco.PictureElement.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace MatthewDotCare.Umbraco.PictureElement;

public interface IPictureElementService
{
    Task GetPictureContentAsync(TagHelperContext context, TagHelperOutput output, MediaWithCrops media, int quality, Func<ImageCropperValue.ImageCropperCrop, bool>? predicate);
    void ProcessImageTag(TagHelperContext context, TagHelperOutput output, PictureChildContentModel model);
    HtmlString GetImageStyle(IImage image);
}