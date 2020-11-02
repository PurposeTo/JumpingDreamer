using UnityEngine;

public class LoadingWindow : MonoBehaviour
{
    public void TurnOn() => gameObject.SetActive(true);


    public void TurnOff() => gameObject.SetActive(false);
}
