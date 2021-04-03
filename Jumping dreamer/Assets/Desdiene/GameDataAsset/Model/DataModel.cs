using System;
using Desdiene.GameDataAsset.Data;

namespace Desdiene.GameDataAsset.Model
{
    public class DataModel<TData, TGetter, TSetter, TChangingNotifier> :
        IDataInteraction<TGetter, TSetter, TChangingNotifier>,
        IModelInteraction<TData>
        where TGetter : Data.IDataGetter
        where TData : GameData, TGetter, new()
        where TSetter : DataSetter<TData>, Data.IDataSetter, new()
        where TChangingNotifier : Data.IDataChangingNotifier
    {
        private readonly TSetter dataSetter;

        public DataModel()
        {
            data = new TData();
            dataSetter = new TSetter();
            dataSetter.SetData(data);
        }

        private TData data; // Не может быть null

        TGetter IDataInteraction<TGetter, TSetter, TChangingNotifier>.Getter => data;

        TSetter IDataInteraction<TGetter, TSetter, TChangingNotifier>.Setter => dataSetter;

        TChangingNotifier IDataInteraction<TGetter, TSetter, TChangingNotifier>.Notifier => throw new NotImplementedException();

        TData IModelInteraction<TData>.GetData() => data;

        void IModelInteraction<TData>.SetData(TData data) => this.data = data;
    }
}
