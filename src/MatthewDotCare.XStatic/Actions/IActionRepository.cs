using MatthewDotCare.XStatic.Actions.Db;

namespace MatthewDotCare.XStatic.Actions
{
    public interface IActionRepository
    {
        ActionDataModel Create(ActionDataModel data);
        void Delete(int id);
        ActionDataModel Get(int dbId);
        IEnumerable<ActionDataModel> GetAll();
        IEnumerable<ActionDataModel> GetAllInCategory(string category);
        ActionDataModel Update(ActionDataModel update);
    }
}