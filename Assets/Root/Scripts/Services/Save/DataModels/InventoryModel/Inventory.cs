using System;
using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship.ShipSOs;
using Root.Scripts.ScriptableObjects.Rarity.Types;
using Root.Scripts.Services.Save.Abstractions.Interfaces;

namespace Root.Scripts.Services.Save.DataModels.InventoryModel
{
    public class Inventory : IDataRoot
    {
        public Dictionary<Type, ShipTypeSettings> Ships { get; set; } =
            new Dictionary<Type, ShipTypeSettings>()
            {
                {
                    typeof(ArcherSO), new ShipTypeSettings()
                    {
                        ShipInstances = new List<ShipInstanceSettings>()
                        {
                            new ShipInstanceSettings()
                        },
                        UpgradeLevel = 1
                    }
                },
                {
                    typeof(HeavySO), new ShipTypeSettings()
                    {
                        ShipInstances = new List<ShipInstanceSettings>()
                        {
                            new ShipInstanceSettings(),
                            new ShipInstanceSettings()
                            {
                                Rarity = "a2c96b86bf3610c46a48822fc810e438" //Legendary
                            },
                            
                        },
                        UpgradeLevel = 1
                    }
                }
            };

        public string FileName => nameof(Inventory);
        public bool IsCloudData => false;
    }
}