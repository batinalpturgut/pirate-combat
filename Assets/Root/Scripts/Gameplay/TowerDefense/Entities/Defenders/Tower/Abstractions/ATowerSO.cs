using System;
using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.ScriptableObjects;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions
{
    public abstract class ATowerSO : ScriptableObject, ISoulEater
    {
        [field: SerializeField]
        public int SoulCost { get; private set; }

        [field: SerializeField]
        public Sprite PreviewSprite { get; private set; }

        [field: SerializeField]
        public Transform Prefab { get; private set; }

        [field: SerializeField]
        public bool IsCumulative { get; private set; }

        [SerializeField, Header("General Settings:")]
        private ThemeVisualPair[] themeVisualPairList;

        [field: SerializeField]
        protected float BaseRange { get; set; } = 5;

        public abstract List<IUpgradeDefinition> Upgrades { get; }

        public abstract ATowerEffect TowerEffect { get; }

        public abstract IDefenderToDefenderEffect GetDefenderEffect();

        [Serializable]
        public struct ThemeVisualPair
        {
            [field: SerializeField]
            public EThemeType Theme { get; set; }

            [field: SerializeField]
            public GameObject[] UpgradeLevelPrefabs { get; set; } // Upgrade visuals    
        }

        public GameObject[] GetVisuals(EThemeType themeType)
        {
            for (int i = 0; i < themeVisualPairList.Length; i++)
            {
                if (themeVisualPairList[i].Theme == themeType)
                {
                    return themeVisualPairList[i].UpgradeLevelPrefabs;
                }
            }

            return null;
        }

        public void UpdateSoulCost(int newSoulCost)
        {
            SoulCost = newSoulCost;
        }
    }
}