
namespace KrolStudio
{
    public interface IDataPersistor<out T, in TK> where T : IDataModel where TK : IDataModel
    {
        T GetDataModel();
        void UpdateModel(TK dataModel);
    }
}