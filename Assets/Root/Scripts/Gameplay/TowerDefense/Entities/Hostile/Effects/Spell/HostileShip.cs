using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Effects.Spell
{
    public class HostileShip : ISpellEffect
    {
        private HostileBlueprint _hostileBlueprint;

        public void Initialize(HostileBlueprint hostileBlueprint)
        {
            _hostileBlueprint = hostileBlueprint;
        }

        public void PortalEffect()
        {
            _hostileBlueprint.Movement.ApplyPortal();
        }
    }
}