using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Effects.Captain
{
    public class HostileShip : ICaptainEffect
    {
        private HostileBlueprint _hostileBlueprint;
        public void Initialize(HostileBlueprint hostileBlueprint)
        {
            _hostileBlueprint = hostileBlueprint;
        }
        
        public void PoisonousCaptainEffect()
        {
            
        }

        public void IceCaptainEffect()
        {
            
        }
    }
}