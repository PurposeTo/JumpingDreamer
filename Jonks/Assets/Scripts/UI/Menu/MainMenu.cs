using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneLoader.LoadScene(SceneLoader.GameSceneName);
    }
}