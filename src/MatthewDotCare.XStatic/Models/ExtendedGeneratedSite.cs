﻿using MatthewDotCare.XStatic.Generator.Db;

namespace MatthewDotCare.XStatic.Models
{
    public class ExtendedGeneratedSite : SiteConfig
    {
        public string RootPath { get; set; }

        public string FolderSize { get; set; }

        public string ExportTypeName { get; set; }

        public string LastRunString => LastDeployed?.ToString("hh:mm dd MMM yy");

        public string LastDeployedString => LastDeployed?.ToString("hh:mm dd MMM yy");
    }
}
