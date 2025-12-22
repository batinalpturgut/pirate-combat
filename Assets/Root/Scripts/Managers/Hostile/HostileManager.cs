using System;
using System.Collections;
using System.Collections.Generic;
using Root.Scripts.Factories;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.HostileSOs;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Inventory;
using Root.Scripts.Managers.Inventory.Enums;
using Root.Scripts.Managers.Island;
using Root.Scripts.Managers.Tick;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.ScriptableObjects.Island;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Logger;
using UnityEngine;
using UnityEngine.Rendering;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;

namespace Root.Scripts.Managers.Hostile
{
    public class HostileManager : MonoBehaviour, IInitializer<AContext>
    {
        [SerializeField]
        private HostileShipSO hostileShipSO;

        [SerializeField]
        private Transform enemyPrefab;

        public ObservableList<HostileBlueprint> HostileList { get; } = new ObservableList<HostileBlueprint>();

        private GridManager _gridManager;
        private InventoryManager _inventoryManager;
        private TickManager _tickManager;
        private IslandManager _islandManager;
        private readonly List<HostileBlueprint> _enemiesToUpgradeEffects = new List<HostileBlueprint>();

        private HostileSpawner _hostileSpawner;

        private RoadBlueprint[] _roadBlueprints;
        public static int RemainingIslandHealth { get; private set; }
        public static event Action<int> OnHealthChanged;
        public static event Action<Vector3, int> OnHostileDeath;

        void IInitializer<AContext>.Initialize(AContext context)
        {
            _tickManager = context.Resolve<TickManager>();
            _gridManager = context.Resolve<GridManager>();
            _inventoryManager = context.Resolve<InventoryManager>();
            _islandManager = context.ResolveShared<IslandManager>();
        }

        private void Start()
        {
            RemainingIslandHealth = _islandManager.CurrentIslandConfig.TotalHealth;
            OnHealthChanged?.Invoke(RemainingIslandHealth);
            InitializeRoads();

#if UNITY_EDITOR
            for (int i = 0; i < _roadBlueprints.Length; i++)
            {
                List<NodePosition> hostilePath = _roadBlueprints[i].Road.Path;

                if (_roadBlueprints[i].Road.MovementType != EMovementType.Grid)
                {
                    continue;
                }

                for (int j = 0; j < hostilePath.Count; j++)
                {
                    if (!_gridManager.IsValidNodePosition(hostilePath[j]))
                    {
                        Log.Console($"Path node position isn't valid. {hostilePath[j]}", LogType.Error);
                    }

                    if (j == hostilePath.Count - 1)
                    {
                        continue;
                    }

                    if (Mathf.Abs(hostilePath[j].x - hostilePath[j + 1].x) +
                        Mathf.Abs(hostilePath[j].z - hostilePath[j + 1].z) > 2)
                    {
                        Log.Console($"Path node position isn't valid. {hostilePath[j]}", LogType.Error);
                    }
                }
            }

#endif

            _hostileSpawner = new HostileSpawner(_roadBlueprints, _islandManager.CurrentIslandConfig.WaveCount,
                this, _gridManager);
            _hostileSpawner.StartWave(0);
        }


        public void InitializeRoads()
        {
            _roadBlueprints = new RoadBlueprint[_islandManager.CurrentIslandConfig.Roads.Length];
            for (int i = 0; i < _roadBlueprints.Length; i++)
            {
                _roadBlueprints[i] = new RoadBlueprint()
                {
                    Road = _islandManager.CurrentIslandConfig.Roads[i],
                };
            }
        }

        public void PlaceEnemy2()
        {
            var hostile =
                HostileFactory.CreateAndInitialize(hostileShipSO, _islandManager.CurrentIslandConfig.Roads[0],
                    _tickManager, _gridManager,
                    this);
            HostileList.Add(hostile);
            _gridManager.GetNodeObjectWithNodePosition(hostile.Road.Path[0]).AddHostileToNode(hostile);
        }

        public void Place(AHostileSO hostileSO, Road road)
        {
            var hostile =
                HostileFactory.CreateAndInitialize(hostileSO, road, _tickManager, _gridManager, this);
            HostileList.Add(hostile);

            if (road.MovementType == EMovementType.Grid)
            {
                _gridManager.GetNodeObjectWithNodePosition(road.Path[0]).AddHostileToNode(hostile);
            }
        }

        private void MoveHostileToTheStartingPos(HostileBlueprint hostileBlueprint)
        {
            _gridManager.GetNodeObjectWithNodePosition(hostileBlueprint.Path[0]).AddHostileToNode(hostileBlueprint);
            hostileBlueprint.Movement.Reset();
        }

        public void HostilePortalEffect(HostileBlueprint hostileBlueprint, NodePosition currentNode)
        {
            _gridManager.GetNodeObjectWithNodePosition(currentNode).RemoveHostileFromNode();
            hostileBlueprint.gameObject.SetActive(false);
            AddToPortalInterruptCycle(hostileBlueprint);
        }

        private void AddToPortalInterruptCycle(HostileBlueprint hostileBlueprint)
        {
            foreach (var roadBlueprint in _roadBlueprints)
            {
                if (!roadBlueprint.Road.Equals(hostileBlueprint.Road))
                {
                    continue;
                }

                roadBlueprint.WaitList.Enqueue(hostileBlueprint);

                if (!roadBlueprint.IsInterrupted)
                {
                    roadBlueprint.IsInterrupted = true;
                    StartCoroutine(PortalInterruptHandler(roadBlueprint));
                }

                break;
            }
        }

        private IEnumerator PortalInterruptHandler(RoadBlueprint roadBlueprint)
        {
            while (roadBlueprint.WaitList.Count != 0)
            {
                while (_gridManager.GetNodeObjectWithNodePosition(roadBlueprint.Road.Path[0]).HasHostile)
                {
                    yield return null;
                }

                if (roadBlueprint.WaitList.TryDequeue(out HostileBlueprint hostile))
                {
                    MoveHostileToTheStartingPos(hostile);
                    hostile.gameObject.SetActive(true);
                }
                else
                {
                    // Art arda portala girecek elemanlar icin bekleme suresi. Eger 2 saniye boyunca
                    // portala eleman girmezse wave kaldigi yerden devam eder.
                    yield return Wait.ForSeconds(2f);
                    if (roadBlueprint.WaitList.Count == 0)
                    {
                        break;
                    }
                }

                yield return null;
            }

            roadBlueprint.IsInterrupted = false;
        }

        public void HostileDeath(HostileBlueprint hostileBlueprint, NodePosition currentNode,
            EMovementType movementType)
        {
            if (hostileBlueprint is ISoulSource soulSource)
            {
                _inventoryManager.AddSouls(soulSource, SoulChangeReason.HostileDeath);
            }

            if (movementType == EMovementType.Grid)
            {
                _gridManager.GetNodeObjectWithNodePosition(currentNode).RemoveHostileFromNode();
            }
            
            OnHostileDeath?.Invoke(hostileBlueprint.transform.position, hostileBlueprint.SoulDrop);
            RemoveHostile(hostileBlueprint);
        }

        public void HostilePathFinished(HostileBlueprint hostileBlueprint, NodePosition currentNode,
            NodePosition previousNode)
        {
            _gridManager.GetNodeObjectWithNodePosition(previousNode).RemoveHostileFromNode();
            // Log.Console($"Kaldirildi: {previousNode}");
            _gridManager.GetNodeObjectWithNodePosition(currentNode).AddHostileToNode(hostileBlueprint);
            // Log.Console($"Eklendi: {currentNode}");
            RemoveHostile(hostileBlueprint);

            if (--RemainingIslandHealth > 0)
            {
                OnHealthChanged?.Invoke(RemainingIslandHealth);
            }
            else
            {
                Log.Console("Lost.");
                // Game finished.
            }
        }

        public void HostileNodeChanged(HostileBlueprint hostileBlueprint, NodePosition currentNode,
            NodePosition previousNode)
        {
            _gridManager.GetNodeObjectWithNodePosition(previousNode).RemoveHostileFromNode();
            _gridManager.GetNodeObjectWithNodePosition(currentNode).AddHostileToNode(hostileBlueprint);
        }


        public void RemoveHostile(HostileBlueprint hostileBlueprint)
        {
            HostileList.Remove(hostileBlueprint);
            hostileBlueprint.Destroy();
        }

        public void UpdateAppliedTowerEffects<T>(T effector, Action<T> effectUpgradeLogic) where T : ATowerEffect
        {
            // Log.Console("Upgrade calisti.");

            _enemiesToUpgradeEffects.Clear();
            for (int i = 0; i < HostileList.Count; i++)
            {
                if (HostileList[i].TowerEffectHandler.RemoveForUpgrade<T>(effector))
                {
                    _enemiesToUpgradeEffects.Add(HostileList[i]);
                }
            }

            effectUpgradeLogic?.Invoke(effector);

            for (int i = 0; i < _enemiesToUpgradeEffects.Count; i++)
            {
                _enemiesToUpgradeEffects[i].TowerEffectHandler.AddForUpgrade<T>(effector);
            }
        }

        public class RoadBlueprint
        {
            public Road Road { get; set; }
            public bool IsInterrupted { get; set; }
            public Queue<HostileBlueprint> WaitList { get; private set; } = new Queue<HostileBlueprint>();
        }
    }
}