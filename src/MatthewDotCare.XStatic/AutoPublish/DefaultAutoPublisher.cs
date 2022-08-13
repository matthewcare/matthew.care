﻿using MatthewDotCare.XStatic.Actions;
using MatthewDotCare.XStatic.Deploy;
using MatthewDotCare.XStatic.Deploy.Processes;
using MatthewDotCare.XStatic.Generator.ExportTypes;
using MatthewDotCare.XStatic.Generator.Processes;
using MatthewDotCare.XStatic.Generator.Storage;
using MatthewDotCare.XStatic.Models;
using MatthewDotCare.XStatic.Repositories;
using Microsoft.AspNetCore.Hosting;
using Umbraco.Cms.Core.Web;

namespace MatthewDotCare.XStatic.AutoPublish
{
    public class DefaultAutoPublisher : IAutoPublisher
    {
        private readonly ISitesRepository _sitesRepository;
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly IStaticSiteStorer _storer;
        private readonly IDeployerService _deployerService;
        private readonly IExportTypeService _exportTypeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IActionFactory _actionFactory;

        public DefaultAutoPublisher(ISitesRepository sitesRepository,
            IUmbracoContextFactory umbracoContextFactory,
            IDeployerService deployerService,
            IStaticSiteStorer storer,
            IExportTypeService exportTypeService,
            IWebHostEnvironment webHostEnvironment,
            IActionFactory actionFactory)
        {
            _sitesRepository = sitesRepository;
            _umbracoContextFactory = umbracoContextFactory;
            _storer = storer;
            _deployerService = deployerService;
            _exportTypeService = exportTypeService;
            _webHostEnvironment = webHostEnvironment;
            _actionFactory = actionFactory;
        }

        public void RunAutoPublish(IEnumerable<Umbraco.Cms.Core.Models.IContent> publishedEntities)
        {
            var autoPublishSites = _sitesRepository.GetAutoPublishSites();

            var sitesToDeploy = new List<ExtendedGeneratedSite>();

            foreach (var publishedItem in publishedEntities)
            {
                foreach (var site in autoPublishSites)
                {
                    if (sitesToDeploy.Contains(site))
                    {
                        continue;
                    }

                    if (publishedItem.Path.Contains($",{site.RootNode},") || publishedItem.Path.EndsWith($",{site.RootNode}"))
                    {
                        sitesToDeploy.Add(site);
                    }
                }
            }

            var process = new RebuildProcess(_umbracoContextFactory, _exportTypeService, _sitesRepository, _webHostEnvironment, _actionFactory);
            var deployProcess = new DeployProcess(_storer, _deployerService, _sitesRepository);

            foreach (var site in sitesToDeploy)
            {
                Task.Run(async () =>
                {
                    await process.RebuildSite(site.Id);
                    await deployProcess.DeployStaticSite(site.Id);
                });
            }
        }
    }
}