using System;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Inventory;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities.Abstractions;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions
{
    public abstract class ATowerEffect : IInitializer<TowerBlueprint, HostileManager, InventoryManager, GridManager>,
        IComparable<ATowerEffect>
    {
        protected HostileManager HostileManager;
        protected GridManager GridManager;
        protected TowerBlueprint TowerBlueprint;
        protected InventoryManager InventoryManager;
        private readonly ATowerSO _towerSO;
        public bool IsCumulative => _towerSO.IsCumulative;

        protected ATowerEffect(ATowerSO towerSO)
        {
            _towerSO = towerSO;
        }

        public void Initialize(TowerBlueprint towerBlueprint, HostileManager hostileManager,
            InventoryManager inventoryManager, GridManager gridManager)
        {
            HostileManager = hostileManager;
            TowerBlueprint = towerBlueprint;
            InventoryManager = inventoryManager;
            GridManager = gridManager;
            Initialize();
        }

        public abstract void HandleTargets();
        protected abstract void Initialize();
        public abstract void ApplyEffect(Hostile.HostileBlueprint hostileBlueprint);
        public abstract void RemoveEffect(Hostile.HostileBlueprint hostileBlueprint);
        public abstract void ApplyEffectToDefender(IDefenderEffectable defender);
        public abstract void RemoveEffectFromDefender(IDefenderEffectable defender);
        public abstract void Upgrade(IUpgradeDefinition upgradeDefinition);
        protected abstract int ACompareTo(ATowerEffect other);

        public int CompareTo(ATowerEffect other)
        {
            return ACompareTo(other);
        }
    }
}