using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Captain;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;

namespace Root.Scripts.Factories
{
    public static class CaptainFactory
    {
        public static CaptainBlueprint CreateAndInitialize(ACaptainSO captainSO, Vector3 worldPos, ShipBlueprint shipBlueprint)
        {
            CaptainBlueprint spawnedCaptainBlueprint = Spawner.Spawn<CaptainBlueprint>(captainSO.CaptainBlueprint, Vector3.zero,
                Quaternion.identity);

            spawnedCaptainBlueprint.transform.SetParent(shipBlueprint.transform, false);
            spawnedCaptainBlueprint.transform.localPosition =
                shipBlueprint.ShipModel.CaptainPosition;
            spawnedCaptainBlueprint.Initialize(captainSO);
            shipBlueprint.CaptainBlueprint = spawnedCaptainBlueprint;
            return spawnedCaptainBlueprint;
        }
    }
}