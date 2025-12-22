using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Inventory;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;

namespace Root.Scripts.Factories
{
    public static class TowerFactory
    {
        public static TowerBlueprint CreateAndInitialize(ATowerSO towerSO, Vector3 worldPos, NodeObject nodeObject,
            TickManager tickManager,
            HostileManager hostileManager, InventoryManager inventoryManager, GridManager gridManager)
        {
            TowerBlueprint towerBlueprint =
                Spawner.Spawn<TowerBlueprint, TickManager, HostileManager, InventoryManager, GridManager>(towerSO.Prefab, worldPos,
                    Quaternion.identity, tickManager, hostileManager, inventoryManager, gridManager);

            towerBlueprint.Initialize(towerSO, nodeObject);
            towerBlueprint.Build();
            return towerBlueprint;
        }
    }
}