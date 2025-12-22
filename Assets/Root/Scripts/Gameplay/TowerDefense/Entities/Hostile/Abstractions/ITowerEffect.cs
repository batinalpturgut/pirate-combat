using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions.Abstractions;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions
{
    public interface ITowerEffect : IEffect
    {
        void AddSlowingEffect(float slowingEffectPercentage);

        void RemoveSlowingEffect(float slowingEffectPercentage);
    }
}