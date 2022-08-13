using MatthewDotCare.XStatic.Generator.Storage;
using MatthewDotCare.XStatic.Generator.Transformers;

namespace MatthewDotCare.XStatic.Generator.ExportTypes
{
    public interface IExportTypeService
    {
        IEnumerable<IExportTypeFields> GetExportTypes();

        IGenerator GetGenerator(int exportFormatId);

        ITransformerListFactory GetTransformerListFactory(int exportFormatId);

        IFileNameGenerator GetFileNameGenerator(int exportFormatId);
    }
}