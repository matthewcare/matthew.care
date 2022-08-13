﻿using System.Diagnostics;
using MatthewDotCare.XStatic.Actions;
using MatthewDotCare.XStatic.Generator.Db;
using MatthewDotCare.XStatic.Generator.ExportTypes;
using MatthewDotCare.XStatic.Generator.Jobs;
using MatthewDotCare.XStatic.Generator.Storage;
using MatthewDotCare.XStatic.Helpers;
using MatthewDotCare.XStatic.Repositories;
using Microsoft.AspNetCore.Hosting;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Web;

namespace MatthewDotCare.XStatic.Generator.Processes
{
    public class RebuildProcess
    {
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly IExportTypeService _exportTypeService;
        private ISitesRepository _sitesRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IActionFactory _actionFactory;

        public RebuildProcess(IUmbracoContextFactory umbracoContextFactory,
            IExportTypeService exportTypeService,
            ISitesRepository repo,
            IWebHostEnvironment webHostEnvironment,
            IActionFactory actionFactory)
        {
            _umbracoContextFactory = umbracoContextFactory;
            _exportTypeService = exportTypeService;
            _sitesRepo = repo;
            _webHostEnvironment = webHostEnvironment;
            _actionFactory = actionFactory;
        }

        public async Task<string> RebuildSite(int staticSiteId)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var entity = _sitesRepo.Get<SiteConfig>(staticSiteId);

            if (entity?.ExportFormat == null)
            {
                throw new XStaticException("Site not found with id " + staticSiteId);
            }

            using (var umbracoContext = _umbracoContextFactory.EnsureUmbracoContext())
            {
                IFileNameGenerator fileNamer = _exportTypeService.GetFileNameGenerator(entity.ExportFormat);

                int rootNodeId = entity.RootNode;
                var rootNode = umbracoContext.UmbracoContext.Content.GetById(rootNodeId);

                var builder = new JobBuilder(entity.Id, fileNamer)
                    .AddPageWithDescendants(rootNode);

                AddMediaToBuilder(entity, umbracoContext, builder);
                AddMediaCropsToBuilder(entity, builder);

                AddAssetsToBuilder(entity, builder);

                var listFactory = _exportTypeService.GetTransformerListFactory(entity.ExportFormat);
                var transformers = listFactory.BuildTransformers(entity);

                if (transformers.Any())
                {
                    builder.AddTransformers(transformers);
                }

                var results = await GetResults(entity, builder);

                var postActionResults = await RunPostActions(entity);
                results.AddRange(postActionResults.Select(r => r.WasSuccessful + " - " + r.Message));

                stopwatch.Stop();

                _sitesRepo.UpdateLastRun(staticSiteId, (int)(stopwatch.ElapsedMilliseconds / 1000));

                return string.Join(Environment.NewLine, results);
            }
        }

        private async Task<List<string>> GetResults(SiteConfig entity, JobBuilder builder)
        {
            var results = new List<string>();

            var generator = _exportTypeService.GetGenerator(entity.ExportFormat);

            if (generator == null)
            {
                throw new Exception("Export format not supported");
            }

            var job = builder.Build();
            var runner = new JobRunner(generator);
            results.AddRange(await runner.RunJob(job));

            return results;
        }

        private void AddAssetsToBuilder(SiteConfig entity, JobBuilder builder)
        {
            if (!string.IsNullOrEmpty(entity.AssetPaths))
            {
                var splitPaths = entity.AssetPaths.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim());
                var rootPath = _webHostEnvironment.WebRootPath;

                foreach (var path in splitPaths)
                {
                    var absolutePath = FileHelpers.PathCombine(rootPath, path);

                    if (path.Contains("?") || path.Contains("*"))
                    {
                        var trimmedPath = path.TrimStart(new[] { '\\', '/' });

                        var directory = new DirectoryInfo(rootPath);
                        var files = directory.GetFiles(trimmedPath, SearchOption.AllDirectories);

                        builder.AddAssetFiles(files.Select(f => "/" + FileHelpers.GetRelativePath(rootPath, f.FullName)));
                    }
                    else if (Directory.Exists(absolutePath))
                    {
                        builder.AddAssetFolder(path);
                    }
                    else if (System.IO.File.Exists(absolutePath))
                    {
                        builder.AddAssetFile(path);
                    }
                    else
                    {
                        // Invalid file.
                    }
                }
            }
        }

        private static void AddMediaToBuilder(SiteConfig entity, UmbracoContextReference umbracoContext, JobBuilder builder)
        {
            if (!string.IsNullOrEmpty(entity.MediaRootNodes))
            {
                var mediaRoots = entity.MediaRootNodes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var mediaRoot in mediaRoots)
                {
                    int mediaId;

                    if (int.TryParse(mediaRoot, out mediaId))
                    {
                        var rootMedia = umbracoContext.UmbracoContext.Media.GetById(mediaId);

                        if (rootMedia != null)
                        {
                            builder.AddMediaWithDescendants(rootMedia);
                        }
                    }
                }
            }
        }

        private void AddMediaCropsToBuilder(SiteConfig entity, JobBuilder builder)
        {
            if (string.IsNullOrEmpty(entity.ImageCrops))
            {
                return;
            }

            var crops = Crop.GetCropsFromCommaDelimitedString(entity.ImageCrops);
            builder.AddMediaCrops(crops);
        }

        private async Task<IEnumerable<XStaticResult>> RunPostActions(SiteConfig entity)
        {
            var actions = _actionFactory.CreateConfiguredPostGenerationActions(entity.PostGenerationActionIds.ToArray());
            var results = new List<XStaticResult>();

            foreach(var action in actions)
            {
                try
                {
                    var result = await action.Action.RunAction(entity.Id, action.Config);
                    results.Add(result);
                }
                catch (Exception e)
                {
                    results.Add(XStaticResult.Error($"Error thrown in RunPostActions for {action?.Action?.GetType()?.Name} - {e.Message}", e));
                }
            }

            return results;
        }
    }
}