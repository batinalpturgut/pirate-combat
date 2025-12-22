using System;
using System.IO;
using Root.Scripts.Services.Save.Abstractions.Interfaces;
using UnityEngine;

namespace Root.Scripts.Services.Save.Abstractions.Classes
{
    public abstract class ASerializer : ScriptableObject
    {
        [field: SerializeField]
        public string Extension { get; private set; }
        public abstract string Serialize(IDataRoot dataRoot);
        public abstract IDataRoot Deserialize<T>(string deserializable) where T : IDataRoot;
        public abstract IDataRoot Deserialize(string deserializable, Type type);


        private void OnValidate()
        {
            // TODO: Daha duzgun bir kontrol yapilip helper'a tasinabilir.
            if (!Extension.StartsWith("."))
            {
                Extension = "." + Extension;
            }
        }
    }
}