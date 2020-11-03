using UnityEngine;

public class RefuseToViewAdsScreen : MonoBehaviour
{
    public void OpenMainMenu()
    {
        SingleSceneLoader.LoadScene(SingleSceneLoader.MainMenuName);
    }


    public void ReloadLvl()
    {
        SingleSceneLoader.LoadScene(SingleSceneLoader.GameSceneName);
    }
}
