using UnityEngine;

public class PlayerDataSynchronizer
{
    // Синхронизация данных модели из облака и локальной модели
    public void SynchronizePlayerDataStorages(ref PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        if (cloudModel == null)
        {
            return;
        }


        if (localModel.Id != cloudModel.Id)
        {
            if (PlayerDataModelController.IsPlayerDataHaveAlreadyDeletedOrRestored)
            {
                // Отправка изменений (данных новой модели) на облако
                GPGSPlayerDataCloudStorage.Instance.CreateSave(localModel);
            }
            else
            {
                ProvideModelSelection(localModel, cloudModel);
            }
        }
        else
        {
            if (!(localModel.PlayerStats.Equals(cloudModel.PlayerStats) &&
            localModel.PlayerInGamePurchases.Equals(cloudModel.PlayerInGamePurchases)))
            {
                MixModels(ref localModel, cloudModel);
            }
            else
            {
                Debug.Log("Cloud and local models already have the same data.");
            }
        }
    }


    // В зависимости от выбора пользователя загрузить модель либо в облако, либо на устройство
    public void OnDataModelSelected(PlayerDataModel selectedModel, ref PlayerDataModel localModel, PlayerDataModelController.DataModelSelectionStatus modelSelectionStatus)
    {
        switch(modelSelectionStatus)
        {
            case PlayerDataModelController.DataModelSelectionStatus.LocalModel:
                GPGSPlayerDataCloudStorage.Instance.CreateSave(selectedModel);
                break;
            case PlayerDataModelController.DataModelSelectionStatus.CloudModel:
                localModel = selectedModel;
                break;
        }
    }


    private void ProvideModelSelection(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        DialogWindowGenerator.Instance.CreateChoosingWindow(localModel, cloudModel);
    }


    private void MixModels(ref PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        PlayerDataModel mixedPlayerDataModel = PlayerDataModel.MixPlayerModels(cloudModel, localModel);

        // Если произошло смешение моделей, то необходимо обновить модель на облаке И локально
        localModel = mixedPlayerDataModel;
        GPGSPlayerDataCloudStorage.Instance.CreateSave(mixedPlayerDataModel);
    }
}
