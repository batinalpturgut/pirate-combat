using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower;
using Root.Scripts.Utilities.Abstractions;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions
{
    public interface ITowerDefenderEffect : IDefenderToDefenderEffect, IInitializer<TowerBlueprint>
    {
        
    }
}