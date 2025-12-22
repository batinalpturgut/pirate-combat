using System;
using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.ScriptableObjects.Rarity;
using Root.Scripts.Services.Save.DataModels;
using Root.Scripts.Services.Save.DataModels.InventoryModel;

namespace Root.Scripts.Managers.Inventory
{
    public class ShipSystem
    {
        public List<ShipDTO> ShipDTOs { get; } = new List<ShipDTO>();

        public ShipSystem(AShipSO[] shipSOArray, RaritySystemSO raritySystemSO)
        {
            Regenerate(shipSOArray, raritySystemSO);
        }
        
        public void Regenerate(AShipSO[] shipSOArray, RaritySystemSO raritySystemSO)
        {
            ShipDTOs.Clear();
            foreach (AShipSO shipSO in shipSOArray)
            {
                GenerateByType(shipSO, raritySystemSO);
            }  
        }

        private void GenerateByType(AShipSO shipSO, RaritySystemSO raritySystem)
        {
            Dictionary<Type, ShipTypeSettings> ships = DataRoots.Inventory.Ships;

            ShipTypeSettings shipTypeSettings = ships[shipSO.GetType()];
            List<ShipInstanceSettings> shipValues = shipTypeSettings.ShipInstances;

            for (int index = 0; index < shipValues.Count; index++)
            {
                ARaritySO raritySO = raritySystem.GetRaritySOById(shipValues[index].Rarity);

                if (raritySystem.IsDefault(raritySO))
                {
                    shipValues[index].Rarity = raritySO.Id;
                }

                ShipDTO shipDto = new ShipDTO()
                {
                    ShipSO = shipSO,
                    RaritySO = raritySO,
                    UpgradeLevel = shipTypeSettings.UpgradeLevel
                };
                ShipDTOs.Add(shipDto);
            }
        }
    }
}