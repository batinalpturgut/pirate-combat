using System;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerSOs;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using Root.Scripts.Managers.Inventory.Enums;
using Root.Scripts.Utilities.Logger;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerEffects
{
    public class MusicTowerEffect : ATowerEffect, ISoulSource
    {
        private float _timer;
        private float _soulSpawnTime;
        public int SoulDrop { get; private set; }

        public MusicTowerEffect(ATowerSO towerSO, int initialSoulDrop, float soulSpawnTime) : base(towerSO)
        {
            SoulDrop = initialSoulDrop;
            _soulSpawnTime = soulSpawnTime;
        }

        public override void HandleTargets()
        {
            _timer += Time.deltaTime;
            if (Math.Abs(_timer - _soulSpawnTime) < 0.1f)
            {
                _timer = 0f;
                InventoryManager.AddSouls(this, SoulChangeReason.TowerEffect);
            }
        }

        protected override void Initialize()
        {
        }

        public override void ApplyEffect(Hostile.HostileBlueprint enemy)
        {
        }

        public override void RemoveEffect(Hostile.HostileBlueprint enemy)
        {
        }

        public override void ApplyEffectToDefender(IDefenderEffectable defender)
        {
            throw new NotImplementedException();
        }

        public override void RemoveEffectFromDefender(IDefenderEffectable defender)
        {
            throw new NotImplementedException();
        }

        public override void Upgrade(IUpgradeDefinition upgradeDefinition)
        {
            if (upgradeDefinition is not MusicTowerSO.UpgradeDefinition musicTowerUpgrade)
            {
                Log.Console("Invalid upgrade definition set!", LogType.Error);
                return;
            }

            var allEffects = TowerBlueprint.TowerEffectHandler.GetAndRemoveAllEffects();
            SoulDrop = musicTowerUpgrade.SoulDrop;
            _soulSpawnTime = musicTowerUpgrade.SoulSpawnTime;
            _timer = 0f;
            TowerBlueprint.TowerEffectHandler.AddAll(allEffects);
        }

        protected override int ACompareTo(ATowerEffect other)
        {
            return 0;
        }
    }
}