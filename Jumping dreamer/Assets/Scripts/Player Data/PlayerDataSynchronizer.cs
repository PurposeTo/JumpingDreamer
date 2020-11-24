using System;
using System.Collections;
using UnityEngine;

public enum PlayerDataModelType
{
    LocalModel,
    CloudModel
}

public class PlayerDataSynchronizer : SuperMonoBehaviourContainer
{
    private readonly PlayerDataLocalStorageSafe localStorage;
    private readonly GPGSPlayerDataCloudStorage cloudStorage;
    private ICoroutineContainer getSyncronizedPlayerDataModelInfo;
    private ICoroutineContainer provideModelChoosingToPlayerInfo;


    public PlayerDataSynchronizer(SuperMonoBehaviour superMonoBehaviour,
                                  PlayerDataLocalStorageSafe localStorage,
                                  GPGSPlayerDataCloudStorage cloudStorage) : base(superMonoBehaviour)
    {
        this.localStorage = localStorage;
        this.cloudStorage = cloudStorage;
        getSyncronizedPlayerDataModelInfo = superMonoBehaviour.CreateCoroutineContainer();
        provideModelChoosingToPlayerInfo = superMonoBehaviour.CreateCoroutineContainer();
    }


    private PlayerDataModel LocalModel => localStorage.LocalPlayerDataModel;
    private PlayerDataModel CloudModel => cloudStorage.CloudPlayerDataModel;


    // Синхронизация данных модели из облака и локальной модели
    public PlayerDataModel SynchronizePlayerDataModels()
    {
        Debug.Log($"SYNC: Received cloud model: {CloudModel}.\nCloud model as json: {JsonConverterWrapper.SerializeObject(CloudModel, null)}");
        Debug.Log($"SYNC: Received local model: {LocalModel}.\nLocal model as json: {JsonConverterWrapper.SerializeObject(LocalModel, null)}");

        if (LocalModel.Id != CloudModel.Id)
        {
            if (PlayerDataModelController.IsPlayerDataHaveAlreadyDeletedOrRestored)
            {
                cloudStorage.SaveData(LocalModel);
                return LocalModel;
            }
            else
            {
                PlayerDataModelType choosenDataModelType = default;

                superMonoBehaviour.ExecuteCoroutineContinuously(ref provideModelChoosingToPlayerInfo, ProvideModelChoosingToPlayer(choosenModelType => choosenDataModelType = choosenModelType));

                if (choosenDataModelType == PlayerDataModelType.CloudModel) return CloudModel;
                else return LocalModel;
            }
        }
        else
        {
            if (!(LocalModel.PlayerStats.Equals(CloudModel.PlayerStats) &&
            LocalModel.PlayerInGamePurchases.Equals(CloudModel.PlayerInGamePurchases)))
            {
                PlayerDataModel combinedModel = CombineModels();
                SavePlayerDataToAllStorages(combinedModel);
                return combinedModel;
            }
            else
            {
                Debug.Log("Cloud and local models already have the same data.");

                return LocalModel;
            }
        }
    }


    public IEnumerator GetSynchronizedPlayerDataModelEnumerator(Action<PlayerDataModel> onPlayerDataModelsSynchronizedCallback)
    {
        float timeout = 15f;

        localStorage.LoadPlayerDataFromFileAndDecrypt();
        cloudStorage.StartLoadingData();
        yield return new WaitForDoneRealtime(timeout, () => !cloudStorage.IsDataLoading);

        if (LocalModel != null && CloudModel != null)
        {
            onPlayerDataModelsSynchronizedCallback?.Invoke(SynchronizePlayerDataModels());
        }
        else if (LocalModel != null)
        {
            cloudStorage.SaveData(LocalModel);
            onPlayerDataModelsSynchronizedCallback?.Invoke(LocalModel);
        }
        else if (CloudModel != null)
        {
            localStorage.SaveDataToFileAndEncrypt(CloudModel);
            onPlayerDataModelsSynchronizedCallback?.Invoke(CloudModel);
        }
        else
        {
            PlayerDataModel emptyModel = PlayerDataModel.CreateModelWithDefaultValues();
            SavePlayerDataToAllStorages(emptyModel);
            onPlayerDataModelsSynchronizedCallback?.Invoke(emptyModel);
        }
    }


    public void StartSynchronizingPlayerDataModel(Action<PlayerDataModel> onPlayerDataModelsSynchronizedCallback)
    {
        superMonoBehaviour.ExecuteCoroutineContinuously(
            ref getSyncronizedPlayerDataModelInfo,
            GetSynchronizedPlayerDataModelEnumerator(onPlayerDataModelsSynchronizedCallback));
    }


    private IEnumerator ProvideModelChoosingToPlayer(Action<PlayerDataModelType> chooseTheModelCallback)
    {
        ModelChoosingWindow choosingWindow = PopUpWindowGenerator.Instance.CreateModelChoosingWindow(LocalModel, CloudModel);

        yield return new WaitUntil(() => choosingWindow.SelectedDataModel == null);
        chooseTheModelCallback?.Invoke(choosingWindow.SelectedDataModelType);

        choosingWindow.CloseWindow();
    }


    private PlayerDataModel CombineModels() => PlayerDataModel.CombinePlayerModels(CloudModel, LocalModel);


    private void SavePlayerDataToAllStorages(PlayerDataModel playerDataModel)
    {
        localStorage.SaveDataToFileAndEncrypt(playerDataModel);
        cloudStorage.SaveData(playerDataModel);
    }
}
