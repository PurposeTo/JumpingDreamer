using System;
using Desdiene.GameDataAsset.Data;
using Desdiene.GameDataAsset.Encryption;
using Desdiene.JsonConvertorWrapper;
using Desdiene.SuperMonoBehaviourAsset;

namespace Desdiene.GameDataAsset.ConcreteStorages
{
    public class LocalStorageCrypto<T> : LocalStorage<T>
         where T : GameData, new()
    {

        private readonly JsonEncryption jsonEncryption;


        public LocalStorageCrypto(SuperMonoBehaviour superMonoBehaviour,
            string fileName,
            string fileExtension,
            IJsonConvertor<T> jsonConvertor)
            : base(superMonoBehaviour,
                  fileName,
                  fileExtension,
                  jsonConvertor) 
        {
            jsonEncryption = new JsonEncryption(FileName, FileExtension);
        }

        protected override void Read(Action<string> jsonDataCallback)
        {
            LoadAndDecryptData(jsonDataCallback.Invoke);
        }

        protected override void Write(string jsonData)
        {
            string modifiedData = jsonEncryption.Encrypt(jsonData);
            base.Write(modifiedData);
        }

        private void LoadAndDecryptData(Action<string> jsonDataCallback)
        {
            deviceDataLoader.LoadDataFromDevice(receivedData =>
            {
                jsonDataCallback?.Invoke(jsonEncryption.Decrypt(receivedData));
            });
        }
    }
}
