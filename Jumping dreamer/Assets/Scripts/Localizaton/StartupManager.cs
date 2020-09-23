using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour {

	// Use this for initialization
	private IEnumerator Start () {

        yield return new WaitUntil(() => LocalizationManager.Instance.GetIsReady());

        SceneManager.LoadScene("Training_10");
    }
}
