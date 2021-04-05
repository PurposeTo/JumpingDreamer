using UnityEngine;
using System.IO;

namespace Desdiene.Tools
{
    public static class FilePathGetter
    {
        public static string GetFilePath(string fileName)
        {
            var runningPlatform = Application.platform;

            switch (runningPlatform)
            {
                case RuntimePlatform.WindowsEditor:
                    return Path.Combine(Application.dataPath, fileName);
                case RuntimePlatform.Android:
                    return Path.Combine(Application.persistentDataPath, fileName);
                default:
                    Debug.LogError($"{runningPlatform} is unknown platform to GetFilePath()!");
                    return "";
            }
        }
    }
}
