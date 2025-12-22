using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions.Abstractions;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions
{
    public interface ICaptainEffect : IEffect
    {
        void PoisonousCaptainEffect();
        void IceCaptainEffect();
    }
}