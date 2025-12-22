using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.EffectHandlers;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Inventory;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.Tick.Abstractions;
using Root.Scripts.ScriptableObjects;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower
{
    public class TowerBlueprint : MonoBehaviour, IDeployable, IUpgradable, IInitializer<ATowerSO, NodeObject>,
        IInitializer<TickManager, HostileManager, InventoryManager, GridManager>, IStandardTickable, IDefenderEffectable
    {
        [SerializeField]
        private Transform modelReference;

        private int _currentLevel;
        private ATowerSO _towerSO;
        private TickManager _tickManager;
        private InventoryManager _inventoryManager;
        private HostileManager _hostileManager;
        private GridManager _gridManager;
        private GameObject[] _visualPrefabList;
        public int ExecutionOrder => 0;
        public ATowerEffect TowerEffect { get; private set; }

        public NodeObject NodeObject { get; private set; }
        public IDefenderToDefenderEffect Apply { get; private set; }
        public TowerEffectHandler<IDefenderEffectable> TowerEffectHandler { get; private set; }


        public void Initialize(ATowerSO towerSO, NodeObject nodeObject)
        {
            _towerSO = towerSO;
            _visualPrefabList = towerSO.GetVisuals(EThemeType.Classic);
            TowerEffect = towerSO.TowerEffect;
            NodeObject = nodeObject;
            TowerEffectHandler =
                new TowerEffectHandler<IDefenderEffectable>(this,
                    (tower, effectedTower) => tower.ApplyEffectToDefender(effectedTower),
                    (tower, effectedTower) => tower.RemoveEffectFromDefender(effectedTower));

            TowerEffect.Initialize(this, _hostileManager, _inventoryManager, _gridManager);
            Apply = _towerSO.GetDefenderEffect();
            ((ITowerDefenderEffect)Apply).Initialize(this);
        }

        public void Initialize(TickManager tickManager, HostileManager hostileManager,
            InventoryManager inventoryManager, GridManager gridManager)
        {
            _tickManager = tickManager;
            _hostileManager = hostileManager;
            _inventoryManager = inventoryManager;
            _gridManager = gridManager;
        }

        private void Start()
        {
            _tickManager.Register(this);
        }

        void IStandardTickable.Tick()
        {
            TowerEffect.HandleTargets();
        }

        void IStandardTickable.FixedTick()
        {
        }

        void IStandardTickable.LateTick()
        {
        }

        public void Upgrade()
        {
            if (_currentLevel >= _towerSO.Upgrades.Count || !_inventoryManager.TryPlaceSoulEater(
                _towerSO.Upgrades[_currentLevel]))
            {
                return;
            }
            
            TowerEffect.Upgrade(_towerSO.Upgrades[_currentLevel++]);
            Build();
        }

        public void Build()
        {
            if (modelReference.childCount > 0)
            {
                Destroy(modelReference.GetChild(0).gameObject);
            }

            if (_currentLevel > _visualPrefabList.Length - 1)
            {
                Log.Console("Upgrade level didn't match with the visual prefab count.");
            }

            GameObject prefab = _visualPrefabList[_currentLevel];
            Spawner.Spawn<Transform>(prefab.transform, Vector3.zero, Quaternion.identity)
                .SetParent(modelReference, false);
        }

        public void Place()
        {
        }

        public void Remove()
        {
        }
    }
}