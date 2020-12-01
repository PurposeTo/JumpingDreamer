using System;
using System.Collections;
using UnityEngine;

public enum PlayerModelDataType
{
    LocalModel,
    CloudModel
}

public class PlayerDataSynchronizer : SuperMonoBehaviourContainer
{
    private readonly PlayerDataLocalStorageSafe localStorage;
    private readonly GPGSPlayerDataCloudStorage cloudStorage;
    private ICoroutineContainer getSyncronizedPlayerModelDataInfo;
    private ICoroutineContainer provideModelChoosingToPlayerInfo;


    public PlayerDataSynchronizer(SuperMonoBehaviour superMonoBehaviour,
                                  PlayerDataLocalStorageSafe localStorage,
                                  GPGSPlayerDataCloudStorage cloudStorage) : base(superMonoBehaviour)
    {
        this.localStorage = localStorage;
        this.cloudStorage = cloudStorage;
        getSyncronizedPlayerModelDataInfo = superMonoBehaviour.CreateCoroutineContainer();
        provideModelChoosingToPlayerInfo = superMonoBehaviour.CreateCoroutineContainer();
    }


    private PlayerModelData LocalModel => localStorage.Data;
    private PlayerModelData CloudModel => cloudStorage.Data;


    // Синхронизация данных модели из облака и локальной модели
    public IEnumerator SynchronizePlayerDataModels(Action<PlayerModelData> modelData)
    {
        Debug.Log($"SYNC: Received cloud model: {CloudModel}.\nCloud model as json: {JsonConverterWrapper.SerializeObject(CloudModel, null)}");
        Debug.Log($"SYNC: Received local model: {LocalModel}.\nLocal model as json: {JsonConverterWrapper.SerializeObject(LocalModel, null)}");

        if (LocalModel.Id != CloudModel.Id)
        {
            if (PlayerDataModelController.IsPlayerDataHaveAlreadyDeletedOrRestored)
            {
                cloudStorage.SaveData(LocalModel);
                modelData?.Invoke(LocalModel);
            }
            else
            {
                PlayerModelDataType choosenDataModelType = default;

                superMonoBehaviour.ExecuteCoroutineContinuously(ref provideModelChoosingToPlayerInfo, ProvideModelChoosingToPlayer(choosenModelType => choosenDataModelType = choosenModelType));
                yield return new WaitWhile(() => provideModelChoosingToPlayerInfo.IsExecuting);

                if (choosenDataModelType == PlayerModelDataType.CloudModel) modelData?.Invoke(CloudModel);
                else modelData?.Invoke(LocalModel);
            }
        }
        else
        {
            if (!(LocalModel.StatsData.Equals(CloudModel.StatsData) &&
            LocalModel.InGamePurchasesData.Equals(CloudModel.InGamePurchasesData)))
            {
                PlayerModelData combinedModel = CombineModels();
                SavePlayerDataToAllStorages(combinedModel);
                modelData?.Invoke(combinedModel);
            }
            else
            {
                Debug.Log("Cloud and local models already have the same data.");

                modelData?.Invoke(LocalModel);
            }
        }
    }


    public IEnumerator GetSynchronizedPlayerDataModelEnumerator(Action<PlayerModelData> onPlayerDataModelsSynchronizedCallback)
    {
        float timeout = 15f;

        localStorage.LoadDataFromFileAndDecrypt();
        cloudStorage.StartOpeningGameSession();
        yield return new WaitForDoneRealtime(timeout, () => !cloudStorage.IsDataLoading);

        if (LocalModel != null && CloudModel != null)
        {
            yield return SynchronizePlayerDataModels(modelData => onPlayerDataModelsSynchronizedCallback?.Invoke(modelData));
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
        else onPlayerDataModelsSynchronizedCallback?.Invoke(null);
    }


    public void StartSynchronizingPlayerDataModel(Action<PlayerModelData> onPlayerDataModelsSynchronizedCallback)
    {
        superMonoBehaviour.ExecuteCoroutineContinuously(
            ref getSyncronizedPlayerModelDataInfo,
            GetSynchronizedPlayerDataModelEnumerator(onPlayerDataModelsSynchronizedCallback));
    }


    private IEnumerator ProvideModelChoosingToPlayer(Action<PlayerModelDataType> chooseTheModelCallback)
    {
        ModelChoosingWindow choosingWindow = PopUpWindowGenerator.Instance.CreateModelChoosingWindow(((IGetModelData)LocalModel).StatsData, ((IGetModelData)CloudModel).StatsData);

        yield return new WaitUntil(() => choosingWindow.SelectedDataModel == null);
        chooseTheModelCallback?.Invoke(choosingWindow.SelectedModelDataType);

        choosingWindow.CloseWindow();
    }


    private PlayerModelData CombineModels() => PlayerModel.CombineData(CloudModel, LocalModel);


    private void SavePlayerDataToAllStorages(PlayerModelData playerModelData)
    {
        localStorage.SaveDataToFileAndEncrypt(playerModelData);
        cloudStorage.SaveData(playerModelData);
    }
}
