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

    private ISavedGameClient SavedGameClient => ((PlayGamesPlatform)Social.Active).SavedGame;


    private void Start()
    {
        StartCoroutine(LoadSavedGameFromCloudEnumerator());
    }


    private IEnumerator LoadSavedGameFromCloudEnumerator()
    {
        yield return new WaitUntil(() => GPGSAuthentication.IsAuthenticated);

        // Как только выполнена аутентификация, необходимо начать отсчет времени для текущей сессии игры
        StartPlayingTime = DateTime.Now;

        yield return new WaitUntil(() => PlayerDataLocalStorageSafe.Instance.IsDataFileLoaded);

        // Загрузка данных из облака
        ReadSavedGame(CloudFilePath);
    }


    private void OpenSavedGame(string fileName, Action<SavedGameRequestStatus, ISavedGameMetadata> OnSavedGameOpened)
    {
        if (!GPGSAuthentication.IsAuthenticated)
        {
            return;
        }

        SavedGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,                                              // ?
            ConflictResolutionStrategy.UseLongestPlaytime,                              // ?
            OnSavedGameOpened);
    }


    public void CreateSave(byte[] dataToSave)
    {
        Debug.Log("#CreateSave: begin");

        if (!GPGSAuthentication.IsAuthenticated ||
            !InternetConnectionChecker.Instance.IsInternetConnectionAvaliable() ||
            dataToSave == null ||
            dataToSave.Length == 0 ||
            !PlayerDataLocalStorageSafe.Instance.IsDataFileLoaded)
        {
            return;
        }
        

        Debug.Log("#CreateSave: after checking");


        void SavePlayerData()
        {
            TimeSpan allPlayingTime = DateTime.Now - StartPlayingTime;
            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

            builder = builder.WithUpdatedPlayedTime(CurrentGameMetadata.TotalTimePlayed + allPlayingTime).WithUpdatedDescription("Saved game at " + DateTime.Now).WithUpdatedPngCoverImage(GetScreenshot().EncodeToPNG());

            SavedGameMetadataUpdate updatedMetadata = builder.Build();
            SavedGameClient.CommitUpdate(CurrentGameMetadata, updatedMetadata, dataToSave, OnSaveCreated);
        }


        if (CurrentGameMetadata == null)
        {
            Debug.Log("#CreateSave: CurrentGameMetadata == null");

            OpenSavedGame(CloudFilePath, (gameRequestStatus, gameMetadata) =>
            {
                if (gameRequestStatus == SavedGameRequestStatus.Success)
                {
                    // Получаем метаданные открытого файла
                    CurrentGameMetadata = gameMetadata;

                    SavePlayerData();
                }
                else
                {
                    // handle error
                }
            });

            return;
        }

        Debug.Log("#CreateSave: CurrentGameMetadata != null");

        SavePlayerData();
    }


    private void OnSaveCreated(SavedGameRequestStatus status, ISavedGameMetadata gameMetadata)
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
        if (!GPGSAuthentication.IsAuthenticated || !InternetConnectionChecker.Instance.IsInternetConnectionAvaliable())
        {
            return;
        }

        OpenSavedGame(fileName, (gameRequestStatus, gameMetadata) =>
        {
            Debug.Log("Данные с облака были открыты со статусом " + gameRequestStatus);

            if (gameRequestStatus == SavedGameRequestStatus.Success)
            {
                // Получаем метаданные открытого файла
                CurrentGameMetadata = gameMetadata;

                SavedGameClient.ReadBinaryData(gameMetadata, OnSavedGameDataRead);
            }
            else
            {
                // handle error
            }
        });
    }


    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        Debug.Log($"Данные с облака были извлечены. Длина извлеченного массива байт = {data.Length}.\nДанные в виде строки: " + Encoding.UTF8.GetString(data));

        if (status == SavedGameRequestStatus.Success)
        {
            // handle processing the byte array data

            // Синхронизация данных модели из облака и локальной модели
            if (data.Length != 0)
            {
                PlayerDataSynchronizer.SynchronizePlayerDataStorages(data);
            }
            else { Debug.Log("Данные на облаке не были найдены."); }
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

                CurrentGameMetadata = null;
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
