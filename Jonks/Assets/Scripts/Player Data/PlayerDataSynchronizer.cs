﻿using UnityEngine;

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
        PlayerDataModel mixedPlayerDataModel = PlayerDataModel.MixPlayerModels(cloudModel, localModel);

        // Если произошло смешение моделей, то необходимо обновить модель на облаке И локально
        GPGSPlayerDataCloudStorage.Instance.CreateSave(mixedPlayerDataModel);
        localModel = mixedPlayerDataModel;
    }


    // Обработка предложенного пользователю выбора одной из моделей
    private void SelectDataModel(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        // TODO: Вызвать корутину ожидания с предложением игроку выбрать модель
        // TODO: вывести окно с предложением о выборе модели. В зависимости от выбора пользователя загрузить модель либо в облако, либо на устройство
        /* 1. Создать объект окна выбора модели
         * 2. Получить с него скрипт (выбора)
         * 3. Проинициализировать значения (два поля) внутри скрипта двумя моделями
         * 4. Ждать пока пользователь выберет модель (isSelected == true)
         * 5. Получить значения selected model из UI скрипта. Присвоить полученное значение в модель, которая будет передана в корутину в качестве параметра.
         * 6. Вызвать метод в UI скрипте - "Закрыть окошко"
         */
        #region Игрок выбирает модель
        // Заглушка
        PlayerDataModel selectedModel = PlayerDataModel.CreateModelWithDefaultValues();
        #endregion
    }

}
