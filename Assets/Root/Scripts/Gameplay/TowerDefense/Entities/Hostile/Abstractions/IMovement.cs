using System.Collections.Generic;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Utilities.Abstractions;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions
{
    public interface IMovement : IInitializer<HostileBlueprint, List<NodePosition>, GridManager, HostileManager>
    {
        float CurrentSpeed { get; }
        void Move();
        void ApplyPortal();
        void OnDeath();
        void Reset();
    }
}