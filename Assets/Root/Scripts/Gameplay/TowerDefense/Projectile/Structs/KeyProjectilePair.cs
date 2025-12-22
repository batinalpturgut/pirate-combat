using System;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Projectile.Structs
{
    [Serializable]
    public struct KeyProjectilePair<TKey>
    {
        [field: SerializeField]
        public TKey Key { get; private set; }

        [field: SerializeField]
        public GameObject Visual { get; private set; }
    }
}