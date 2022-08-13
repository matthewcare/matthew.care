﻿using System.IO.Compression;
using MatthewDotCare.XStatic.Generator.Db;
using MatthewDotCare.XStatic.Generator.Storage;
using MatthewDotCare.XStatic.Repositories;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace MatthewDotCare.XStatic.Controllers
{
    [PluginController("xstatic")]
    public class DownloadController : UmbracoAuthorizedApiController
    {
        private readonly IStaticSiteStorer _storer;
        private ISitesRepository _sitesRepo;

        public DownloadController(IStaticSiteStorer storer, ISitesRepository sitesRepo)
        {
            _storer = storer;
            _sitesRepo = sitesRepo;
        }

        [HttpGet]
        public FileStreamResult DownloadStaticSite(int staticSiteId)
        {
            var entity = _sitesRepo.Get<SiteConfig>(staticSiteId);

            if (entity == null)
            {
                throw new XStaticException("Site not found with id " + staticSiteId);
            }

            var localFolderPath = _storer.GetStorageLocationOfSite(entity.Id);

            if (!Directory.Exists(localFolderPath))
            {
                throw new FileNotFoundException();
            }

            var localZipFilePath = localFolderPath.Trim('/').Trim('\\') + ".zip";

            if (System.IO.File.Exists(localZipFilePath))
            {
                System.IO.File.Delete(localZipFilePath);
            }

            ZipFile.CreateFromDirectory(localFolderPath, localZipFilePath);

            var fileName = $"xStatic site download {staticSiteId}.zip";
            var mimeType = "application/zip";
            Stream stream = new FileStream(localZipFilePath, FileMode.Open, FileAccess.Read);

            return new FileStreamResult(stream, mimeType)
            {
                FileDownloadName = fileName
            };
        }
    }
}