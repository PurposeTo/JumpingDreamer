using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseButton;
    public GameObject PauseScreen;

    public void Pause()
    {
        Time.timeScale = 0f;

        PauseScreen.SetActive(true);
        PauseButton.SetActive(false);
    }


    public void Resume()
    {
        Time.timeScale = 1f;

        PauseScreen.SetActive(false);
        PauseButton.SetActive(true);
    }


    public void OpenMainMenu()
    {
        SceneLoader.LoadScene(SceneLoader.MainMenuName);
    }
}
