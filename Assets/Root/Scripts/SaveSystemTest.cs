using System;
using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship.ShipSOs;
using Root.Scripts.Services.Core;
using Root.Scripts.Services.Save;
using Root.Scripts.Services.Save.DataModels;
using Root.Scripts.Services.Save.DataModels.InventoryModel;
using Root.Scripts.Utilities.Logger;
using UnityEngine;

namespace Root.Scripts
{
    public class SaveSystemTest : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.K))
            {
                DataRoots.Inventory.Ships = new Dictionary<Type, ShipTypeSettings>()
                {
                    {
                        typeof(ArcherSO), new ShipTypeSettings()
                        {
                            UpgradeLevel = 5,
                            ShipInstances = new List<ShipInstanceSettings>()
                            {
                                new ShipInstanceSettings()
                                {
                                    Durability = 5,
                                }
                            }
                        }
                    }
                };
                
                GameServices.Get<SaveService>().Save(DataRoots.Inventory);
            }
            else if (Input.GetKeyUp(KeyCode.L))
            {
                Log.Console($"Inventory Count: {DataRoots.Inventory.Ships.Count}");
            }
        }
    }
}