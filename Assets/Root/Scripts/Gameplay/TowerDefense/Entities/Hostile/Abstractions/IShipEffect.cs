using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Captain;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions.Abstractions;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions
{
    public interface IShipEffect : IEffect
    {
        void Damage(float damage);

        void DamageWithCaptainEffect(float damage, CaptainBlueprint captainBlueprint);
    }
}