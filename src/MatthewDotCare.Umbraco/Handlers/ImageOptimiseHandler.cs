using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;
using IImage = MatthewDotCare.Umbraco.DocumentTypes.IImage;

namespace MatthewDotCare.Umbraco.Handlers;

public class ImageOptimiseHandler : INotificationAsyncHandler<MediaSavingNotification>
{
    private readonly MediaFileManager _mediaFileManager;
    private readonly MediaUrlGeneratorCollection _mediaUrlGeneratorCollection;
    private readonly IOptionsMonitor<ContentSettings> _contentSettings;
    private readonly int _maxImageDimension;

    public ImageOptimiseHandler(IConfiguration configuration, MediaFileManager mediaFileManager,
        MediaUrlGeneratorCollection mediaUrlGeneratorCollection, IOptionsMonitor<ContentSettings> contentSettings)
    {
        _mediaFileManager = mediaFileManager;
        _mediaUrlGeneratorCollection = mediaUrlGeneratorCollection;
        _contentSettings = contentSettings;
        _maxImageDimension = configuration.GetValue("MaxImageDimension", 1920);
    }

    public async Task HandleAsync(MediaSavingNotification notification, CancellationToken cancellationToken)
    {
        IEnumerable<string> supportedTypes = _contentSettings.CurrentValue.Imaging.ImageFileTypes.ToList();

        foreach (var entity in notification.SavedEntities)
        {
            if (!entity.HasProperty("umbracoFile") || entity.GetValue<string>("umbracoFile").IsNullOrWhiteSpace())
            {
                continue;
            }

            // Make sure it's an image.
            var path = entity.GetUrl("umbracoFile", _mediaUrlGeneratorCollection);

            if (string.IsNullOrWhiteSpace(path))
            {
                continue;
            }

            var extension = Path.GetExtension(path)[1..];

            if (string.IsNullOrWhiteSpace(extension) || !supportedTypes.InvariantContains(extension))
            {
                continue;
            }

            // Resize the image to the max allowed dimension
            var fullPath = _mediaFileManager.FileSystem.GetFullPath(path);

            var image = Image.Load(fullPath, out var format);
            var widthLargerThanHeight = image.Width > image.Height;

            image.Metadata.ExifProfile = null;
            await ConstrainImageAsync(notification, image, widthLargerThanHeight, fullPath, entity);
            CreatePreLoad(image, widthLargerThanHeight, entity, format);
        }
    }

    private async Task ConstrainImageAsync(MediaSavingNotification notification, Image image,
        bool widthLargerThanHeight, string fullPath, IContentBase entity)
    {
        ResizeOptions? resizeOptions = null;

        if (widthLargerThanHeight && image.Width > _maxImageDimension)
        {
            resizeOptions = new ResizeOptions
            {
                Size = new Size(_maxImageDimension, 0),
                Mode = ResizeMode.Min
            };
        }
        else if (!widthLargerThanHeight && image.Height > _maxImageDimension)
        {
            resizeOptions = new ResizeOptions
            {
                Size = new Size(0, _maxImageDimension),
                Mode = ResizeMode.Min
            };
        }

        if (resizeOptions == null)
        {
            return;
        }

        var originalHeight = image.Height;
        var originalWidth = image.Width;

        var clone = image.Clone(i => i.Resize(resizeOptions));
        await clone.SaveAsync(fullPath);

        var newHeight = clone.Height;
        var newWidth = clone.Width;

        notification.Messages.Add(new EventMessage("Image Resized",
            $"'{entity.Name}' has been resized from {originalWidth}x{originalHeight} to {newWidth}x{newHeight} to meet the configured limit.",
            EventMessageType.Info));


        // Update these, else we'll have stale properties
        entity.SetValue("umbracoWidth", newWidth);
        entity.SetValue("umbracoHeight", newHeight);
        await using var bytes = _mediaFileManager.FileSystem.OpenFile(fullPath);
        entity.SetValue("umbracoBytes", bytes.Length);
    }

    private void CreatePreLoad(Image image, bool widthLargerThanHeight, IContentBase entity, IImageFormat format)
    {
        var tinyHeight = 0;
        var tinyWidth = 0;

        if (widthLargerThanHeight)
        {
            tinyWidth = 6;
        }
        else
        {
            tinyHeight = 6;
        }

        var tinyResizeLayer = new ResizeOptions
        {
            Size = new Size(tinyWidth, tinyHeight),
            Mode = ResizeMode.Min
        };

        var clone = image.Clone(i => i.Resize(tinyResizeLayer));

        var base64String = clone.ToBase64String(format);
        entity.SetValue(nameof(IImage.PictureElementBase64).ToFirstLower(), base64String);
    }

}