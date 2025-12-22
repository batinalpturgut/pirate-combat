using System;
using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Tower;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerEffects;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerSOs
{
    [CreateAssetMenu(fileName = Consts.SOFileNames.FreezingTower, menuName = Consts.SOMenuNames.FreezingTowerMenu)]
    public class FreezingTowerSO : ATowerSO
    {
        [field: SerializeField, Header("Tower Specific Settings:")]
        private float BaseSlowingEffectPercentage { get; set; }

        [SerializeField, Header("\r")]
        private List<UpgradeDefinition> upgrades;

        public override ATowerEffect TowerEffect =>
            new FreezingTowerEffect(this, BaseSlowingEffectPercentage, BaseRange);

        public override List<IUpgradeDefinition> Upgrades { get; } = new List<IUpgradeDefinition>();

        [Serializable]
        public struct UpgradeDefinition : IUpgradeDefinition
        {
            [field: SerializeField]
            public float Range { get; set; }

            [field: SerializeField]
            public int SoulCost { get; set; }

            [field: SerializeField]
            public float SlowingEffectPercentage { get; set; }
        }

        public override IDefenderToDefenderEffect GetDefenderEffect()
        {
            return new FreezingTowerDefenderEffect();
        }


        private void OnValidate()
        {
            Upgrades.Clear();
            foreach (UpgradeDefinition upgrade in upgrades)
            {
                Upgrades.Add(upgrade);
            }
        }
    }
}