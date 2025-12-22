using System;
using System.Security.Cryptography;
using System.Text;
using Root.Scripts.Services.Save.Abstractions.Classes;
using UnityEngine;
using System.IO;
using Root.Scripts.Utilities;

namespace Root.Scripts.Services.Save.Chipers
{
    [CreateAssetMenu(fileName = "AESChiper", menuName = "Services/Save/Chipers/AESChiper", order = 0)]
    public class AESChiper : AChiper
    {
        private const int EncryptionKeyLength = 32;
        private const int InitializationVectorLength = 16;
        
        public override string Encrypt(string data)
        {
            using Aes aes = Aes.Create();
            
            aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            aes.IV = Encoding.UTF8.GetBytes(InitializationVector);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
            {
                streamWriter.Write(data);
            }
            
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public override string Decrypt(string data)
        {
            using Aes aes = Aes.Create();
            
            aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            aes.IV = Encoding.UTF8.GetBytes(InitializationVector);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader(cryptoStream);
            
            return streamReader.ReadToEnd();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EncryptionKey = EditorHelpers.SetAbsoluteLength(EncryptionKey, EncryptionKeyLength);
            InitializationVector = EditorHelpers.SetAbsoluteLength(InitializationVector, InitializationVectorLength);
        }
#endif
    }
}