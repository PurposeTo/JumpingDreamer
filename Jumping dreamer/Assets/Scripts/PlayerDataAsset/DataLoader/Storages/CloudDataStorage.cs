using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using System.Text;
using System.Collections;
using Desdiene.Coroutine.CoroutineExecutor;
using Desdiene.SuperMonoBehaviourAsset;

public class CloudDataStorage : DataStorageOld, IDataReloader
{
    private ICoroutineContainer loadDataInfo;


    public CloudDataStorage(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour)
    {
        loadDataInfo = this.superMonoBehaviour.CreateCoroutineContainer();
    }


    private ISavedGameMetadata currentGameMetadata;
    private DateTime startPlayingTime;

    private ISavedGameClient SavedGameClient => ((PlayGamesPlatform)Social.Active).SavedGame;


    void IDataReloader.LoadDataAgain()
    {
        ReadSavedGame(receivedData => data = receivedData);
    }


    private protected override void ReadFromStorage(Action<PlayerGameData> callback)
    {
        superMonoBehaviour.ExecuteCoroutineContinuously(loadDataInfo, LoadDataEnumerator(callback));
    }


    private protected override void WriteToStorage(PlayerGameData data)
    {
        CreateSavedGame(data);
    }


    private IEnumerator LoadDataEnumerator(Action<PlayerGameData> callback)
    {
        yield return new WaitUntil(() => GPGSAuthentication.IsAuthenticated);

        // Начать отсчет времени для текущей сессии игры
        startPlayingTime = DateTime.Now;

        // Загрузка данных из облака
        ReadSavedGame(cloudData =>
        {
            data = cloudData;

            Debug.Log("Чтение данных с облака завершено.");
        });

        yield return new WaitWhile(() => data == null);
        callback?.Invoke(data);
    }


    private void OpenSavedGame(Action<SavedGameRequestStatus, ISavedGameMetadata> OnSavedGameOpened)
    {
        if (!GPGSAuthentication.IsAuthenticated)
        {
            OnSavedGameOpened(SavedGameRequestStatus.AuthenticationError, null);
            return;
        }

        SavedGameClient.OpenWithAutomaticConflictResolution(DataModel.FileNameWithExtension,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            OnSavedGameOpened);
    }


    private void ReadSavedGame(Action<PlayerGameData> callback)
    {
        OpenSavedGame((gameRequestStatus, gameMetadata) =>
        {
            Debug.Log("Данные с облака были открыты со статусом " + gameRequestStatus);

            if (gameRequestStatus == SavedGameRequestStatus.Success)
            {
                // Получаем метаданные открытого файла
                currentGameMetadata = gameMetadata;

                SavedGameClient.ReadBinaryData(gameMetadata, (readingStatus, data) => OnSavedGameWasReaded(readingStatus, data, callback));
            }
        });
    }


    private void OnSavedGameWasReaded(SavedGameRequestStatus readingStatus, byte[] data, Action<PlayerGameData> callback)
    {
        Debug.Log($"Данные с облака были извлечены со статусом " + readingStatus);

        PlayerGameData cloudData = null;

        if (readingStatus == SavedGameRequestStatus.Success)
        {
            if (data != null)
            {
                Debug.Log($"Длина извлеченного массива байт = { data.Length }.\nДанные в виде строки: " + Encoding.UTF8.GetString(data));

                cloudData = DataConverter.ToObject(Encoding.UTF8.GetString(data), out bool isSuccess, out Exception exception);

                if (!isSuccess) Debug.LogError("Ошибка десериализации данных с облака " + exception);
                else callback?.Invoke(cloudData);
            }
            else Debug.LogError("Данные на облаке не были найдены. Если даже на облаке нет данных, то возвращается пустой массив байт. Так что этот блок не должен выполняться.");
        }

        Debug.Log($"Данные, считанные с облака: {cloudData}.\nСтатус чтения: {readingStatus}.");
    }


    private void CreateSavedGame(IDataGetter data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        string json = DataConverter.ToJson(data, out bool isSerializationSuccess, out Exception _);
        if (!isSerializationSuccess) return;
        byte[] dataToSave = Encoding.UTF8.GetBytes(json);

        if (currentGameMetadata == null)
        {
            OpenSavedGame((gameRequestStatus, gameMetadata) =>
            {
                if (gameRequestStatus == SavedGameRequestStatus.Success)
                {
                    // Получаем метаданные открытого файла
                    currentGameMetadata = gameMetadata;

                    SaveData(dataToSave);
                }
                else return;
            });

            return;
        }

        SaveData(dataToSave);
    }


    private void SaveData(byte[] dataToSave)
    {
        TimeSpan allPlayingTime = DateTime.Now - startPlayingTime;
        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

        builder = builder.WithUpdatedPlayedTime(currentGameMetadata.TotalTimePlayed + allPlayingTime).WithUpdatedDescription("Saved game at " + DateTime.Now);

        SavedGameMetadataUpdate updatedMetadata = builder.Build();
        SavedGameClient.CommitUpdate(currentGameMetadata, updatedMetadata, dataToSave, OnSavedGameCreated);
    }


    private void OnSavedGameCreated(SavedGameRequestStatus gameRequestStatus, ISavedGameMetadata gameMetadata)
    {
        if (gameRequestStatus == SavedGameRequestStatus.Success)
        {
            // Так как при сохранении метаданные обновляются, то после его завершения необходимо их перезаписать
            currentGameMetadata = gameMetadata;

            // Заново считаем время игры с момента записи сохранения
            startPlayingTime = DateTime.Now;
        }
    }
}
