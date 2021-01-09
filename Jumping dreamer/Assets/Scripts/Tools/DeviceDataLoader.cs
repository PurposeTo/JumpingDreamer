using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DeviceDataLoader
{
    private readonly string filePath;


    public DeviceDataLoader(string filePath)
    {
        this.filePath = filePath;
    }


    public IEnumerator LoadDataEnumerator(Action<string> jsonAction)
    {
        var platform = Application.platform;

        switch (platform)
        {
            case RuntimePlatform.Android:
                yield return LoadViaAndroid(jsonAction);
                break;
            case RuntimePlatform.WindowsEditor:
                jsonAction?.Invoke(LoadViaEditor());
                break;
            default:
                Debug.LogError($"{platform} is unknown platform!");
                yield return LoadViaAndroid(jsonAction);
                break;
        }
    }


    private string LoadViaEditor()
    {
        return File.Exists(filePath) ? File.ReadAllText(filePath) : null;
    }


    private IEnumerator LoadViaAndroid(Action<string> action)
    {
        string data = null;

        using (UnityWebRequest request = new UnityWebRequest { url = filePath, downloadHandler = new DownloadHandlerBuffer() })
        {
            int counter = 0;

            while (counter != 3 && data == null)
            {
                counter++;
                yield return request.SendWebRequest();

                if (request.error != null || request.responseCode == 404)
                {
                    Debug.LogWarning(request.error);

                    yield return null;
                }
                else data = request.downloadHandler.text;
            }
        }

        action?.Invoke(data);
    }
}
