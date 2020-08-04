using UnityEngine;
using System.IO;

public static class DataLoaderHelper
{
    public static string GetFilePath(string fileName)
    {
        string FilePath = "";

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            FilePath = Path.Combine(Application.dataPath + "/" + fileName);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            FilePath = Path.Combine(Application.persistentDataPath + "/" + fileName);
        }

        return FilePath;
    }
}
