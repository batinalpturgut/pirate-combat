using Root.Scripts.Gameplay.TowerDefense.Projectile.Abstractions;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Projectile.Structures
{
    public class SingleStructure<TKey> : IProjectileStructure<TKey>
    {
        private GameObject _projectile;

        public SingleStructure(GameObject projectile)
        {
            _projectile = projectile;
        }
        
        
        public GameObject GetVisual(TKey key)
        {
            return _projectile;
        }

        public GameObject GetDefaultVisual()
        {
            return _projectile;
        }
    }
}