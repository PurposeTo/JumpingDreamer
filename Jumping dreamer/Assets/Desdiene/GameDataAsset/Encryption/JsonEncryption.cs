using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using UnityEngine;
using Desdiene.Tools;

namespace Desdiene.GameDataAsset.Encryption
{
    public class JsonEncryption
    {
        protected string CryptoFileName { get; }
        protected string FileExtension { get; }
        protected string FileNameWithExtension => CryptoFileName + FileExtension;

        protected readonly string hashDataFilePath;
        private static readonly int salt = 100;

        public JsonEncryption(string fileName, string fileExtension)
        {
            CryptoFileName = $"{fileName}Alpha";
            FileExtension = fileExtension;
            hashDataFilePath = FilePathGetter.GetFilePath(FileNameWithExtension);
        }

        public string Encrypt(string data)
        {
            string saltedData = AddSalt(data);
            File.WriteAllText(hashDataFilePath, StringHash(saltedData));

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(saltedData));
        }


        public string Decrypt(string dataInBase64Encoding)
        {
            if (string.IsNullOrEmpty(dataInBase64Encoding)) return null;

            if (File.Exists(hashDataFilePath))
            {
                string saltedData;

                try
                {
                    saltedData = Encoding.UTF8.GetString(Convert.FromBase64String(dataInBase64Encoding));
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                    return null;
                }

                // Совпадает ли хэш считанных данных с хэшом ранее сохраненных данных?
                return IsDataWasNotEdited(saltedData) ? AddSalt(saltedData) : null;
            }
            else return null;
        }

        public string StringHash(string data)
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

        public bool IsDataWasNotEdited(string dataAsJSON)
        {
            return StringHash(dataAsJSON) == File.ReadAllText(hashDataFilePath);
        }

        private string AddSalt(string data)
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
    }
}
