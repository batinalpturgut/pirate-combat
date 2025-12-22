using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Projectile.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Projectile.Structs;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Projectile.Structures
{
    public class KeyStructure<TKey> : IProjectileStructure<TKey>
    {
        private List<KeyProjectilePair<TKey>> _keyProjectileVisualPairList;

        public KeyStructure(List<KeyProjectilePair<TKey>> visualList)
        {
            _keyProjectileVisualPairList = visualList;
        }
        
        public GameObject GetVisual(TKey key)
        {
            return null;
        }

        public GameObject GetDefaultVisual()
        {
            return null;
        }
    }
}