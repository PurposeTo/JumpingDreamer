namespace Desdiene.GameDataAsset.Data
{
    public interface IDataInteraction<TGetter, TSetter, TChangingNotifier>
        where TGetter : IDataGetter
        where TSetter : IDataSetter
        where TChangingNotifier : IDataChangingNotifier
    {
        TGetter Getter { get; }
        TSetter Setter { get; }
        TChangingNotifier Notifier { get; }
    }
}
