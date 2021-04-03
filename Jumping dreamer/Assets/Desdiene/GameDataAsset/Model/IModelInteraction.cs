using Desdiene.GameDataAsset.Data;

namespace Desdiene.GameDataAsset.Model
{
    public interface IModelInteraction<T> where T : GameData
    {
        T GetData();
        void SetData(T data);
    }
}
