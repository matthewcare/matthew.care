using MatthewDotCare.Umbraco.Handlers;
using MatthewDotCare.Umbraco.PictureElement;
using MatthewDotCare.Umbraco.SiteMap;
using MatthewDotCare.Umbraco.ViewContent;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using SixLabors.ImageSharp.Web.Middleware;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;

namespace MatthewDotCare.Umbraco;

public class DependencyComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        // Options
        builder.Services.Configure<PictureElementOptions>(builder.Config.GetSection("PictureElement"));

        // Umbraco registrations
        builder.Services.AddSingleton<IPictureElementService, PictureElementService>();
        builder.Services.AddTransient<ISiteMapService, SiteMapService>();
        builder.Services.AddSingleton<ISiteMapProvider, UmbracoSiteMapProvider>();
        builder.Services.AddUnique<IDefaultViewContentProvider, ViewContentProvider>();

        // Notification handlers
        builder.AddNotificationAsyncHandler<MediaSavingNotification, ImageOptimiseHandler>();

        // ImageSharp
        builder.Services.Configure<ImageSharpMiddlewareOptions>(options =>
        {
            // Capture existing command parser so we don't override
            var onParseCommandsAsync = options.OnParseCommandsAsync;
            options.OnParseCommandsAsync = async context =>
            {
                await onParseCommandsAsync(context);

                context.Commands.TryGetValue("format", out var format);
                var acceptsWebP = context.Context.Request.GetTypedHeaders().Accept
                    .Contains(new MediaTypeHeaderValue("image/webp"));

                if (string.Equals(format, "webp", StringComparison.InvariantCultureIgnoreCase) && !acceptsWebP)
                {
                    context.Commands.Remove("format");
                }

            };
        });

    }
}
