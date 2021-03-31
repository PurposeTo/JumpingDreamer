using Desdiene.GameDataAsset;
using Desdiene.GameDataAsset.DataLoader;
using Desdiene.GameDataAsset.Model;
using Desdiene.SuperMonoBehaviourAsset;

public class Test : SuperMonoBehaviour
{
    private void Start()
    {
        //Задание T
        ReaderWriter<GameData> readerWriter = 
            DataStoragesReaderWriterFactory.GetReaderWriter(this, new DataStorage<GameData>("Тестовое хранилище", this));

        GameData gameData;
        readerWriter.Read(x => gameData = x);
        readerWriter.Read(x => gameData = x);
    }
}
