using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;
namespace Root.Scripts.Factories
{
    public static class ShipFactory
    {
        public static ShipBlueprint CreateAndInitialize(ShipDTO shipDto, Vector3 worldPos, NodeObject nodeObject, TickManager tickManager,
            HostileManager hostileManager)
        {
            ShipBlueprint spawnedShipBlueprint = Spawner.Spawn<ShipBlueprint, TickManager, HostileManager>(
                shipDto.ShipSO.ShipBlueprintPrefab.transform,
                worldPos, 
                Quaternion.identity, 
                tickManager, hostileManager);
            
            spawnedShipBlueprint.Initialize(shipDto.ShipSO, shipDto.RaritySO, nodeObject);
            spawnedShipBlueprint.Build();
            
            return spawnedShipBlueprint;
        }
    }
}