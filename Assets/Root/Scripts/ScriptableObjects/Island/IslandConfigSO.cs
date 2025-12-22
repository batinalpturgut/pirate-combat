using System;
using System.Collections.Generic;
using Root.Scripts.Utilities.Logger;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;

namespace Root.Scripts.ScriptableObjects.Island
{
    /// <summary>
    /// Island ile alakali tum bilgileri tutan sinif.
    /// </summary>
    [CreateAssetMenu(fileName = "IslandConfig", menuName = "Island/IslandConfig")]
    public class IslandConfigSO : ScriptableObject
    {
        [field: SerializeField]
        public string Id { get; private set; }

        [field: SerializeField]
        public int WaveCount { get; private set; }

        [field: SerializeField]
        public int TotalHealth { get; private set; }

        [field: SerializeField]
        public GameObject AllVisuals { get; private set; }

        [field: SerializeField]
        public Road[] Roads { get; private set; }

        private void OnValidate()
        {
            for (int i = 0; i < Roads.Length; i++)
            {
                if (Roads[i].WaveConfigList.Count != WaveCount)
                {
                    Log.Console("Roads wave count have to match with island total wave count.", LogType.Error);
                }
            }
        }
    }
}