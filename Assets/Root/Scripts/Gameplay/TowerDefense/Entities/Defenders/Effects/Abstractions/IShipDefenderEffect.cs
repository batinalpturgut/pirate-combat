using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship;
using Root.Scripts.Utilities.Abstractions;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions
{
    public interface IShipDefenderEffect : IDefenderToDefenderEffect, IInitializer<ShipBlueprint>
    {
        
    }
}