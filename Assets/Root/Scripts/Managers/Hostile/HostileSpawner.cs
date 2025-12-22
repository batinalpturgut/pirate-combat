using System.Collections;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;
using Root.Scripts.Managers.Grid;

namespace Root.Scripts.Managers.Hostile
{
    public class HostileSpawner
    {
        private HostileManager _hostileManager;
        private GridManager _gridManager;
        private HostileManager.RoadBlueprint[] _roadBlueprints;
        private int _waveCount;


        public HostileSpawner(HostileManager.RoadBlueprint[] roadBlueprints, int waveCount,
            HostileManager hostileManager, GridManager gridManager)
        {
            _roadBlueprints = roadBlueprints;
            _waveCount = waveCount;
            _hostileManager = hostileManager;
            _gridManager = gridManager;
        }

        public void StartWave(int waveNum)
        {
            foreach (var road in _roadBlueprints)
            {
                _hostileManager.StartCoroutine(StartWave(waveNum, road));
            }
        }

        private IEnumerator StartWave(int waveNum, HostileManager.RoadBlueprint roadBlueprint)
        {
            var hostileConfigList = roadBlueprint.Road.WaveConfigList[waveNum].HostileConfigList;
            var path = roadBlueprint.Road.Path;
            foreach (var hostileConfig in hostileConfigList)
            {
                if (roadBlueprint.IsInterrupted)
                {
                    yield return null;
                }

                SpawnCondition spawnCondition =
                    SpawnCondition.GetSpawnCondition(this, hostileConfig.Condition,
                        hostileConfig.IntParameter);

                // TODO: Interrupt conditionlari sifirlasin mi yoksa  kaldiklari yerden devam etsinler mi?
                // Ornegin 5 saniye sonra spawnlanacak bir hostile var. 3 saniye gectikten sonra road interrupt'a 
                // ugruyor. 
                while (!spawnCondition.Condition.Invoke())
                {
                    yield return null;
                }

                while (roadBlueprint.IsInterrupted)
                {
                    yield return null;
                }

                while (roadBlueprint.Road.MovementType == EMovementType.Grid && 
                       _gridManager.GetNodeObjectWithNodePosition(path[0]).HasHostile)
                {
                    yield return null;
                }

                _hostileManager.Place(hostileConfig.Hostile, roadBlueprint.Road);
                spawnCondition.ReturnSpawnCondition();
            }
        }
    }
}