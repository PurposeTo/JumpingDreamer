using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class InternetConnectionChecker
{
    public IEnumerator PingGoogleEnumerator(Action<bool> isInternetAvailable)
    {
        using (UnityWebRequest request = new UnityWebRequest("https://google.com", "GET"))
        {
            yield return request.SendWebRequest();

            if (request.error != null || request.isHttpError == true) // isHttpError - нужна проверка?
            {
                Debug.LogWarning(request.error);
                isInternetAvailable?.Invoke(false);
            }
            else isInternetAvailable?.Invoke(true);
        }
    }


    public IEnumerator PingGoogleCheckerWithTimeoutEnumerator(Action<bool> isInternetAvailable)
    {
        int maxWaitingTime = 4;

        using (UnityWebRequest request = new UnityWebRequest("https://google.com", "GET"))
        {
            request.timeout = maxWaitingTime;
            yield return request.SendWebRequest();

            if (request.error != null || request.isHttpError == true) // isHttpError - нужна проверка?
            {
                Debug.LogWarning(request.error);
                isInternetAvailable?.Invoke(false);
            }
            else isInternetAvailable?.Invoke(true);
        }
    }
}
