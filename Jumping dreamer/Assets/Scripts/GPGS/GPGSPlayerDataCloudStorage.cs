using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using System.Collections;
using System.Text;

public class GPGSPlayerDataCloudStorage : SingletonMonoBehaviour<GPGSPlayerDataCloudStorage>
{
    public ISavedGameMetadata CurrentGameMetadata { get; private set; }
    public DateTime StartPlayingTime { get; private set; }

    private ISavedGameClient SavedGameClient => ((PlayGamesPlatform)Social.Active).SavedGame;


    private void Start()
    {
        StartCoroutine(LoadSavedGameFromCloudEnumerator());
    }


    public void CreateSave(PlayerDataModel localModelToSaveOnCloud)
    {
        Debug.Log("#CreateSave: begin");

        bool isSerializationSuccess = false;
        string json = JsonConverterWrapper.SerializeObject(localModelToSaveOnCloud, (success, exception) => isSerializationSuccess = success);

        if (!isSerializationSuccess || !PlayerDataModelController.Instance.IsDataFileLoaded)
        {
            return;
        }

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
                else { return; }
            });

            return;
        }

        Debug.Log("#CreateSave: CurrentGameMetadata != null");

        SavePlayerData();
    }


    public void ReadSavedGame(Action<PlayerDataModel, SavedGameRequestStatus> action)
    {
        OpenSavedGame((gameRequestStatus, gameMetadata) =>
        {
            Debug.Log("Данные с облака были открыты со статусом " + gameRequestStatus);

            if (gameRequestStatus == SavedGameRequestStatus.Success)
            {
                // Получаем метаданные открытого файла
                CurrentGameMetadata = gameMetadata;

                SavedGameClient.ReadBinaryData(gameMetadata, (readingCloudDataStatus, data) =>
                {
                    Debug.Log($"Данные с облака были извлечены со статусом " + readingCloudDataStatus);

                    PlayerDataModel cloudModel = null;

                    if (readingCloudDataStatus == SavedGameRequestStatus.Success)
                    {
                        if (data != null)
                        {
                            Debug.Log($"Длина извлеченного массива байт = { data.Length }.\nДанные в виде строки: " + Encoding.UTF8.GetString(data));

                            cloudModel = JsonConverterWrapper.DeserializeObject(Encoding.UTF8.GetString(data), (isSuccess, exception) =>
                            {
                                if (!isSuccess)
                                {
                                    Debug.LogError("Ошибка десериализации данных с облака " + exception);
                                }
                            });
                        }
                        else { Debug.LogError("Данные на облаке не были найдены. Если даже на облаке нет данных, то возвращается пустой массив байт. Так что этот блок не должен выполняться."); }
                    }
                    else
                    {
                        // handle error
                    }

                    Debug.Log($"Received from cloud model: {cloudModel}. Reading status: {readingCloudDataStatus}.");
                    action?.Invoke(cloudModel, readingCloudDataStatus);
                });
            }
            else { return; }
        });
    }


    public void DeleteSavedGame(Action action)
    {
        OpenSavedGame((gameRequestStatus, gameMetadata) =>
        {
            if (gameRequestStatus == SavedGameRequestStatus.Success)
            {
                SavedGameClient.Delete(gameMetadata);

                CurrentGameMetadata = null;
            }
            else { return; }

            action?.Invoke();
        });
    }


    private void OpenSavedGame(Action<SavedGameRequestStatus, ISavedGameMetadata> OnSavedGameOpened)
    {
        if (!GPGSAuthentication.IsAuthenticated)
        {
            OnSavedGameOpened(SavedGameRequestStatus.AuthenticationError, null);
            return;
        }

        print("Opening!");
        SavedGameClient.OpenWithManualConflictResolution(PlayerDataModel.FileName, DataSource.ReadCacheOrNetwork, true, new ConflictCallback((conflictResolver, originGameMetadata, originData, unmergedGameMetadata, unmergedData) =>
        {
            print("OpenWithManualConflictResolution");
            print($"Origin data: {originData}");
            print($"Unmerged data: {unmergedData}");
        }), OnSavedGameOpened);

        //SavedGameClient.OpenWithAutomaticConflictResolution(PlayerDataModel.FileName,
        //    DataSource.ReadCacheOrNetwork,                                              // ?
        //    ConflictResolutionStrategy.UseLongestPlaytime,                              // ?
        //    OnSavedGameOpened);
    }


    private IEnumerator LoadSavedGameFromCloudEnumerator()
    {
        yield return new WaitUntil(() => GPGSAuthentication.IsAuthenticated);
        yield return new WaitUntil(() => PlayerDataModelController.Instance.IsDataFileLoaded);

        // Начать отсчет времени для текущей сессии игры
        StartPlayingTime = DateTime.Now;

        // Загрузка данных из облака
        ReadSavedGame((cloudModel, readingStatus) =>
        {
            Debug.Log("###ДЕСЕРИАЛИЗАЦИЯ ДАННЫХ С ОБЛАКА ЗАВЕРШЕНА. РЕЗУЛЬТАТ: " + cloudModel);

            PlayerDataModelController.Instance.SynchronizePlayerDataStorages(cloudModel);
        });
    }


    private void OnSaveCreated(SavedGameRequestStatus gameRequestStatus, ISavedGameMetadata gameMetadata)
    {
        if (gameRequestStatus == SavedGameRequestStatus.Success)
        {
            // handle writing of saved game.

            // Так как при сохранении метаданные обновляются, то после его завершения необходимо их перезаписать
            CurrentGameMetadata = gameMetadata;

            // Заново считаем время игры с момента записи сохранения
            StartPlayingTime = DateTime.Now;
        }
        else
        {
            // handle error
        }
    }


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
}
