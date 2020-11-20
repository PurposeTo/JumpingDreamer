using System;
using System.Collections;
using UnityEngine;

public class PlayerDataSynchronizer
{
    private readonly SuperMonoBehaviour superMonoBehaviour;
    private GPGSPlayerDataCloudStorage GPGSPlayerDataCloudStorage;
    private ICoroutineInfo getSyncronizedPlayerDataModelInfo;


    public PlayerDataSynchronizer(SuperMonoBehaviour superMonoBehaviour)
    {
        this.superMonoBehaviour = superMonoBehaviour != null ? superMonoBehaviour : throw new ArgumentNullException(nameof(superMonoBehaviour));
        GPGSPlayerDataCloudStorage = new GPGSPlayerDataCloudStorage(superMonoBehaviour);
        getSyncronizedPlayerDataModelInfo = superMonoBehaviour.CreateCoroutineInfo();
    }


    public bool IsDataFileLoaded => localStorageSafe.IsDataFileLoaded;

    private PlayerDataLocalStorageSafe localStorageSafe = new PlayerDataLocalStorageSafe();


    // Синхронизация данных модели из облака и локальной модели
    public PlayerDataModel SynchronizePlayerDataStorages(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        Debug.Log($"SYNC: Received cloud model: {cloudModel}.\nCloud model as json: {JsonConverterWrapper.SerializeObject(cloudModel, null)}");
        Debug.Log($"SYNC: Received local model: {localModel}.\nLocal model as json: {JsonConverterWrapper.SerializeObject(localModel, null)}");

        // Все еще нужна эта проверка?
        if (cloudModel == null)
        {
            return localModel;
        }


        if (localModel.Id != cloudModel.Id)
        {
            if (PlayerDataModelController.IsPlayerDataHaveAlreadyDeletedOrRestored)
            {
                // Отправка изменений (данных новой модели) на облако
                GPGSPlayerDataCloudStorage.CreateSave(localModel);
                return cloudModel;
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
                MixModels(localModel, cloudModel);
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
                if (selectedModel == null) throw new ArgumentNullException("selectedModel which in fact is the localModel");
                GPGSPlayerDataCloudStorage.CreateSave(selectedModel);
                break;
            case PlayerDataModelController.DataModelSelectionStatus.CloudModel:
                if (selectedModel == null) break;
                else localModel = selectedModel;
                break;
        }
    }


    public IEnumerator GetSynchronizedPlayerDataModelEnumerator(Action<PlayerDataModel> onPlayerDataModelsSynchronizedCallback)
    {
        float timeout = 15f;

        localStorageSafe.LoadPlayerData();
        GPGSPlayerDataCloudStorage.StartLoadSavedGameFromCloudCoroutine();
        yield return new WaitForDoneRealtime(timeout, () => GPGSPlayerDataCloudStorage.CloudPlayerDataModel != null);

        // Передаем модели из классов-холдеров моделей (облачн. и лок.)
        // Метод должен возвращать синхронизованную модель
        // Далее должен вызываться callback с ранее полученной синхр. моделью
        PlayerDataModel synchronizedModel = SynchronizePlayerDataStorages(localStorageSafe.LocalPlayerDataModel, GPGSPlayerDataCloudStorage.CloudPlayerDataModel);
        onPlayerDataModelsSynchronizedCallback?.Invoke(synchronizedModel);
    }


    public void StartGetSynchronizedPlayerDataModelCoroutine(Action<PlayerDataModel> onPlayerDataModelsSynchronizedCallback)
    {
        superMonoBehaviour.ContiniousCoroutineExecution(ref getSyncronizedPlayerDataModelInfo, GetSynchronizedPlayerDataModelEnumerator(onPlayerDataModelsSynchronizedCallback));
    }


    public bool IsGetSynchronizedPlayerDataModelCoroutineExecuting()
    {
        return getSyncronizedPlayerDataModelInfo.IsExecuting;
    }


    private void ProvideModelSelection(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        PopUpWindowGenerator.Instance.CreateChoosingWindow(localModel, cloudModel);
    }


    private PlayerDataModel MixModels(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        // Локальная модель не может = null.
        if (localModel == null) throw new ArgumentNullException("localModel");

        PlayerDataModel mixedPlayerDataModel = PlayerDataModel.MixPlayerModels(cloudModel, localModel);

        // Если произошло смешение моделей, то необходимо обновить модель на облаке И локально
        localModel = mixedPlayerDataModel;
        GPGSPlayerDataCloudStorage.CreateSave(mixedPlayerDataModel);
    }
}
