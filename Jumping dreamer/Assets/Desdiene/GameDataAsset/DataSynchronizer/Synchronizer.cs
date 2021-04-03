using System;
using System.Collections;
using Desdiene.Container;
using Desdiene.Coroutine.CoroutineExecutor;
using Desdiene.GameDataAsset.Data;
using Desdiene.GameDataAsset.DataLoader;
using Desdiene.GameDataAsset.Model;
using Desdiene.SuperMonoBehaviourAsset;

namespace Desdiene.GameDataAsset.DataSynchronizer
{
    public class Synchronizer<T> : SuperMonoBehaviourContainer, ISynchronizer where T : GameData
    {
        private readonly IModelInteraction<T> model;
        private readonly ReaderWriter<T> readerWriter;

        private readonly ICoroutineContainer ChooseDataInfo;

        public Synchronizer(SuperMonoBehaviour superMonoBehaviour, IModelInteraction<T> model, ReaderWriter<T> readerWriter)
            : base(superMonoBehaviour)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.readerWriter = readerWriter ?? throw new ArgumentNullException(nameof(readerWriter));

            ChooseDataInfo = superMonoBehaviour.CreateCoroutineContainer();
        }

        private T cashData = null;


        void ISynchronizer.ReadDataFromStorage()
        {
            readerWriter.Read(loadedData =>
            {
                if (loadedData == null) return;
                else
                {
                    if (cashData == null)
                    {
                        cashData = loadedData;
                        T combinedData = CombineData(model.GetData(), loadedData);
                        model.SetData(combinedData);
                        return;
                    }
                    else
                    {
                        if (cashData.Equals(loadedData)) return;
                        else
                        {
                            ChooseData(loadedData, choosedData => model.SetData(choosedData));
                            return;
                        }
                    }

                }
            });
        }

        void ISynchronizer.WriteDataToStorage()
        {
            readerWriter.Write(model.GetData());
        }

        private void ChooseData(T loadedData, Action<T> choosedData)
        {
            T currentData = model.GetData();

            superMonoBehaviour.ExecuteCoroutineContinuously(ChooseDataInfo,
                ChooseDataEnumerator(currentData, loadedData, choosedData));
        }

        private IEnumerator ChooseDataEnumerator(T currentData, T loadedData, Action<T> choosedData)
        {
            //todo предложить игроку выбрать модель
            yield return null;
            choosedData?.Invoke(currentData);
        }

        private T CombineData(T data1, T data2)
        {
            //todo добавить смешение данных за прошлую игровую сессию с текущими данными
            return data1;
        }
    }
}
