using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSynchronizer
{
    // Синхронизация данных модели из облака и локальной модели
    public void SynchronizePlayerDataStorages(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        if (cloudModel == null)
        {
            return;
        }


        if (localModel.Id != cloudModel.Id)
        {
            if (PlayerDataModelController.IsPlayerDataAlreadyReset)
            {
                // Отправка изменений (данных новой модели) на облако
                GPGSPlayerDataCloudStorage.Instance.CreateSave(localModel);
            }
            else
            {
                SelectDataModel(localModel, cloudModel);
            }
        }
        else
        {
            if (!(localModel.PlayerStats.Equals(cloudModel.PlayerStats) &&
            localModel.PlayerInGamePurchases.Equals(cloudModel.PlayerInGamePurchases)))
            {
                MixModels(localModel, cloudModel);
            }
            else
            {
                Debug.Log("Cloud and local models already have the same data.");
            }
        }
    }


    private void MixModels(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        #region Логика смешения моделей
        // Заглушка
        PlayerDataModel mixedPlayerDataModel = PlayerDataModel.CreateModelWithDefaultValues();
        #endregion

        // Если произошло смешение моделей, то необходимо обновить модель на облаке И локально
        GPGSPlayerDataCloudStorage.Instance.CreateSave(localModel);
        localModel = mixedPlayerDataModel;
    }


    private PlayerDataModel SelectDataModel(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        // TODO: Вызвать корутину ожидания с предложением игроку выбрать модель
        // TODO: вывести окно с предложением о выборе модели. В зависимости от выбора пользователя загрузить модель либо в облако, либо на устройство
        #region Игрок выбирает модель
        // Заглушка
        PlayerDataModel selectedModel = PlayerDataModel.CreateModelWithDefaultValues();
        #endregion

        return selectedModel;
    }

}
