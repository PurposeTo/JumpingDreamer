using UnityEngine;
using System.IO;

public static class DataLoaderHelper
{
    public static string GetFilePath(string fileName)
    {
        string filePath = "";

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            filePath = Path.Combine(Application.dataPath, fileName);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            filePath = Path.Combine(Application.persistentDataPath, fileName);
        }

        return filePath;
    }
}
