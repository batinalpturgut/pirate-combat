using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship.Enums;
using Root.Scripts.ScriptableObjects.Abstractions;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects.Rarity.Abstractions
{
    public class RaritySONotUsed : ScriptableObject
    {
        [field: SerializeField]
        public float RangePercentage { get; private set; }
        
        [field: SerializeField]
        public float DamagePercentage { get; private set; }
        
        [field: SerializeField]
        public float BpsPercentage { get; private set; }

        public float GetDamage(AShipSO shipSO)
        {
            return shipSO.Damage * (100 + DamagePercentage) / 100;
        }

        public float GetRange(AShipSO shipSO)
        {
            return shipSO.Range * (100 + RangePercentage) / 100;
        }

        public float GetBps(AShipSO shipSO)
        {
            return shipSO.Bps * (100 + BpsPercentage) / 100;
        }
    }
}