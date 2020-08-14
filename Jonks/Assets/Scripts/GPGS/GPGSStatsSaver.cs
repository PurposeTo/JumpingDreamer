using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

public class GPGSStatsSaver : SingletonMonoBehaviour<GPGSStatsSaver>
{
    private PlayGamesPlatform GamesPlatform => (PlayGamesPlatform)Social.Active;
    private ISavedGameClient SavedGameClient => GamesPlatform.SavedGame;
    //public ISavedGameMetadata savedGameMetadata { get; private set; }
    public DateTime StartPlayingTime { get; private set; }


    public void ShowSavedGamesSelectMenu()
    {
        uint maxSavesNumberToDisplay = 5;
        bool allowCreateSave = false;
        bool allowDeleteSave = true;

        SavedGameClient.ShowSelectSavedGameUI("Select saved game",
            maxSavesNumberToDisplay,
            allowCreateSave,
            allowDeleteSave,
            OnSavedGameSelected);
    }


    private void OnSavedGameSelected(SelectUIStatus selectUIStatus, ISavedGameMetadata gameMetadata)
    {
        if (selectUIStatus == SelectUIStatus.SavedGameSelected)
        {
            // handle selected game save
            ReadSavedGame(gameMetadata.Filename);
        }
        else
        {
            // handle cancel or error
        }
    }


    public void OpenSavedGame(string fileName, Action<SavedGameRequestStatus, ISavedGameMetadata> OnOpenSavedGameOpened)
    {
        SavedGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,                            // What to use?
            OnOpenSavedGameOpened);

        // Also conflict can be resolved by OpenWithManualConflictResolution()
    }



    public void CreateSave(string fileName, byte[] savedData)
    {
        OpenSavedGame(fileName, (gameRequestStatus, gameMetadata) =>
        {
            if (gameRequestStatus == SavedGameRequestStatus.Success)
            {
                TimeSpan allPlayingTime = DateTime.Now - StartPlayingTime;
                SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

                builder = builder.WithUpdatedPlayedTime(allPlayingTime).WithUpdatedDescription("Saved game at " + DateTime.Now).WithUpdatedPngCoverImage(GetScreenshot().EncodeToPNG());

                SavedGameMetadataUpdate metadataUpdate = builder.Build();
                SavedGameClient.CommitUpdate(gameMetadata, metadataUpdate, savedData, OnSaveCreated);
            }
            else
            {
                // handle error
            }
        });
    }


    public void OnSaveCreated(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle writing of saved game.
        }
        else
        {
            // handle error
        }
    }


    public Texture2D GetScreenshot()
    {
        // Create a 2D texture that is 1024x700 pixels from which the PNG will be extracted
        Texture2D screenShot = new Texture2D(1024, 700);

        // Takes the screenshot from top left hand corner of screen and maps to top left hand corner of screenShot texture
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, (Screen.width / 1024) * 700), 0, 0);

        return screenShot;
    }


    public void ReadSavedGame(string fileName)
    {
        OpenSavedGame(fileName, (gameRequestStatus, gameMetadata) =>
        {
            if (gameRequestStatus == SavedGameRequestStatus.Success)
            {
                SavedGameClient.ReadBinaryData(gameMetadata, OnSavedGameDataRead);
                //savedGameMetadata = gameMetadata;
            }
            else
            {
                // handle error
            }
        });
    }


    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle processing the byte array data
            StartPlayingTime = DateTime.Now;
        }
        else
        {
            // handle error
        }
    }


    public void DeleteSavedGame(string fileName)
    {
        OpenSavedGame(fileName, (gameRequestStatus, gameMetadata) =>
        {
            if (gameRequestStatus == SavedGameRequestStatus.Success)
            {
                SavedGameClient.Delete(gameMetadata);
            }
            else
            {
                // handle error
            }
        });
    }
}
