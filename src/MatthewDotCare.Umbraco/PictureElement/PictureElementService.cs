using System.Configuration;
using System.Text;
using System.Text.Encodings.Web;
using MatthewDotCare.Umbraco.DocumentTypes;
using MatthewDotCare.Umbraco.PictureElement.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Media;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Extensions;

namespace MatthewDotCare.Umbraco.PictureElement;

public class PictureElementService : IPictureElementService
{
    private readonly IImageUrlGenerator _imageUrlGenerator;
    private readonly Dictionary<string, int> _breakpoints;
    private readonly string? _convertTo;

    public PictureElementService(IOptions<PictureElementOptions> options, IImageUrlGenerator imageUrlGenerator)
    {
        _imageUrlGenerator = imageUrlGenerator;
        _convertTo = options.Value.ConvertTo;
        _breakpoints = options.Value.Breakpoints ?? throw new NullReferenceException("Picture breakpoints have not been provided");
    }

    public async Task GetPictureContentAsync(TagHelperContext context, TagHelperOutput output, MediaWithCrops media, int quality, Func<ImageCropperValue.ImageCropperCrop, bool>? predicate)
    {
        var cropsWithBreakpoints = new SortedDictionary<int, ImageCropperValue.ImageCropperCrop>(Comparer<int>.Create((x, y) => y.CompareTo(x)));
        var contents = new StringBuilder();

        var crops = media.LocalCrops.Crops ?? Array.Empty<ImageCropperValue.ImageCropperCrop>();

        if (predicate != null)
        {
            crops = crops.Where(predicate);
        }

        foreach (var crop in crops)
        {
            if (string.IsNullOrWhiteSpace(crop.Alias))
            {
                throw new ConfigurationErrorsException("Crop doesn't contain an alias");
            }

            if (!_breakpoints.TryGetValue(crop.Alias, out var breakpoint))
            {
                throw new ConfigurationErrorsException($"'{crop.Alias}' doesn't have a corresponding configuration value");
            }

            if (cropsWithBreakpoints.ContainsKey(breakpoint))
            {
                throw new ConfigurationErrorsException($"The breakpoint '{breakpoint}' exists for multiple crop alias");
            }

            cropsWithBreakpoints[breakpoint] = crop;
        }

        var format = string.IsNullOrWhiteSpace(_convertTo) ? "" : $"&format={_convertTo}";
        var formatAndQualityQuery = quality != 100 ? $"{format}&quality={quality}" : format;

        //Skip the first one, because that is the smallest crop
        foreach (var (breakpoint, crop) in cropsWithBreakpoints.Take(cropsWithBreakpoints.Count - 1))
        {
            var cropUrl = media.LocalCrops.Src?.GetCropUrl(_imageUrlGenerator, media.LocalCrops, cropAlias: crop.Alias, useCropDimensions: true);

            contents.Append("<source srcset=")
                .Append('"')
                .Append(cropUrl)
                .Append(formatAndQualityQuery)
                .Append('"')
                .Append(" media=\"(min-width:").Append(breakpoint).Append("px)").Append('"')
                .Append(" width=").Append('"').Append(crop.Width).Append('"')
                .Append(" height=").Append('"').Append(crop.Height).Append('"')
                .Append(" />");
        }

        var childContentModel = new PictureChildContentModel
        {
            Crop = cropsWithBreakpoints.Last().Value,
            Media = media,
            FormatAndQualityQuery = formatAndQualityQuery
        };

        context.Items.Add(typeof(PictureChildContentModel), childContentModel);

        var content = new DefaultTagHelperContent().AppendHtml(contents.ToString());

        var childContent = await output.GetChildContentAsync();

        if (childContentModel.HasProcessed)
        {
            content.AppendHtml(childContent);
        }
        else
        {
            var imgTagContext = new TagHelperContext("img", new TagHelperAttributeList(), new Dictionary<object, object>(), "UNIQUE");
            var imgTagOutput = new TagHelperOutput("img", new TagHelperAttributeList(), GetChildContentAsync);
            ProcessImageTag(imgTagContext, imgTagOutput, childContentModel);
            content.AppendHtml(imgTagOutput);
        }

        output.Content.SetHtmlContent(content);
    }

    private static Task<TagHelperContent> GetChildContentAsync(bool arg1, HtmlEncoder arg2) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent());

    public void ProcessImageTag(TagHelperContext context, TagHelperOutput output, PictureChildContentModel model)
    {
        var iImage = model.Media.Content as IImage;
        var cropUrl = model.Media.LocalCrops.Src?.GetCropUrl(_imageUrlGenerator, model.Media.LocalCrops, cropAlias: model.Crop.Alias, useCropDimensions: true);

        var altAttributeValue = GetAltAttributeValue(context, model, iImage);
        output.Attributes.SetAttribute("alt", altAttributeValue);

        output.Attributes.SetAttribute("src", $"{cropUrl}{model.FormatAndQualityQuery}");
        output.Attributes.SetAttribute("width", model.Crop.Width);
        output.Attributes.SetAttribute("height", model.Crop.Height);

        if (!output.Attributes.TryGetAttribute("decoding", out _))
        {
            output.Attributes.SetAttribute("decoding", "async");
        }

        if (!output.Attributes.TryGetAttribute("loading", out _))
        {
            output.Attributes.SetAttribute("loading", "lazy");
        }

        if (string.IsNullOrWhiteSpace(iImage?.PictureElementBase64))
        {
            return;
        }

        var styleValue = GetImageStyle(iImage);

        // Required so that the value doesn't get encoded
        var attribute = new TagHelperAttribute("style", styleValue);

        output.Attributes.Add(attribute);
    }

    /// <summary>
    /// Gets the contents of the style attribute
    /// </summary>
    /// <param name="image">An IImage with base64 data</param>
    public HtmlString GetImageStyle(IImage image)
    {
        var data = new StringBuilder()
            .Append("background-size:cover;background-image:url(&quot;data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink'%3E%3Cfilter id='a' color-interpolation-filters='sRGB'%3E%3CfeGaussianBlur stdDeviation='9'/%3E%3CfeComponentTransfer%3E%3CfeFuncA type='discrete' tableValues='1 1'/%3E%3C/feComponentTransfer%3E%3C/filter%3E%3Cimage filter='url(%23a)' height='100%25' width='100%25' preserveAspectRatio='xMidYMid slice' xlink:href='")
            .Append(image.PictureElementBase64)
            .Append("'/%3E%3C/svg%3E&quot;);")
            .ToString();

        return new HtmlString(data);
    }

    /// <summary>
    /// Gets the alt attribute value from either an existing alt attribute
    /// or a specified alternate text from the Umbraco property, or if all
    /// of those are unspecified, the Umbraco image name.
    /// </summary>
    /// <returns>A non-empty value for image alternate text, with a appended "." for screen readers.</returns>
    private static string? GetAltAttributeValue(TagHelperContext context, PictureChildContentModel model, IImage? iImage)
    {
        context.AllAttributes.TryGetAttribute("alt", out var altAttribute);

        var altValue = altAttribute?.Value?.ToString();

        if (string.IsNullOrWhiteSpace(altValue))
        {
            altValue = iImage?.AlternativeText;
        }

        if (string.IsNullOrWhiteSpace(altValue))
        {
            altValue = model.Media.Content.Name;
        }

        // This should never be the case, but Umbraco
        // marks content names as nullable, so we need
        // to check regardless.
        if (string.IsNullOrWhiteSpace(altValue))
        {
            return null;
        }

        // Ensure it ends with a "." for screen reader purposes
        altValue = altValue.EnsureEndsWith(".");

        return altValue;
    }
}