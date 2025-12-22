using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Abstractions
{
    public interface IShootable
    {
        MonoBehaviour MonoBehaviour { get; }
        float ColliderRange { get; }
    }
}