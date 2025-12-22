using Root.Scripts.Factories;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.Tick.Abstractions;
using UnityEngine;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;
using Root.Scripts.Gameplay.TowerDefense.Spells;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Inventory;
using Root.Scripts.Managers.Island;
using Root.Scripts.Managers.Spell;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Spawner;

namespace Root.Scripts.Managers.Grid
{
    public class GridManager : MonoBehaviour, IStandardTickable, IInitializer<AContext>
    {
        [SerializeField]
        private Transform nodeTextPrefab;

        [field: SerializeField]
        public Transform NodeTextParent { get; private set; }

        [field: SerializeField]
        public float CellSize { get; private set; }

        public int ExecutionOrder => 0;
        private GridSystem _gridSystem;

        private HostileManager _hostileManager;
        private TickManager _tickManager;
        private SpellManager _spellManager;
        private InventoryManager _inventoryManager;
        private IslandManager _islandManager;

        public void Initialize(AContext context)
        {
            _tickManager = context.Resolve<TickManager>();
            _hostileManager = context.Resolve<HostileManager>();
            _spellManager = context.Resolve<SpellManager>();
            _inventoryManager = context.Resolve<InventoryManager>();
            _islandManager = context.ResolveShared<IslandManager>();
            _gridSystem = new GridSystem(10, 10, CellSize, _islandManager.CurrentIslandConfig.Roads, this);
            Spawner.Spawn<Transform>(_islandManager.CurrentIslandConfig.AllVisuals.transform,
                _islandManager.CurrentIslandConfig.AllVisuals.transform.position, Quaternion.identity);
            _gridSystem.CreateNodeTextObjects(nodeTextPrefab);
        }

        private void Start()
        {
            _tickManager.Register(this);
        }

        public void Tick()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _hostileManager.PlaceEnemy2();
            }
        }

        public void PlaceTower(ATowerSO towerSO, NodeObject nodeObject)
        {
            if (!_inventoryManager.TryPlaceSoulEater(towerSO))
            {
                return;
            }

            Vector3 gridWorldPosition = _gridSystem.GetWorldPosition(nodeObject.NodePosition);
            TowerBlueprint towerBlueprint =
                TowerFactory.CreateAndInitialize(towerSO, gridWorldPosition, nodeObject, _tickManager,
                    _hostileManager, _inventoryManager, this);
            nodeObject.AddDeployableToNode(towerBlueprint);
        }

        public void PlaceCaptain(ACaptainSO captainSO, ShipBlueprint ship, Vector3 worldPosition)
        {
            if (!_inventoryManager.TryPlaceSoulEater(captainSO))
            {
                return;
            }

            CaptainFactory.CreateAndInitialize(captainSO, worldPosition, ship);
        }

        public void PlaceShip(ShipDTO shipDto, NodeObject nodeObject)
        {
            if (!_inventoryManager.TryPlaceSoulEater(shipDto.ShipSO))
            {
                return;
            }

            Vector3 gridWorldPosition = _gridSystem.GetWorldPosition(nodeObject.NodePosition);
            ShipBlueprint spawnedShipBlueprint =
                ShipFactory.CreateAndInitialize(shipDto, gridWorldPosition, nodeObject, _tickManager, _hostileManager);
            nodeObject.AddDeployableToNode(spawnedShipBlueprint);
        }


        public void PlaceSpell(SpellBlueprint spellBlueprint, NodePosition nodePosition)
        {
            _spellManager.CastSpell(spellBlueprint, nodePosition);
        }

        public bool IsValidNodePosition(NodePosition targetNodePosition)
        {
            return _gridSystem.IsValidNodePosition(targetNodePosition);
        }

        public bool TryGetNodePosition(Vector3 worldPos, out NodePosition nodePosition)
        {
            NodePosition temp = _gridSystem.GetNodePosition(worldPos);

            if (IsValidNodePosition(temp))
            {
                nodePosition = temp;
                return true;
            }

            nodePosition = default;
            return false;
        }

        public bool TryGetHostileWithNodePosition(NodePosition nodePosition, out HostileBlueprint enemy)
        {
            if (!IsValidNodePosition(nodePosition))
            {
                enemy = null;
                return false;
            }

            NodeObject nodeObject = GetNodeObjectWithNodePosition(nodePosition);
            if (nodeObject.TryGetHostile(out HostileBlueprint enemy2))
            {
                enemy = enemy2;
                return true;
            }

            enemy = null;
            return false;
        }

        public NodeObject GetNodeObjectWithNodePosition(NodePosition nodePosition)
        {
            return _gridSystem.GetNodeObjectWithNodePosition(nodePosition);
        }

        public bool TryGetNodeObjectEntity<T>(NodePosition nodePosition, out T entity)
        {
            NodeObject nodeObject = GetNodeObjectWithNodePosition(nodePosition);
            if (!(nodeObject.HasDeployable || nodeObject.HasHostile))
            {
                entity = default;
                return false;
            }

            if (nodeObject.TryGetDeployable(out T entity2))
            {
                entity = entity2;
                return true;
            }

            if (nodeObject.TryGetHostile(out HostileBlueprint enemy))
            {
                if (enemy is T enemy1)
                {
                    entity = enemy1;
                    return true;
                }
            }

            entity = default;
            return false;
        }

        public NodePosition GetNodePosition(Vector3 worldPosition) => _gridSystem.GetNodePosition(worldPosition);
        public Vector3 GetWorldPosition(NodePosition nodePosition) => _gridSystem.GetWorldPosition(nodePosition);

        void IStandardTickable.FixedTick()
        {
        }

        void IStandardTickable.LateTick()
        {
        }

        private void OnDestroy()
        {
            _tickManager.Unregister(this);
        }
    }
}