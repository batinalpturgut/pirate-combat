using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects.Island
{
    /// <summary>
    /// Tum island'larin bilgisini tutan sinif.
    /// </summary>
    [CreateAssetMenu(fileName = "IslandConfigLookupTable", menuName = "Island/IslandConfigLookupTable")]
    public class IslandConfigLookupTable : ScriptableObject
    {
        [field: SerializeField, Expandable]
        public List<IslandConfigSO> ConfigList { get; private set; } = new List<IslandConfigSO>();

        private readonly Dictionary<string, IslandConfigSO> _islandConfigSODict = new Dictionary<string, IslandConfigSO>();

        public IslandConfigSO GetIslandConfig(string id)
        {
            return _islandConfigSODict[id];
        }

        private void OnValidate()
        {
            _islandConfigSODict.Clear();
            for (int i = 0; i < ConfigList.Count; i++)
            {
                _islandConfigSODict[ConfigList[i].Id] = ConfigList[i];
            }
        }
    }
}