using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Desdiene.Tools
{
    public class DeviceDataLoader
    {
        private readonly string filePath;
        private readonly int retryCount = 3;


        public DeviceDataLoader(string filePath)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Загрузить данные с устройства.
        /// </summary>
        /// <param name="jsonAction">Полученные данные. Может быть null, если данные не были найдены.</param>
        /// <returns></returns>
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
                for (int i = 0; data == null && i < retryCount; i++)
                {
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
}
