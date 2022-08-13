using MatthewDotCare.Contracts.Extensions;
using MatthewDotCare.Umbraco.SiteMap;
using MatthewDotCare.Web.Controllers.SiteMap;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace MatthewDotCare.Web.Controllers.XmlSiteMap;

public class XmlSiteMapPageController : RenderController
{
    private readonly ISiteMapService _siteMapService;

    public XmlSiteMapPageController(ILogger<SiteMapPageController> logger, ICompositeViewEngine compositeViewEngine,
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

        var stream = await _siteMapService.GetXmlSiteMapAsync(home);
        return File(stream, "application/xml");
    }
}