using UnityEngine;
using UnityEngine.UI;

public class SignInGoogleServicesButton : MonoBehaviour
{
    private Button button;

    private GPGSAuthentication GPGSAuthentication => GPGSServices.Instance.GPGSAuthentication;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        SetButtonInteractable();
    }


    public void SignInGoogleServices()
    {
        GPGSAuthentication.Authenticate();
        SetButtonInteractable();
    }


    private void SetButtonInteractable()
    {
        button.interactable = !GPGSAuthentication.IsAuthenticated;
    }
}
