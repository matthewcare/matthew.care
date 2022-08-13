using MatthewDotCare.XStatic.Generator.Db;

namespace MatthewDotCare.XStatic.Generator.ExportTypes
{
    public interface IExportTypeRepository
    {
        ExportTypeDataModel Create(ExportTypeDataModel data);

        void Delete(int id);

        IEnumerable<ExportTypeDataModel> GetAll();

        ExportTypeDataModel Get(int dbId);

        ExportTypeDataModel Update(ExportTypeDataModel update);
    }
}