public interface IDataInteraction
{
    IDataGetter Getter { get; }
    IDataSetter Setter { get; }
    IDataChangingNotifier Notifier { get; }
}
