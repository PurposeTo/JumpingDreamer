using System;
using System.Text;
using UnityEngine;

public static class PlayerDataSynchronizer
{
    public static event Action<PlayerDataModel> OnLocalModelWasChanged;


    // Синхронизация данных модели из облака и локальной модели
    public static void SynchronizePlayerDataStorages(byte[] data)
    {
        PlayerDataModel cloudModel = JsonConverterWrapper.DeserializeObject(Encoding.UTF8.GetString(data), null);

        if (cloudModel == null)
        {
            return;
        }


        PlayerDataModel localModel = PlayerDataLocalStorageSafe.Instance.PlayerDataModel;

        if (cloudModel.Id != localModel.Id)
        {
            if (PlayerDataLocalStorageSafe.IsPlayerDataAlreadyReset)
            {
                // Отправка изменений (данных новой модели) на облако
                GPGSPlayerDataCloudStorage.Instance.CreateSave(Encoding.UTF8.GetBytes(JsonConverterWrapper.SerializeObject(localModel, null)));
            }
            else
            {
                // TODO: вывести окно с предложением о выборе модели. В зависимости от выбора пользователя загрузить модель либо в облако, либо на устройство
            }
        }
        else
        {
            if (!(cloudModel.PlayerStats.Equals(localModel.PlayerStats) &&
            cloudModel.PlayerInGamePurchases.Equals(localModel.PlayerInGamePurchases)))
            {
                MixModels(cloudModel, localModel);
            }
            else
            {
                Debug.Log("Models already have the same data.");
            }
        }
    }


    // TODO: Перенести в PlayerDataModel
    private static void MixModels(PlayerDataModel cloudModel, PlayerDataModel localModel)
    {
        #region Логика смешения моделей
        // Заглушка
        PlayerDataModel mixedPlayerDataModel = PlayerDataModel.CreateModelWithDefaultValues();
        #endregion

        // Если произошло смешение моделей, то необходимо обновить модель на облаке И локально
        GPGSPlayerDataCloudStorage.Instance.CreateSave(
            Encoding.UTF8.GetBytes(JsonConverterWrapper.SerializeObject(mixedPlayerDataModel, null)));
        OnLocalModelWasChanged?.Invoke(mixedPlayerDataModel);
    }


    public static void SelectLocalModelHandler()
    {
        GPGSPlayerDataCloudStorage.Instance.CreateSave(
            Encoding.UTF8.GetBytes(JsonConverterWrapper.SerializeObject(PlayerDataLocalStorageSafe.Instance.PlayerDataModel, null)));
    }


    public static void SelectCloudModelHandler(PlayerDataModel cloudModel)
    {
        // Нужна ли проверка? Выбор не будет доступен, если cloudModel == null (т.е. этому обработчику нечего будет обрабатывать, т.к. нельзя нажать на кнопку, которой нет)
        if (cloudModel != null)
        {
            OnLocalModelWasChanged?.Invoke(cloudModel);
        }
    }


    public static void RestorePlayerDataFromCloud()
    {
        // 1. Получить облачную модель из GPGSPlayerDataCloudStorage
        // 2. Сделать Set приватному полю модели
    }
}
