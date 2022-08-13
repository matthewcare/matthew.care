using MatthewDotCare.XStatic.Actions.Db;
using MatthewDotCare.XStatic.Generator.Db;
using Umbraco.Cms.Infrastructure.Migrations;

namespace MatthewDotCare.XStatic.Db
{
    public class XStaticDatabaseMigrationPlan : MigrationPlan
    {
        public XStaticDatabaseMigrationPlan()
            : base("xStatic")
        {
            From(string.Empty)
                .To<MigrationCreateTable>("init")
                .To<MigrationCreateExportTypesTable>("Manage Export Types in DB")
                .To<MigrationCreateActionsTable>("Manage Actions in DB");
        }
    }

    public class MigrationCreateTable : MigrationBase
    {
        public MigrationCreateTable(IMigrationContext context)
            : base(context)
        {
        }

        protected override void Migrate()
        {
            if (!TableExists(SiteConfig.TableName))
            {
                var builder = Create.Table(SiteConfig.TableName)
                    .WithColumn("Id").AsInt16().Identity()
                    .WithColumn("Name").AsString()
                    .WithColumn("AutoPublish").AsBoolean()
                    .WithColumn("RootNode").AsString()
                    .WithColumn("MediaRootNodes").AsString().Nullable()
                    .WithColumn("ExportFormat").AsInt16()
                    .WithColumn("LastRun").AsDateTime().Nullable()
                    .WithColumn("LastBuildDurationInSeconds").AsInt16().Nullable()
                    .WithColumn("LastDeployed").AsDateTime().Nullable()
                    .WithColumn("LastDeployDurationInSeconds").AsInt16().Nullable()
                    .WithColumn("AssetPaths").AsString(1000).Nullable()
                    .WithColumn("DeploymentTarget").AsString(2500).Nullable()
                    .WithColumn("TargetHostname").AsString().Nullable()
                    .WithColumn("ImageCrops").AsString().Nullable()
                    .WithColumn("PostGenerationActionIds").AsString(200).Nullable(); ;

                builder.Do();
            }
        }
    }


    public class MigrationCreateExportTypesTable : MigrationBase
    {
        public MigrationCreateExportTypesTable(IMigrationContext context)
            : base(context)
        {
        }

        protected override void Migrate()
        {

            if (!TableExists(ExportTypeDataModel.TableName))
            {
                var builder = Create.Table(ExportTypeDataModel.TableName)
                    .WithColumn("Id").AsInt16().Identity()
                    .WithColumn("Name").AsString(100)
                    .WithColumn("TransformerFactory").AsString(500).Nullable()
                    .WithColumn("Generator").AsString(500).Nullable()
                    .WithColumn("FileNameGenerator").AsString(500).Nullable();

                builder.Do();
            }

            Insert.IntoTable(ExportTypeDataModel.TableName).Row(new
            {
                Name = "HTML Website",
                TransformerFactory = "MatthewDotCare.XStatic.Generator.Transformers.DefaultHtmlTransformerListFactory, MatthewDotCare.XStatic",
                Generator = "MatthewDotCare.XStatic.Generator.StaticHtmlSiteGenerator, MatthewDotCare.XStatic",
                FileNameGenerator = "MatthewDotCare.XStatic.Generator.Storage.EverythingIsIndexHtmlFileNameGenerator, MatthewDotCare.XStatic"
            }).Do();
        }
    }

    public class MigrationCreateActionsTable : MigrationBase
    {
        public MigrationCreateActionsTable(IMigrationContext context)
            : base(context)
        {
        }

        protected override void Migrate()
        {
            if (!TableExists(ActionDataModel.TableName))
            {
                var builder = Create.Table(ActionDataModel.TableName)
                    .WithColumn("Id").AsInt16().Identity()
                    .WithColumn("Name").AsString(100)
                    .WithColumn("Category").AsString(100)
                    .WithColumn("Type").AsString(500).Nullable()
                    .WithColumn("Config").AsString(2500).Nullable();

                builder.Do();
            }
        }
    }
}