using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerSOs;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.Logger.Enums;
using Root.Scripts.Utilities.TriggerController;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerEffects
{
    public class RumTowerEffect : ATowerEffect
    {
        private static int _counter;
        private float _rangeBoostPercentage;
        private int _spawnCount;
        private int _range;
        private GridTriggerController<RumTowerEffect, IDefenderEffectable> _triggerController;


        public RumTowerEffect(ATowerSO towerSO, float rangeBoostPercentage, int range) : base(towerSO)
        {
            _rangeBoostPercentage = rangeBoostPercentage;
            _range = range;
            _spawnCount = _counter++;
        }

        public override void HandleTargets()
        {
            // TODO: Surekli kontrol yapilmayabilir. Sadece defender'lar eklenip cikarildiginda event olarak ateslenebilir.
            _triggerController.Calculate();
        }

        public override void ApplyEffect(HostileBlueprint hostileBlueprint)
        {
        }

        public override void RemoveEffect(HostileBlueprint hostileBlueprint)
        {
        }

        public override void ApplyEffectToDefender(IDefenderEffectable defender)
        {
            defender.Apply.RangeBoost(_rangeBoostPercentage);
        }

        public override void RemoveEffectFromDefender(IDefenderEffectable defender)
        {
            defender.Apply.RemoveRangeBoost(_rangeBoostPercentage);
        }

        public override void Upgrade(IUpgradeDefinition upgradeDefinition)
        {
            if (upgradeDefinition is not RumTowerSO.UpgradeDefinition rumTowerUpgrade)
            {
                Log.Console("Invalid upgrade definition set!", LogType.Error);
                return;
            }

            var allEffects = TowerBlueprint.TowerEffectHandler.GetAndRemoveAllEffects();
            foreach (var entity in _triggerController.AllInRangeEntities())
            {
                entity.TowerEffectHandler.RemoveTowerEffect<RumTowerEffect>(this);
            }

            _triggerController.ClearAllInRangeEntities();

            _range = (int)rumTowerUpgrade.Range;
            _triggerController.UpdateTriggerRange(rumTowerUpgrade.Range);
            _rangeBoostPercentage = rumTowerUpgrade.RangeBoostPercentage;
            TowerBlueprint.TowerEffectHandler.AddAll(allEffects);
        }

        protected override void Initialize()
        {
            _triggerController =
                new GridTriggerController<RumTowerEffect, IDefenderEffectable>(this,
                    TowerBlueprint.NodeObject.NodePosition, _range, GridManager,
                    (tower, effectable) => effectable.TowerEffectHandler.AddTowerEffect<RumTowerEffect>(tower),
                    (tower, effectable) => effectable.TowerEffectHandler.RemoveTowerEffect<RumTowerEffect>(tower));
        }

        protected override int ACompareTo(ATowerEffect other)
        {
            RumTowerEffect otherRumTowerEffect = (RumTowerEffect)other;

            if (_rangeBoostPercentage < otherRumTowerEffect._rangeBoostPercentage)
            {
                return -1;
            }

            if (_rangeBoostPercentage > otherRumTowerEffect._rangeBoostPercentage)
            {
                return 1;
            }

            if (_spawnCount > otherRumTowerEffect._spawnCount)
            {
                return 1;
            }

            if (_spawnCount < otherRumTowerEffect._spawnCount)
            {
                return -1;
            }

            return 0;
        }
    }
}