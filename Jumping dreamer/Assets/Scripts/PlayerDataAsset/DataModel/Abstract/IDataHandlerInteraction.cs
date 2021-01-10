public interface IDataHandlerInteraction
{
    IModelInteraction GetInteractionWithDataOfLastGamingSessions();
    IModelInteraction GetInteractionWithDataOfCurrentGameSession();
    PlayerGameData GetUsedData();
}
