using UnityEngine;

namespace Root.Scripts.Services.Save.Abstractions.Classes
{
    public abstract class AChiper : ScriptableObject
    {
        [field: SerializeField]
        protected string EncryptionKey { get; set; }
        
        [field: SerializeField]
        protected string InitializationVector { get; set; }

        public abstract string Encrypt(string data);
        public abstract string Decrypt(string data);
    }
}