using Root.Scripts.Gameplay.TowerDefense.Entities.Abstractions;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions
{
    public interface IShooter : IBehaviour
    {
        HostileBlueprint Target { get; set; }
        float Range { get; set; }
        float Angle { get; }
        float ShooterRotateSpeed { get; }
        Vector3 ShootPosition { get; }
    }
}