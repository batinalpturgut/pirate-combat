using System;
using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Tower;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerEffects;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerSOs
{
    [CreateAssetMenu(fileName = Consts.SOFileNames.MusicTower, menuName = Consts.SOMenuNames.MusicTowerMenu)]
    public class MusicTowerSO : ATowerSO
    {
        // Bu kule muzik caldikca etrafinda ruhlar ucusacak. Dunyadaki basibos ruhlari muzik ile toplayan kule.
        [field: SerializeField]
        private int BaseSoulDrop { get; set; }

        [field: SerializeField]
        private float BaseSoulSpawnTime { get; set; }

        public override List<IUpgradeDefinition> Upgrades { get; } = new List<IUpgradeDefinition>();

        [SerializeField]
        public List<UpgradeDefinition> upgrades = new List<UpgradeDefinition>();

        public override ATowerEffect TowerEffect => new MusicTowerEffect(this, BaseSoulDrop, BaseSoulSpawnTime);

        public override IDefenderToDefenderEffect GetDefenderEffect()
        {
            return new MusicTowerDefenderEffect();
        }

        [Serializable]
        public struct UpgradeDefinition : IUpgradeDefinition
        {
            [field: SerializeField]
            public int SoulDrop { get; private set; }

            [field: SerializeField]
            public float SoulSpawnTime { get; private set; }

            public float Range { get; set; }
            
            [field: SerializeField]
            public int SoulCost { get; set; }
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