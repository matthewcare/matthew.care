using MatthewDotCare.XStatic.Generator.ExportTypes;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace MatthewDotCare.XStatic.Generator.Db
{
    [TableName(TableName)]
    [PrimaryKey("Id")]
    public class ExportTypeDataModel : IExportTypeFields
    {
        public const string TableName = "XStaticExportTypes";

        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string TransformerFactory { get; set; }

        public string Generator { get; set; }

        public string FileNameGenerator { get; set; }
    }
}