using System.Text;
using UnityEngine;

public static class PlayerDataSynchronizer
{
    // Проверка соответствия данных модели из облака и локальной модели
    private static bool IsModelsEqual(PlayerDataModel cloudModel, PlayerDataModel localModel)
    {
        if (cloudModel.Id != localModel.Id)
        {
            if (PlayerDataStorageSafe.IsPlayerDataAlreadyReset)
            {
                GPGSPlayerDataCloudStorage.Instance.CreateSave(Encoding.UTF8.GetBytes(JsonConverterWrapper.SerializeObject(localModel, null)));
            }
            else
            {
                // TODO: вывести окно с предложением о выборе модели.
                // В зависимости от выбора пользователя загрузить модель либо в облако, либо на устройство
            }

            return true;
        }

        return cloudModel.PlayerStats.Equals(localModel.PlayerStats) &&
            cloudModel.PlayerInGamePurchases.Equals(localModel.PlayerInGamePurchases);
    }


    public static void MixModels(PlayerDataModel cloudModel, PlayerDataModel localModel)
    {
        if (IsModelsEqual(cloudModel, localModel))
        {
            Debug.Log("The model from the cloud already has up-to-date data.");
        }
        else
        {
        }
    }


    // Если произошло смешение моделей, то необходимо обновить модель (на облаке/локально)
}
