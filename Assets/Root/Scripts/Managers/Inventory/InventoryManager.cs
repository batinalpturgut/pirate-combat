using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using Root.Scripts.Managers.Inventory.Enums;
using Root.Scripts.ScriptableObjects.Rarity;
using UnityEngine;

namespace Root.Scripts.Managers.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] 
        private int initialSoul;

        [SerializeField] 
        private AShipSO[] shipSOReferences;

        [field: SerializeField] 
        public ATowerSO[] towerSOReferences { get; private set; }

        [field: SerializeField] 
        public ACaptainSO[] captainSOReferences { get; private set; }

        [SerializeField] 
        private RaritySystemSO raritySystemSO;

        private ShipSystem _shipSystem;
        private SoulSystem _soulSystem;
        public List<ShipDTO> ShipDTOs => _shipSystem.ShipDTOs;

        private void Awake()
        {
            _shipSystem = new ShipSystem(shipSOReferences, raritySystemSO);
            _soulSystem = new SoulSystem(initialSoul);
        }

        public void AddSouls(ISoulSource soulSource, SoulChangeReason soulChangeReason = SoulChangeReason.Undefined) =>
            _soulSystem.AddSouls(soulSource, soulChangeReason);
        public bool TryPlaceSoulEater(ISoulEater soulEater) => _soulSystem.TryPlaceSoulEater(soulEater);
    }
}