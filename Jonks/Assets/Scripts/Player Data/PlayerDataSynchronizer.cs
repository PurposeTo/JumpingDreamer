using UnityEngine;
using System.Collections;
using System;

public class PlayerDataSynchronizer
{
    // Синхронизация данных модели из облака и локальной модели
    public void SynchronizePlayerDataStorages(PlayerDataModel localModel, PlayerDataModel cloudModel, Action WaitForPlayerChoose)
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
                SelectDataModel(localModel, cloudModel, WaitForPlayerChoose);
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
        PlayerDataModel mixedPlayerDataModel = PlayerDataModel.MixPlayerModels(cloudModel, localModel);

        // Если произошло смешение моделей, то необходимо обновить модель на облаке И локально
        GPGSPlayerDataCloudStorage.Instance.CreateSave(mixedPlayerDataModel);
        localModel = mixedPlayerDataModel;
    }


    // Обработка предложенного пользователю выбора одной из моделей
    private void SelectDataModel(PlayerDataModel localModel, PlayerDataModel cloudModel, Action WaitForPlayerChoose)
    {
        // Вызов корутины ожидания с предложением выбора модели (локальной или облачной) игроку
        WaitForPlayerChoose?.Invoke();

        // В зависимости от выбора пользователя загрузить модель либо в облако, либо на устройство
        if (PlayerDataModelController.Instance.PlayerDataLocalModel.Equals(cloudModel))
        {
            localModel = GPGSPlayerDataCloudStorage.Instance.ReadSavedGame(PlayerDataModel.FileName);
        }
        else { GPGSPlayerDataCloudStorage.Instance.CreateSave(localModel); }
    }


    public IEnumerator WaitForPlayerChooseEnumerator(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        MixingModelsWindow mixingModelsWindow = DialogWindowGenerator.Instance.CreateChoosingWindow(localModel, cloudModel);
        yield return new WaitUntil(() => mixingModelsWindow.IsSelected);

        localModel = mixingModelsWindow.SelectedPlayerDataModel;

        mixingModelsWindow.CloseWindow();
    }
}
