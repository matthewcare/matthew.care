using MatthewDotCare.Contracts.DocumentTypes;
using MatthewDotCare.Umbraco.SiteMap.Models;
using Umbraco.Cms.Core.Models;

namespace MatthewDotCare.Web.Controllers.SiteMap;

public class SiteMapPageModel : ContentModel<SiteMapPage>
{
    public SiteMapPageModel(SiteMapPage page) : base(page)
    {
    }

    public SiteMapModel SiteMap { get; init; } = null!;

}