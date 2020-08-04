using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public static class JsonEncryption
{
    private static readonly string fileName = "StatsAlpha.json";
    public static string filePath => DataLoaderHelper.GetFilePath(fileName);


    public static string Encrypt(string data)
    {
        File.WriteAllText(filePath, StringHash(data));
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
    }

    
    public static string Decrypt(string dataFile)
    {
        if (File.Exists(dataFile) && File.Exists(filePath))
        {
            string loadedData = File.ReadAllText(dataFile);
            string dataAsJSON = Encoding.UTF8.GetString(Convert.FromBase64String(loadedData));

            // Совпадает ли хэш считанных данных с хэшом ранее сохраненных данных?
            return IsDataWasNotEdited(dataAsJSON) ? dataAsJSON : null;
        }

        return null;
    }


    public static string StringHash(string data)
    {
        HashAlgorithm algorithm = SHA256.Create();
        StringBuilder stringBuilder = new StringBuilder();

        byte[] bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(data));
        foreach (byte item in bytes)
        {
            stringBuilder.Append(item.ToString("X2"));
        }

        return stringBuilder.ToString();
    }


    public static bool IsDataWasNotEdited(string dataAsJSON)
    {
        return StringHash(dataAsJSON) == File.ReadAllText(filePath);
    }
}
