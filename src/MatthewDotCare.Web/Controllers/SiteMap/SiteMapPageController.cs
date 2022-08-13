using MatthewDotCare.Contracts.DocumentTypes;
using MatthewDotCare.Contracts.Extensions;
using MatthewDotCare.Umbraco.SiteMap;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace MatthewDotCare.Web.Controllers.SiteMap;

public class SiteMapPageController : RenderController
{
    private readonly ISiteMapService _siteMapService;

    public SiteMapPageController(ILogger<SiteMapPageController> logger, ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor, ISiteMapService siteMapService)
        : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
        _siteMapService = siteMapService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string type)
    {
        var home = CurrentPage?.GetHomePage();

        if (CurrentPage is null || home is null)
        {
            throw new NullReferenceException("Couldn't find home page");
        }

        var model = new SiteMapPageModel((SiteMapPage)CurrentPage)
        {
            SiteMap = await _siteMapService.GetSiteMapAsync(home)
        };

        return CurrentTemplate(model);
    }
}