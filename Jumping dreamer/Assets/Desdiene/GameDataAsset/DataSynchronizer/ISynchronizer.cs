namespace Desdiene.GameDataAsset.DataSynchronizer
{
    public interface ISynchronizer
    {
        void ReadDataFromStorage();

        void WriteDataToStorage();
    }
}
