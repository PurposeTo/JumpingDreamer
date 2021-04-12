using System;
using System.Collections;
using System.Text;
using Desdiene.Coroutine.CoroutineExecutor;
using Desdiene.GameDataAsset.Data;
using Desdiene.GameDataAsset.DataLoader.Storage;
using Desdiene.JsonConvertorWrapper;
using Desdiene.SuperMonoBehaviourAsset;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Desdiene.GameDataAsset.ConcreteStorages
{
    public class GooglePlayStorage<T> : DataStorage<T>
         where T : GameData, new()
    {
        private readonly ICoroutineContainer loadDataInfo;

        public GooglePlayStorage(SuperMonoBehaviour superMonoBehaviour,
            string fileName,
            string fileExtension,
            IJsonConvertor<T> jsonConvertor)
            : base(superMonoBehaviour,
                  "Облачное хранилище google play-я",
                  fileName,
                  fileExtension,
                  jsonConvertor)
        {
            loadDataInfo = superMonoBehaviour.CreateCoroutineContainer();
        }

        private DateTime startPlayingTime;
        private ISavedGameMetadata currentGameMetadata;

        private ISavedGameClient SavedGameClient => ((PlayGamesPlatform)Social.Active).SavedGame;

        protected override void Read(Action<string> jsonDataCallback)
        {
            superMonoBehaviour.ExecuteCoroutineContinuously(loadDataInfo, LoadDataEnumerator(jsonDataCallback));
        }

        protected override void Write(string jsonData)
        {
            if (currentGameMetadata == null)
            {
                Debug.LogWarning("Невозможно записать данные на облако, не открыв сохранения! " +
                    "Сохранение откроется автоматически при чтении данных с облака.");
            }
            else
            {
                SaveData(Encoding.UTF8.GetBytes(jsonData));
            }
        }

        private IEnumerator LoadDataEnumerator(Action<string> jsonDataCallback)
        {
            Debug.Log("Начало операции загрузки данных с облака. Ожидание аутентификации пользователя.");
            yield return new WaitUntil(() => GPGSAuthentication.IsAuthenticated);
            Debug.Log("Операция загрузки данных с облака - пользователь аутентифицировался.");

            // Начать отсчет времени для текущей сессии игры
            startPlayingTime = DateTime.Now;

            // Загрузка данных из облака
            ReadSavedGame((readingStatus, data) =>
            {
                Debug.Log($"Данные с облака были извлечены со статусом " + readingStatus);

                // Данные считаются извлеченными только тогда, когда статус их чтения "Success"
                if (readingStatus == SavedGameRequestStatus.Success)
                {
                    string cloudDataAsStr = Encoding.UTF8.GetString(data);
                    Debug.Log($"Длина извлеченного массива байт = { data.Length }.\nДанные в виде строки: " + cloudDataAsStr);
                    jsonDataCallback?.Invoke(cloudDataAsStr);
                }
            });
        }

        private void ReadSavedGame(Action<SavedGameRequestStatus, byte[]> completedCallback)
        {
            OpenSavedGame((openingStatus, gameMetadata) =>
            {
                Debug.Log("Данные с облака были открыты со статусом " + openingStatus);

                // Данные начинают считываться только тогда, когда статус их открытия "Success"
                if (openingStatus == SavedGameRequestStatus.Success)
                {
                    currentGameMetadata = gameMetadata;
                    //Чтение и открытие данных - разные операции
                    SavedGameClient.ReadBinaryData(gameMetadata, completedCallback);
                }
            });
        }


        private void OpenSavedGame(Action<SavedGameRequestStatus, ISavedGameMetadata> OnSavedGameOpened)
        {
            Debug.Log("Начало открытия сохранения на облаке");
            if (!GPGSAuthentication.IsAuthenticated)
            {
                Debug.Log("Ошибка открытия сохранения на облаке! Потеряна аутентификация пользователя...");
                OnSavedGameOpened(SavedGameRequestStatus.AuthenticationError, null);
                return;
            }

            SavedGameClient.OpenWithAutomaticConflictResolution(FileNameWithExtension,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime,
                OnSavedGameOpened);
        }

        private void SaveData(byte[] dataToSave)
        {
            TimeSpan allPlayingTime = DateTime.Now - startPlayingTime;
            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

            builder = builder.WithUpdatedPlayedTime(currentGameMetadata.TotalTimePlayed + allPlayingTime)
                .WithUpdatedDescription("Saved game at " + DateTime.Now);

            SavedGameMetadataUpdate updatedMetadata = builder.Build();
            SavedGameClient.CommitUpdate(currentGameMetadata, updatedMetadata, dataToSave, OnSavedGameCreated);
        }


        private void OnSavedGameCreated(SavedGameRequestStatus dataSavedStatus, ISavedGameMetadata gameMetadata)
        {
            Debug.Log($"Данные были записаны на облако со статусом " + dataSavedStatus);

            if (dataSavedStatus == SavedGameRequestStatus.Success)
            {
                // Так как при сохранении метаданные обновляются, то после его завершения необходимо их перезаписать
                currentGameMetadata = gameMetadata;

                // Заново считаем время игры с момента записи сохранения
                startPlayingTime = DateTime.Now;
            }
        }
    }
}
