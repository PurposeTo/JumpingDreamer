public class DataHandler : IDataInteraction, IDataHandlerInteraction
{
    private DataModel lastGamingSessions;
    private readonly DataModel currentGamingSession;
    private readonly IModelInteraction modelInteraction;


    public DataHandler(DataModel modelWithCurrentData)
    {
        currentGamingSession = modelWithCurrentData;
        modelInteraction = modelWithCurrentData;
    }


    IDataGetter IDataInteraction.Getter => throw new System.NotImplementedException();
    IDataSetter IDataInteraction.Setter => ((IDataInteraction)currentGamingSession).Setter;
    IDataChangingNotifier IDataInteraction.Notifier => throw new System.NotImplementedException();


    IModelInteraction IDataHandlerInteraction.GetInteractionWithDataOfCurrentGameSession()
    {
        return currentGamingSession;
    }
    

    IModelInteraction IDataHandlerInteraction.GetInteractionWithDataOfLastGamingSessions()
    {
        return lastGamingSessions;
    }


    PlayerGameData IDataHandlerInteraction.GetUsedData()
    {
        throw new System.NotImplementedException();
    }
}
