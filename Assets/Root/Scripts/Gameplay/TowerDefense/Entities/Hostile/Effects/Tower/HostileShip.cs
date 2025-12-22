using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Effects.Tower
{
    public class HostileShip : ITowerEffect
    {
        private HostileBlueprint _hostileBlueprint;

        public void Initialize(HostileBlueprint hostileBlueprint)
        {
            _hostileBlueprint = hostileBlueprint;
        }

        public void AddSlowingEffect(float slowingEffectPercentage)
        {
            _hostileBlueprint.MoveSpeed *= (100 - slowingEffectPercentage) / 100;
        }

        public void RemoveSlowingEffect(float slowingEffectPercentage)
        {
            _hostileBlueprint.MoveSpeed = (_hostileBlueprint.MoveSpeed * 100) / (100 - slowingEffectPercentage);
        }
    }
}