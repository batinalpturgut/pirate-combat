using System;
using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Tower;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerEffects;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerSOs
{
    
    [CreateAssetMenu(fileName = Consts.SOFileNames.RumTower, menuName = Consts.SOMenuNames.RumTowerMenu)]
    public class RumTowerSO : ATowerSO
    {
        [field: SerializeField, Header("Tower Specific Settings:")]
        private float BaseRangeBoostPercentage { get; set; }

        [SerializeField, Header("\r")]
        private List<UpgradeDefinition> upgrades;

        [Serializable]
        public struct UpgradeDefinition : IUpgradeDefinition
        {
            [field: SerializeField]
            public float Range { get; set; }

            [field: SerializeField]
            public int SoulCost { get; set; }

            [field: SerializeField]
            public float RangeBoostPercentage { get; set; }
        }

        public override List<IUpgradeDefinition> Upgrades { get; } = new List<IUpgradeDefinition>();
        public override ATowerEffect TowerEffect => new RumTowerEffect(this, BaseRangeBoostPercentage, (int)BaseRange);

        public override IDefenderToDefenderEffect GetDefenderEffect()
        {
            return new RumTowerDefenderEffect();
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