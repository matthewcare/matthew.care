using MatthewDotCare.Umbraco.PictureElement;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace MatthewDotCare.Umbraco.TagHelpers;

[HtmlTargetElement("picture", Attributes = ForMediaName)]
[HtmlTargetElement("picture", Attributes = QualityName)]
[HtmlTargetElement("picture", Attributes = WhereCropsName)]
public class PictureTagHelper : TagHelper
{
    private readonly PictureElementOptions _options;
    private readonly IPictureElementService _pictureService;

    public PictureTagHelper(IOptionsMonitor<PictureElementOptions> options, IPictureElementService pictureService)
    {
        _options = options.CurrentValue;
        _pictureService = pictureService;
    }

    private const string ForMediaName = "for";
    private const string QualityName = "quality";
    private const string WhereCropsName = "where";

    [HtmlAttributeName(ForMediaName)]
    public MediaWithCrops? Media { get; set; }

    [HtmlAttributeName(QualityName)]
    public int Quality { get; set; }

    [HtmlAttributeName(WhereCropsName)]
    public Func<ImageCropperValue.ImageCropperCrop, bool>? WhereCropsPredicate { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (Media?.LocalCrops == null)
        {
            output.SuppressOutput();
            return;
        }

        var quality = context.AllAttributes.ContainsName(QualityName) ? Quality : _options.DefaultQuality;
        quality = Math.Clamp(quality, 5, 100);

        await _pictureService.GetPictureContentAsync(context, output, Media, quality, WhereCropsPredicate);
    }
}