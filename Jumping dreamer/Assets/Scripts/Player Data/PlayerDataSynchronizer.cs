using System;
using System.Collections;
using UnityEngine;

public enum LoadedPlayerDataModel
{
    LocalModel,
    CloudModel,
    CombinedModel,
    SameModels,
    Null
}

public class PlayerDataSynchronizer : SuperMonoBehaviourContainer
{
    private ICoroutineContainer getSyncronizedPlayerDataModelInfo;
    private ICoroutineContainer provideModelChoosingToPlayerInfo;


    public PlayerDataSynchronizer(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour)
    {
        getSyncronizedPlayerDataModelInfo = superMonoBehaviour.CreateCoroutineContainer();
        provideModelChoosingToPlayerInfo = superMonoBehaviour.CreateCoroutineContainer();
    }


    // Синхронизация данных модели из облака и локальной модели
    public PlayerDataModel SynchronizePlayerDataStorages(PlayerDataModel localModel, PlayerDataModel cloudModel, out LoadedPlayerDataModel loadedPlayerDataModel)
    {
        Debug.Log($"SYNC: Received cloud model: {cloudModel}.\nCloud model as json: {JsonConverterWrapper.SerializeObject(cloudModel, null)}");
        Debug.Log($"SYNC: Received local model: {localModel}.\nLocal model as json: {JsonConverterWrapper.SerializeObject(localModel, null)}");

        if (localModel.Id != cloudModel.Id)
        {
            if (PlayerDataModelController.IsPlayerDataHaveAlreadyDeletedOrRestored)
            {
                loadedPlayerDataModel = LoadedPlayerDataModel.LocalModel;
                return localModel;
            }
            else
            {
                LoadedPlayerDataModel choosenModel = LoadedPlayerDataModel.Null;
                superMonoBehaviour.ExecuteCoroutineContinuously(ref provideModelChoosingToPlayerInfo, ProvideModelChoosingToPlayer(choosenPlayerDataModel => choosenModel = choosenPlayerDataModel));

                loadedPlayerDataModel = choosenModel;

                if (choosenModel == LoadedPlayerDataModel.CloudModel) return cloudModel;
                else return localModel;
            }
        }
        else
        {
            if (!(localModel.PlayerStats.Equals(cloudModel.PlayerStats) &&
            localModel.PlayerInGamePurchases.Equals(cloudModel.PlayerInGamePurchases)))
            {
                loadedPlayerDataModel = LoadedPlayerDataModel.CombinedModel;
                return CombineModels(localModel, cloudModel);
            }
            else
            {
                Debug.Log("Cloud and local models already have the same data.");

                loadedPlayerDataModel = LoadedPlayerDataModel.SameModels;
                return localModel;
            }
        }
    }


    //// В зависимости от выбора пользователя загрузить модель либо в облако, либо на устройство
    //public void OnDataModelSelected(PlayerDataModel selectedModel, ref PlayerDataModel localModel, PlayerDataModelController.DataModelSelectionStatus modelSelectionStatus)
    //{
    //    switch(modelSelectionStatus)
    //    {
    //        case PlayerDataModelController.DataModelSelectionStatus.LocalModel:
    //            if (selectedModel == null) throw new ArgumentNullException("selectedModel which in fact is the localModel");
    //            GPGSPlayerDataCloudStorage.SaveDataToCloud(selectedModel);
    //            break;
    //        case PlayerDataModelController.DataModelSelectionStatus.CloudModel:
    //            if (selectedModel == null) break;
    //            else localModel = selectedModel;
    //            break;
    //    }
    //}


    public IEnumerator GetSynchronizedPlayerDataModelEnumerator(PlayerDataLocalStorageSafe localStorage, GPGSPlayerDataCloudStorage cloudStorage, Action<PlayerDataModel, LoadedPlayerDataModel> onPlayerDataModelsSynchronizedCallback)
    {
        float timeout = 15f;

        localStorage.LoadPlayerDataFromFileAndDecrypt();
        cloudStorage.StartLoadSavedGameFromCloudCoroutine();
        yield return new WaitForDoneRealtime(timeout, () => cloudStorage.CloudPlayerDataModel != null);

        PlayerDataModel localModel = localStorage.LocalPlayerDataModel;
        PlayerDataModel cloudModel = cloudStorage.CloudPlayerDataModel;

        if (localModel != null && cloudModel != null)
        {
            onPlayerDataModelsSynchronizedCallback?.Invoke(SynchronizePlayerDataStorages(localModel, cloudModel, out LoadedPlayerDataModel loadedPlayerDataModel), loadedPlayerDataModel);
        }
        else if (localModel != null) onPlayerDataModelsSynchronizedCallback?.Invoke(localModel, LoadedPlayerDataModel.LocalModel);
        else if (cloudModel != null) onPlayerDataModelsSynchronizedCallback?.Invoke(cloudModel, LoadedPlayerDataModel.CloudModel);
        else onPlayerDataModelsSynchronizedCallback?.Invoke(null, LoadedPlayerDataModel.Null);
    }


    public void StartGetSynchronizedPlayerDataModelCoroutine(PlayerDataLocalStorageSafe localStorage, GPGSPlayerDataCloudStorage cloudStorage, Action<PlayerDataModel, LoadedPlayerDataModel> onPlayerDataModelsSynchronizedCallback)
    {
        superMonoBehaviour.ExecuteCoroutineContinuously(ref getSyncronizedPlayerDataModelInfo, GetSynchronizedPlayerDataModelEnumerator(localStorage, cloudStorage, onPlayerDataModelsSynchronizedCallback));
    }


    public bool IsGetSynchronizedPlayerDataModelCoroutineExecuting() => getSyncronizedPlayerDataModelInfo.IsExecuting;


    private IEnumerator ProvideModelChoosingToPlayer(Action<LoadedPlayerDataModel> chooseTheModelCallback)
    {
        ModelChoosingWindow choosingWindow = PopUpWindowGenerator.Instance.CreateModelChoosingWindow(chooseTheModelCallback);
        yield return new WaitUntil(() => choosingWindow == null);
    }


    private PlayerDataModel CombineModels(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        if (localModel is null) throw new ArgumentNullException(nameof(localModel));
        if (cloudModel is null) throw new ArgumentNullException(nameof(cloudModel));

        return PlayerDataModel.CombinePlayerModels(cloudModel, localModel);
    }
}
