using Root.Scripts.ScriptableObjects.Island;
using UnityEngine;

namespace Root.Scripts.MainScene.IslandStarter.Abstractions
{
    public class AIslandStarterSO : ScriptableObject
    {
        [field: SerializeField]
        public GameObject Visual { get; private set; }
        
        [field: SerializeField]
        public string ConfigId { get; private set; }

        [field: SerializeField]
        public IslandConfigSO IslandConfigSo { get; private set; }
    }
}