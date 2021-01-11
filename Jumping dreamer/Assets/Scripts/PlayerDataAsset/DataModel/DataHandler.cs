using UnityEngine;

public class DataHandler : IDataInteraction, IDataHandlerInteraction
{
    private readonly DataModel currentGamingSession;
    private readonly DataModel lastGamingSessions;

    private readonly IDataGetter usedDataGetter;

    public DataHandler(DataModel currentGamingSession, DataModel lastGamingSessions)
    {
        this.currentGamingSession = currentGamingSession ?? throw new System.ArgumentNullException(nameof(currentGamingSession));
        this.lastGamingSessions = lastGamingSessions ?? throw new System.ArgumentNullException(nameof(lastGamingSessions));

        //todo: останется ли внутри UsedDataGetter правильные ссылки на PlayerGameData, если внутри модели ее пере-set'ят?
        usedDataGetter = new UsedDataGetter
            (
                ((IModelInteraction)currentGamingSession).GetData(),
                ((IModelInteraction)lastGamingSessions).GetData()
            );
    }


    IDataGetter IDataInteraction.Getter => usedDataGetter;
    IDataSetter IDataInteraction.Setter => ((IDataInteraction)currentGamingSession).Setter;
    IDataChangingNotifier IDataInteraction.Notifier => currentGamingSession;

    IModelInteraction IDataHandlerInteraction.GetInteractionWithDataOfCurrentGameSession() => currentGamingSession;

    IModelInteraction IDataHandlerInteraction.GetInteractionWithDataOfLastGamingSessions() => lastGamingSessions;
}
