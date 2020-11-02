using UnityEngine;

public class WaitingForAdLoadingWindow : MonoBehaviour
{
    public void TurnOn() => gameObject.SetActive(true);


    public void TurnOff() => gameObject.SetActive(false);
}
