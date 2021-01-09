using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using System.Collections;
using System.Text;

public class GPGSPlayerDataCloudStorage : SuperMonoBehaviourContainer
{
    private ICoroutineContainer loadSavedGameInfo;


    public GPGSPlayerDataCloudStorage(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour)
    {
        loadSavedGameInfo = this.superMonoBehaviour.CreateCoroutineContainer();
    }


    public bool IsDataLoading => loadSavedGameInfo.IsExecuting;
    public PlayerModelData Data { get; private set; } = null;
    public ISavedGameMetadata CurrentGameMetadata { get; private set; }
    public DateTime StartPlayingTime { get; private set; }

    private ISavedGameClient SavedGameClient => ((PlayGamesPlatform)Social.Active).SavedGame;


    public void SaveData(PlayerModelData modelData)
    {
        Debug.Log("#CreateSave: begin");

        if (modelData == null) throw new ArgumentNullException(nameof(modelData));

        string json = JsonConverterWrapper.SerializeObject(modelData, out bool isSerializationSuccess, out Exception exception);
        if (!isSerializationSuccess) return;

        byte[] dataToSave = Encoding.UTF8.GetBytes(json);

        Debug.Log("#CreateSave: after checking");


        void SavePlayerData()
        {
            TimeSpan allPlayingTime = DateTime.Now - StartPlayingTime;
            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

            builder = builder.WithUpdatedPlayedTime(CurrentGameMetadata.TotalTimePlayed + allPlayingTime).WithUpdatedDescription("Saved game at " + DateTime.Now);

            SavedGameMetadataUpdate updatedMetadata = builder.Build();
            SavedGameClient.CommitUpdate(CurrentGameMetadata, updatedMetadata, dataToSave, OnSaveCreated);
        }


        if (CurrentGameMetadata == null)
        {
            Debug.Log("#CreateSave: CurrentGameMetadata == null");

            OpenSavedGame((gameRequestStatus, gameMetadata) =>
            {
                if (gameRequestStatus == SavedGameRequestStatus.Success)
                {
                    // Получаем метаданные открытого файла
                    CurrentGameMetadata = gameMetadata;
                    SavePlayerData();
                }
                else return;
            });

            return;
        }

        Debug.Log($"#CreateSave: CurrentGameMetadata != null\nCurrentGameMetadata = {CurrentGameMetadata}");

        SavePlayerData();
    }


    public void ReadData(Action<PlayerModelData, SavedGameRequestStatus> action)
    {
        OpenSavedGame((gameRequestStatus, gameMetadata) =>
        {
            Debug.Log("Данные с облака были открыты со статусом " + gameRequestStatus);

            if (gameRequestStatus == SavedGameRequestStatus.Success)
            {
                // Получаем метаданные открытого файла
                CurrentGameMetadata = gameMetadata;

                SavedGameClient.ReadBinaryData(gameMetadata, (readingStatus, data) =>
                {
                    Debug.Log($"Данные с облака были извлечены со статусом " + readingStatus);

                    PlayerModelData cloudData = null;

                    if (readingStatus == SavedGameRequestStatus.Success)
                    {
                        if (data != null)
                        {
                            Debug.Log($"Длина извлеченного массива байт = { data.Length }.\nДанные в виде строки: " + Encoding.UTF8.GetString(data));

                            cloudData = JsonConverterWrapper.DeserializeObject(Encoding.UTF8.GetString(data), 
                                out bool isSuccess, out Exception exception);

                            if (!isSuccess) Debug.LogError("Ошибка десериализации данных с облака " + exception);
                        }
                        else Debug.LogError("Данные на облаке не были найдены. Если даже на облаке нет данных, то возвращается пустой массив байт. Так что этот блок не должен выполняться.");
                    }

                    // todo: заменить сериализацию на cloudData.toString();
                    Debug.Log($"Received cloud model: {cloudData}.\nCloud model as json: {JsonConverterWrapper.SerializeObject(cloudData, out bool _1, out Exception _2)}\nReading status: {readingStatus}.");

                    action?.Invoke(cloudData, readingStatus);
                });
            }
            else action?.Invoke(null, gameRequestStatus);
        });
    }


    public void StartOpeningGameSession()
    {
        superMonoBehaviour.ExecuteCoroutineContinuously(ref loadSavedGameInfo, OpenGameSessionAndReadDataEnumerator());
    }


    private IEnumerator OpenGameSessionAndReadDataEnumerator()
    {
        yield return new WaitUntil(() => GPGSAuthentication.IsAuthenticated);

        // Начать отсчет времени для текущей сессии игры
        StartPlayingTime = DateTime.Now;

        bool isDataWasReceived = false;

        // Загрузка данных из облака
        ReadData((cloudModel, readingStatus) =>
        {
            Debug.Log($"#Десериализация данных с облака завершена.");

            Data = cloudModel;
            isDataWasReceived = true;
        });

        // Необходимо закончить выполнение корутины после извлечения данных из облака.
        yield return new WaitUntil(() => isDataWasReceived);
    }


    private void OpenSavedGame(Action<SavedGameRequestStatus, ISavedGameMetadata> OnSavedGameOpened)
    {
        if (!GPGSAuthentication.IsAuthenticated)
        {
            OnSavedGameOpened(SavedGameRequestStatus.AuthenticationError, null);
            return;
        }

        SavedGameClient.OpenWithAutomaticConflictResolution(PlayerModel.FileNameWithExtension,
            DataSource.ReadNetworkOnly,
            ConflictResolutionStrategy.UseLongestPlaytime,
            OnSavedGameOpened);
    }


    private void OnSaveCreated(SavedGameRequestStatus gameRequestStatus, ISavedGameMetadata gameMetadata)
    {
        if (gameRequestStatus == SavedGameRequestStatus.Success)
        {
            // Так как при сохранении метаданные обновляются, то после его завершения необходимо их перезаписать
            CurrentGameMetadata = gameMetadata;

            // Заново считаем время игры с момента записи сохранения
            StartPlayingTime = DateTime.Now;
        }
    }


    #region GetScreenshot
    // Необходимо дождаться конца кадра, чтобы сделать скриншот. (Рендер изображения должен браться не из буфера устройства)
    [Obsolete]
    private Texture2D GetScreenshot()
    {
        // Create a 2D texture that is 1024x700 pixels from which the PNG will be extracted
        Texture2D screenShot = new Texture2D(1024, 700);

        // Takes the screenshot from top left hand corner of screen and maps to top left hand corner of screenShot texture
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, (Screen.width / 1024) * 700), 0, 0);

        return screenShot;
    }
    #endregion
}
