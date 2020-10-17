using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class InternetConnectionChecker
{
    public IEnumerator PingGoogleEnumerator(Action<bool> isNeedToShowAd)
    {
        using (UnityWebRequest request = new UnityWebRequest("https://google.com", "GET"))
        {
            yield return request.SendWebRequest();

            if (request.error != null || request.isHttpError == true) // isHttpError - нужна проверка?
            {
                Debug.LogWarning(request.error);
                isNeedToShowAd?.Invoke(false);
            }
            else isNeedToShowAd?.Invoke(true);
        }
    }
}
