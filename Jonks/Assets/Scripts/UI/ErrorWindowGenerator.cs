using UnityEngine;

public class ErrorWindowGenerator : SingletonMonoBehaviour<ErrorWindowGenerator>
{
    [SerializeField] private GameObject errorWindow = null;

    public void CreateErrorWindow(string errorText)
    {
        Instantiate(errorWindow).GetComponent<ErrorWindow>().errorTextObject.text = errorText;
    }
}
