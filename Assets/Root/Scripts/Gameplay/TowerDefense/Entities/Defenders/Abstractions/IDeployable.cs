using Root.Scripts.Grid;
using Root.Scripts.Utilities.Abstractions;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions
{
    public interface IDeployable
    {
        void Place();
        void Remove();
    }
}