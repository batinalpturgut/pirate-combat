using System;
using Root.Scripts.ScriptableObjects;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Projectile.Structs
{
    [Serializable]
    public struct ThemeKeyProjectileListPair<TKey>
    {
        [field: SerializeField]
        public KeyProjectilePair<TKey>[] KeyVisualPairList { get; private set; }

        [field: SerializeField]
        public EThemeType Theme { get; private set; }
    }
}