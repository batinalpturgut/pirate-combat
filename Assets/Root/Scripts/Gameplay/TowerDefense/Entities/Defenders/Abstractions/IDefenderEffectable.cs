using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.EffectHandlers;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions
{
    public interface IDefenderEffectable
    {
        IDefenderToDefenderEffect Apply { get; }
        TowerEffectHandler<IDefenderEffectable> TowerEffectHandler { get; }
    }
}