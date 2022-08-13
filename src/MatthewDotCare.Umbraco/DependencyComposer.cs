using MatthewDotCare.Umbraco.SiteMap;
using MatthewDotCare.Umbraco.ViewContent;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.IO;
using Umbraco.Extensions;

namespace MatthewDotCare.Umbraco;

public class DependencyComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {

        // Limetta.Umbraco registrations
        builder.Services.AddTransient<ISiteMapService, SiteMapService>();
        builder.Services.AddSingleton<ISiteMapProvider, UmbracoSiteMapProvider>();
        builder.Services.AddUnique<IDefaultViewContentProvider, ViewContentProvider>();

    }
}
