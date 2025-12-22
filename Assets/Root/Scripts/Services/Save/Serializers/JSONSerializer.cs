using System;
using Newtonsoft.Json;
using Root.Scripts.Services.Save.Abstractions.Classes;
using Root.Scripts.Services.Save.Abstractions.Interfaces;
using UnityEngine;

namespace Root.Scripts.Services.Save.Serializers
{
    [CreateAssetMenu(fileName = "JSONSerializer", menuName = "Services/Save/Serializers/JSONSerializer", order = 0)]
    public class JSONSerializer : ASerializer
    {
        public override string Serialize(IDataRoot dataRoot)
        {
            return JsonConvert.SerializeObject(dataRoot);
        }
        public override IDataRoot Deserialize<T>(string deserializable)
        {
            return JsonConvert.DeserializeObject<T>(deserializable);
        }
        
        public override IDataRoot Deserialize(string deserializable, Type type)
        {
            return (IDataRoot)JsonConvert.DeserializeObject(deserializable, type);
        }
    }
}