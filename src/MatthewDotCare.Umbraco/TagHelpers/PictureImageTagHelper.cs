using MatthewDotCare.Umbraco.PictureElement;
using MatthewDotCare.Umbraco.PictureElement.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MatthewDotCare.Umbraco.TagHelpers;

[HtmlTargetElement("img", ParentTag = "picture")]
public class PictureImageTagHelper : TagHelper
{
    private readonly IPictureElementService _pictureService;

    public PictureImageTagHelper(IPictureElementService pictureService)
    {
        _pictureService = pictureService;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!context.Items.TryGetValue(typeof(PictureChildContentModel), out var value))
        {
            return;
        }

        var parentContentModel = (PictureChildContentModel)value;
        parentContentModel.HasProcessed = true;
        _pictureService.ProcessImageTag(context, output, parentContentModel);
    }
}