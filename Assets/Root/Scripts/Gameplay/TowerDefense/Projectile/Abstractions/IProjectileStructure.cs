using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Projectile.Abstractions
{
    public interface IProjectileStructure<TKey>
    {
        GameObject GetVisual(TKey key);
        GameObject GetDefaultVisual();
    }
}