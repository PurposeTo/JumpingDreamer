using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGameClickHandler()
    {
        SceneLoader.LoadScene(SceneLoader.GameSceneName);
    }
}