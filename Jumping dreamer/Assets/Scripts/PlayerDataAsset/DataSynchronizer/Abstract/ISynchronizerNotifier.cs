using System;

public interface ISynchronizerNotifier
{
    event Action OnResetPlayerData;
    event Action OnSavePlayerData;
}
