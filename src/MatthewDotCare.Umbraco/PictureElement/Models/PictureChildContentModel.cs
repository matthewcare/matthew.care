using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace MatthewDotCare.Umbraco.PictureElement.Models;

public class PictureChildContentModel
{
    public MediaWithCrops Media { get; set; } = null!;
    public string? FormatAndQualityQuery { get; set; }
    public ImageCropperValue.ImageCropperCrop Crop { get; set; } = null!;
    public bool HasProcessed { get; set; }
}