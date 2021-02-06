using UnityEngine;

public class DataHandler : IDataInteraction, IDataHandlerInteraction
{
    public DataHandler(DataModel currentGamingSession, DataModel lastGamingSessions)
    {
        CurrentGameSession = currentGamingSession;
        LastGamingSessions = lastGamingSessions;

        //todo: останется ли внутри UsedDataGetter правильные ссылки на PlayerGameData, если внутри модели ее пере-set'ят?
        Getter = new UsedDataGetter
            (
                CurrentGameSession.GetData(),
                LastGamingSessions.GetData()
            );

        Setter = ((IDataInteraction)currentGamingSession).Setter;
        Notifier = ((IDataInteraction)currentGamingSession).Notifier;

    }


    public IDataGetter Getter { get; }
    public IDataSetter Setter { get; }
    public IDataChangingNotifier Notifier { get; }

    public IModelInteraction CurrentGameSession { get; }
    public IModelInteraction LastGamingSessions { get; }
}
