using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Root.Scripts.Extensions;
using Root.Scripts.Services.Core.Abstractions;
using Root.Scripts.Services.Save.Abstractions.Classes;
using Root.Scripts.Services.Save.Abstractions.Interfaces;
using Root.Scripts.Services.Save.EventHandlers;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Guards;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.Logger.Enums;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;

namespace Root.Scripts.Services.Save
{
    [CreateAssetMenu(fileName = "SaveService", menuName = "Services/Save/SaveService", order = 0)]
    public class SaveService : AService
    {
        public event EventHandler<SaveOperationEventArgs> OnGameSaved;
        public event EventHandler<SaveOperationEventArgs> OnGameLoad;

        [SerializeField]
        private ASerializer serializer;

        [SerializeField]
        private AChiper chiper;

        [SerializeField]
        private string salt;

        private const int SaltSize = 16;

        protected override void OnInitialize()
        {
            IsInitialized = true;
        }
        protected override void OnStart() { }

        public void Save(params IDataRoot[] dataRoots)
        {
            Guard.Against.Null(serializer, nameof(serializer));
            Guard.Against.Null(chiper, nameof(chiper));

            foreach (IDataRoot dataRoot in dataRoots)
            {
                try
                {
                    string serializedData = serializer.Serialize(dataRoot);

                    string encryptedData = chiper.Encrypt(serializedData);
                    string hashedFileName = GenerateSHA256(dataRoot.FileName);

                    string path = Path.Combine(Application.persistentDataPath,
                        $"{hashedFileName.SetValidName()}{serializer.Extension}");

                    if (dataRoot.IsCloudData)
                    {
                        SaveToCloud(path, encryptedData);
                    }
                    else
                    {
                        SaveToLocal(path, encryptedData);
                    }

                    OnGameSaved?.Invoke(this, new SaveOperationEventArgs(dataRoot, true));
                }
                catch (Exception ex)
                {
                    Log.Console($"Failed to save {dataRoot.GetType().Name}: {ex.Message}", LogType.Error,
                        LogContext.Always);
                    OnGameSaved?.Invoke(this, new SaveOperationEventArgs(dataRoot, false, ex, ex.Message));
                }
            }
        }

        public void Load<T>(ref T dataRoot) where T : IDataRoot
        {
            Guard.Against.Null(serializer, nameof(serializer));
            Guard.Against.Null(chiper, nameof(chiper));

            string hashedFileName = GenerateSHA256(dataRoot.FileName);
            string path = Path.Combine(Application.persistentDataPath,
                $"{hashedFileName.SetValidName()}{serializer.Extension}");

            if (!File.Exists(path))
            {
                FileNotFoundException ex = new FileNotFoundException($"File not found at path: {path}");
                OnGameSaved?.Invoke(this, new SaveOperationEventArgs(dataRoot, false, ex, ex.Message));
                return;
            }

            string encryptedData = dataRoot.IsCloudData ? LoadFromCloud(path) : LoadFromLocal(path);
            string decryptedData = chiper.Decrypt(encryptedData);

            T deserializedData = (T)serializer.Deserialize<T>(decryptedData);
            dataRoot = deserializedData;
            OnGameLoad?.Invoke(this, new SaveOperationEventArgs(dataRoot, true));
        }

        // TODO: Save islemleri Saver/Loader class'lari ile yapilabilir belki. (Cloud/Local/Custom Server)
        private void SaveToLocal(string path, string content) => File.WriteAllText(path, content);
        private string LoadFromLocal(string path) => File.ReadAllText(path);

        private void SaveToCloud(string path, string content)
        {
        }

        private string LoadFromCloud(string path) => string.Empty;

        private string GenerateSHA256(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            byte[] saltedInput = new byte[saltBytes.Length + inputBytes.Length];

            Array.Copy(saltBytes, 0, saltedInput, 0, saltBytes.Length);
            Array.Copy(inputBytes, 0, saltedInput, saltBytes.Length, inputBytes.Length);

            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(saltedInput));
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            salt = EditorHelpers.SetAbsoluteLength(salt, SaltSize);
        }
#endif
    }
}