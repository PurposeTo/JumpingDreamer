﻿using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class InternetConnectionChecker
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="isInternetAvailable"></param>
    /// <param name="timeOut">
    /// No timeout is applied when timeout is set to 0 and this property defaults to 0.
    /// Note: The set timeout may apply to each URL redirect on Android which can result in a longer response.</param>
    /// <returns></returns>
    public IEnumerator PingGoogleEnumerator(Action<bool> isInternetAvailable, int timeOut = 0)
    {
        using (UnityWebRequest request = new UnityWebRequest("https://google.com", "GET"))
        {
            request.timeout = timeOut;
            yield return request.SendWebRequest();

            if (request.error != null || request.isHttpError == true)
            {
                Debug.LogWarning(request.error);
                isInternetAvailable?.Invoke(false);
            }
            else isInternetAvailable?.Invoke(true);
        }
    }
}
