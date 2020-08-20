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
    public const string CloudFilePath = "GameData.json";

    private PlayGamesPlatform GamePlatform => (PlayGamesPlatform)Social.Active;
    private ISavedGameClient SavedGameClient => GamePlatform.SavedGame;


    private void Start()
    {
        StartCoroutine(LoadSavedGameFromCloudEnumerator());
    }


    private IEnumerator LoadSavedGameFromCloudEnumerator()
    {
        yield return new WaitUntil(() => GPGSAuthentication.IsAuthenticated);
        yield return new WaitUntil(() => PlayerDataStorageSafe.Instance.IsDataFileLoaded);

        // Загрузка данных из облака
        ReadSavedGame(CloudFilePath);
    }


    #region Selection menu
    //public void ShowSavedGamesSelectMenu()
    //{
    //    uint maxSavesNumberToDisplay = 5;
    //    bool allowCreateSave = false;
    //    bool allowDeleteSave = true;

    //    SavedGameClient.ShowSelectSavedGameUI("Select saved game",
    //        maxSavesNumberToDisplay,
    //        allowCreateSave,
    //        allowDeleteSave,
    //        OnSavedGameSelected);
    //}


    //private void OnSavedGameSelected(SelectUIStatus selectUIStatus, ISavedGameMetadata gameMetadata)
    //{
    //    if (selectUIStatus == SelectUIStatus.SavedGameSelected)
    //    {
    //        // handle selected game save
    //
    //        // Чтение данных или удаление?
    //    }
    //    else
    //    {
    //        // handle cancel or error
    //    }
    //}
    #endregion


    private void OpenSavedGame(string fileName, Action<SavedGameRequestStatus, ISavedGameMetadata> OnSavedGameOpened)
    {
        // need?
        if (!GPGSAuthentication.IsAuthenticated)
        {
            OnSavedGameOpened?.Invoke(SavedGameRequestStatus.AuthenticationError, null);
            return;
        }

        // Also conflict can be resolved by OpenWithManualConflictResolution()
        SavedGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,                                              // ?
            ConflictResolutionStrategy.UseLongestPlaytime,                              // ?
            OnSavedGameOpened);
    }


    public void CreateSave(byte[] dataToSave)
    {
        // Если есть доступ к облако, а не была ли пройдена аутентификация !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (!GPGSAuthentication.IsAuthenticated || dataToSave == null || !PlayerDataStorageSafe.Instance.IsDataFileLoaded) // data.Length == 0
        {
            return;
        }

        // AH - video !!!!!!!!!!!!!!!!
        OpenSavedGame(CloudFilePath, (gameRequestStatus, gameMetadata) =>
        {
            if (gameRequestStatus == SavedGameRequestStatus.Success)
            {
                TimeSpan allPlayingTime = DateTime.Now - StartPlayingTime;
                SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

                builder = builder.WithUpdatedPlayedTime(CurrentGameMetadata.TotalTimePlayed + allPlayingTime).WithUpdatedDescription("Saved game at " + DateTime.Now).WithUpdatedPngCoverImage(GetScreenshot().EncodeToPNG());

                SavedGameMetadataUpdate metadataUpdate = builder.Build();
                SavedGameClient.CommitUpdate(gameMetadata, metadataUpdate, dataToSave, OnSaveCreated);
            }
            else
            {
                // handle error
            }
        });
    }


    public void OnSaveCreated(SavedGameRequestStatus status, ISavedGameMetadata gameMetadata)
    {
        if (status == SavedGameRequestStatus.Success)
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


    public void ReadSavedGame(string fileName)
    {
        if (!GPGSAuthentication.IsAuthenticated)
        {
            return;
        }

        // TODO: Если файла на облаке не существует (сможем ли выполнить чтение?)
        OpenSavedGame(fileName, (gameRequestStatus, gameMetadata) =>
        {
            if (gameRequestStatus == SavedGameRequestStatus.Success)
            {
                SavedGameClient.ReadBinaryData(gameMetadata, OnSavedGameDataRead);

                // Получаем метаданные открытого файла
                CurrentGameMetadata = gameMetadata;
            }
            else
            {
                // handle error
            }
        });
    }


    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle processing the byte array data

            // Как только данные загружены, необходимо начать отсчет времени для текущей сессии игры
            StartPlayingTime = DateTime.Now;

            if (data == null) // data.Length == 0
            {
                // TODO: Загрузить данные на облако из локальной модели
            }

            PlayerDataModel cloudModel = JsonConverterWrapper.DeserializeObject(Encoding.UTF8.GetString(data), null);
            if (cloudModel != null)
            {
                // Смешение моделей(синхронизация данных)
                if (!GameManager.IsGameRunning)
                {
                    PlayerDataSynchronizer.MixModels(cloudModel, PlayerDataStorageSafe.Instance.PlayerDataModel);
                }
            }
        }
        else
        {
            // handle error
        }
    }


    public void DeleteSavedGame(string fileName)
    {
        if (!GPGSAuthentication.IsAuthenticated)
        {
            return;
        }

        OpenSavedGame(fileName, (gameRequestStatus, gameMetadata) =>
        {
            if (gameRequestStatus == SavedGameRequestStatus.Success)
            {
                SavedGameClient.Delete(gameMetadata);
            }
            else
            {
                // handle error
            }
        });
    }


    public Texture2D GetScreenshot()
    {
        // Create a 2D texture that is 1024x700 pixels from which the PNG will be extracted
        Texture2D screenShot = new Texture2D(1024, 700);

        // Takes the screenshot from top left hand corner of screen and maps to top left hand corner of screenShot texture
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, (Screen.width / 1024) * 700), 0, 0);

        return screenShot;
    }
}
