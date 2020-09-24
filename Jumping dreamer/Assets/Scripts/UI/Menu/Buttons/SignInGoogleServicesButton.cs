using UnityEngine;
using UnityEngine.UI;

public class SignInGoogleServicesButton : MonoBehaviour
{
    private Button button;


    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        SetButtonInteractable();
    }


    public void SignInGoogleServices()
    {
        GPGSAuthentication.Instance.Authenticate();
        SetButtonInteractable();
    }


    private void SetButtonInteractable()
    {
        button.interactable = !GPGSAuthentication.IsAuthenticated;
    }
}
