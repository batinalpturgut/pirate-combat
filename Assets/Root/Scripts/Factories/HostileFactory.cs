using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Tick;
using Root.Scripts.ScriptableObjects.Island;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;

namespace Root.Scripts.Factories
{
    public static class HostileFactory
    {
        public static HostileBlueprint CreateAndInitialize(AHostileSO hostileSO, Road road,
            TickManager tickManager, GridManager gridManager, HostileManager hostileManager)
        {
            HostileBlueprint spawnedHostileBlueprint =
                Spawner.Spawn<HostileBlueprint, TickManager, GridManager, HostileManager>(hostileSO.HostileBlueprint,
                    gridManager.GetWorldPosition(road.Path[0]), Quaternion.identity,
                    tickManager, gridManager, hostileManager);
            spawnedHostileBlueprint.Initialize(hostileSO, road);
            return spawnedHostileBlueprint;
        }
    }
}