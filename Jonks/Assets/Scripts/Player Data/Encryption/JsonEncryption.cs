﻿using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public static class JsonEncryption
{
    private static readonly string fileName = "StatsAlpha.json";
    public static string filePath => DataLoaderHelper.GetFilePath(fileName);

    private static readonly int salt = 100;


    public static string Encrypt(string data)
    {
        string saltedData = AddSalt(data);
        File.WriteAllText(filePath, StringHash(saltedData));

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(saltedData));
    }

    
    public static string Decrypt(string dataFile)
    {
        if (File.Exists(dataFile) && File.Exists(filePath))
        {
            string dataInBase64Encoding = File.ReadAllText(dataFile);
            string saltedData = Encoding.UTF8.GetString(Convert.FromBase64String(dataInBase64Encoding));

            // Совпадает ли хэш считанных данных с хэшом ранее сохраненных данных?
            return IsDataWasNotEdited(saltedData) ? AddSalt(saltedData) : null;
        }

        return null;
    }


    private static string AddSalt(string data)
    {
        char[] dataAsCharArray = data.ToCharArray();
        StringBuilder saltedData = new StringBuilder();

        for (int i = 0; i < dataAsCharArray.Length; i++)
        {
            int saltedCharacter = Convert.ToInt32(dataAsCharArray[i]) ^ salt;
            saltedData.Append(Convert.ToChar(saltedCharacter));
        }

        return saltedData.ToString();
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