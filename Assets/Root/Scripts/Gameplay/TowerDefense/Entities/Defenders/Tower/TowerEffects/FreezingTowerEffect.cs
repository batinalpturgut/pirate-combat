using System;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Tower;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerSOs;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.TriggerController;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerEffects
{
    public class FreezingTowerEffect : ATowerEffect
    {
        private static int _counter;

        private float _slowingEffectPercentage;

        public float Range { get; set; }

        private float _newPercentage;

        private readonly int _spawnCount;

        public DistanceTriggerController<FreezingTowerEffect, Hostile.HostileBlueprint> DistanceTriggerController
        {
            get;
            private set;
        }

        public FreezingTowerEffect(ATowerSO towerSO, float slowingEffectPercentage, float range) : base(towerSO)
        {
            _slowingEffectPercentage = slowingEffectPercentage;
            Range = range;
            _spawnCount = ++_counter;
        }

        protected override void Initialize()
        {
            DistanceTriggerController =
                new DistanceTriggerController<FreezingTowerEffect, Hostile.HostileBlueprint>(
                    this,
                    TowerBlueprint.gameObject,
                    HostileManager.HostileList,
                    Range,
                    onTriggerEnter: (freezingTower, hostile) =>
                    {
                        hostile.TowerEffectHandler.AddTowerEffect<FreezingTowerEffect>(freezingTower);
                    },
                    onTriggerStay: (freezingTower, hostile) =>
                    {
                        Debug.DrawRay(freezingTower.TowerBlueprint.transform.position,
                            hostile.transform.position - freezingTower.TowerBlueprint.transform.position,
                            Color.magenta);
                    },
                    onTriggerExit: (freezingTower, hostile) =>
                    {
                        hostile.TowerEffectHandler.RemoveTowerEffect<FreezingTowerEffect>(freezingTower);
                    }
                );
        }

        public override void HandleTargets()
        {
            DistanceTriggerController.Calculate();
        }

        public override void RemoveEffectFromDefender(IDefenderEffectable defender)
        {
            throw new System.NotImplementedException();
        }

        public override void Upgrade(IUpgradeDefinition upgradeDefinition)
        {
            if (upgradeDefinition is not FreezingTowerSO.UpgradeDefinition freezingTowerUpgrade)
            {
                Log.Console("Invalid upgrade definition set!", LogType.Error);
                return;
            }

            var allEffects = TowerBlueprint.TowerEffectHandler.GetAndRemoveAllEffects();
            _newPercentage = freezingTowerUpgrade.SlowingEffectPercentage;
            Range = freezingTowerUpgrade.Range;

            DistanceTriggerController.UpdateTriggerRange(Range);
            HostileManager.UpdateAppliedTowerEffects(
                this,
                tower => tower._slowingEffectPercentage = tower._newPercentage
            );
            
            TowerBlueprint.TowerEffectHandler.AddAll(allEffects);
        }

        // TODO: IEffect --> ISlowable yapilabilir?
        public override void ApplyEffect(Hostile.HostileBlueprint hostileBlueprint)
        {
            hostileBlueprint.ApplyTower.AddSlowingEffect(_slowingEffectPercentage);
        }

        public override void RemoveEffect(Hostile.HostileBlueprint hostileBlueprint)
        {
            hostileBlueprint.ApplyTower.RemoveSlowingEffect(_slowingEffectPercentage);
        }

        public override void ApplyEffectToDefender(IDefenderEffectable defender)
        {
            throw new System.NotImplementedException();
        }

        protected override int ACompareTo(ATowerEffect other)
        {
            FreezingTowerEffect otherFreezingTowerEffect = (FreezingTowerEffect)other;

            if (_slowingEffectPercentage < otherFreezingTowerEffect._slowingEffectPercentage)
            {
                return -1;
            }

            if (_slowingEffectPercentage > otherFreezingTowerEffect._slowingEffectPercentage)
            {
                return 1;
            }

            // Equal percentage. So compare the _spawnCounts;
            if (_spawnCount > otherFreezingTowerEffect._spawnCount)
            {
                return 1;
            }

            if (_spawnCount < otherFreezingTowerEffect._spawnCount)
            {
                return -1;
            }

            return 0;
        }
    }
}