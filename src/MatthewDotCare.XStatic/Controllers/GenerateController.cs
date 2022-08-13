﻿using MatthewDotCare.XStatic.Actions;
using MatthewDotCare.XStatic.Generator.ExportTypes;
using MatthewDotCare.XStatic.Generator.Processes;
using MatthewDotCare.XStatic.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace MatthewDotCare.XStatic.Controllers
{
    [PluginController("xstatic")]
    public class GenerateController : UmbracoAuthorizedJsonController
    {
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly IExportTypeService _exportTypeService;
        private ISitesRepository _sitesRepo;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IActionFactory _actionFactory;

        public GenerateController(
            IUmbracoContextFactory umbracoContextFactory,
            IExportTypeService exportTypeService,
            ISitesRepository sitesRepository,
            IWebHostEnvironment webHostEnvironment,
            IActionFactory actionFactory)
        {
            _umbracoContextFactory = umbracoContextFactory;
            _exportTypeService = exportTypeService;
            _sitesRepo = sitesRepository;
            _webHostEnvironment = webHostEnvironment;
            _actionFactory = actionFactory;
        }

        [HttpGet]
        public async Task<string> RebuildStaticSite(int staticSiteId)
        {
            var process = new RebuildProcess(_umbracoContextFactory, _exportTypeService, _sitesRepo, _webHostEnvironment, _actionFactory);

            return await process.RebuildSite(staticSiteId);
        }
    }
}