﻿using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace MatthewDotCare.XStatic.Generator.Db
{
    [TableName(TableName)]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class SiteConfig : ISiteConfig
    {
        public const string TableName = "XStaticSiteConfigs";

        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool AutoPublish { get; set; }

        public int RootNode { get; set; }

        public string MediaRootNodes { get; set; }

        public int ExportFormat { get; set; }

        public DateTime? LastRun { get; set; }

        public int? LastBuildDurationInSeconds { get; set; }

        public DateTime? LastDeployed { get; set; }

        public int? LastDeployDurationInSeconds { get; set; }

        public string AssetPaths { get; set; }

        public string TargetHostname { get; set; }

        public string ImageCrops { get; set; }

        [SerializedColumn]
        public DeploymentTargetModel DeploymentTarget { get; set; }

        [SerializedColumn]
        public List<int> PostGenerationActionIds { get; set; }

        public IDeploymentTarget GetDeploymentTarget()
        {
            return DeploymentTarget;
        }
    }

    public class DeploymentTargetModel : IDeploymentTarget
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> Fields { get; set; }
    }
}
