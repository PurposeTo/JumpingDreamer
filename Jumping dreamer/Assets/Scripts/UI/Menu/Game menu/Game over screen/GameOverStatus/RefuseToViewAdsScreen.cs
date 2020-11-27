using UnityEngine;

public class RefuseToViewAdsScreen : MonoBehaviour
{
    public void OpenMainMenu()
    {
        SingleSceneLoader.Instance.LoadScene(SingleSceneLoader.MainMenuName);
    }


    public void ReloadLvl()
    {
        SingleSceneLoader.Instance.LoadScene(SingleSceneLoader.GameSceneName);
    }
}
