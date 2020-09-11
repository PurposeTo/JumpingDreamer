using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

[Obsolete]
public class InternetConnectionChecker : SingletonMonoBehaviour<InternetConnectionChecker>
{
    public bool IsInternetAvaliable { get; private set; }

    private UnityWebRequest request;

    private Coroutine pingGoogleRoutine;


    private void Start()
    {
        request = new UnityWebRequest("http://google.com", "GET");
        StartCoroutine(CheckInternetConnectionEnumerator());
    }


    private IEnumerator CheckInternetConnectionEnumerator()
    {
        while (true)
        {
            if (pingGoogleRoutine == null) { pingGoogleRoutine = StartCoroutine(PingGoogleEnumerator()); }
            yield return pingGoogleRoutine;
            yield return new WaitForSeconds(15.0f);
        }
    }


    private IEnumerator PingGoogleEnumerator()
    {
        yield return request.SendWebRequest();

        if (request.error != null || request.isHttpError == true)
        {
            Debug.LogWarning(request.error);
            IsInternetAvaliable = false;
        }
        else { IsInternetAvaliable = true; }

        pingGoogleRoutine = null;
    }
}
