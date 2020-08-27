using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class InternetConnectionChecker : SingletonMonoBehaviour<InternetConnectionChecker>
{
    public bool IsInternetConnectionAvaliable()
    {
        bool isInternetAvaliable = false;

        StartCoroutine(CheckInternetConnectionEnumerator(isDeviceHasInternetConnection => isInternetAvaliable = isDeviceHasInternetConnection));

        return isInternetAvaliable;
    }


    private IEnumerator CheckInternetConnectionEnumerator(Action<bool> action)
    {
        using (UnityWebRequest request = new UnityWebRequest("http://google.com", "GET"))
        {
            yield return request.SendWebRequest();

            if (request.error != null) //  || isHttpError == true - Вдруг у гугла ошибки с серверами будут? :)
            {
                Debug.LogWarning(request.error);
                action(false);
            }
            else
            {
                action(true);
            }
        }
    }
}
