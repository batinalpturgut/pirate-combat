using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Captain;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Effects.Ship
{
    public class HostileShip : IShipEffect
    {
        private HostileBlueprint _hostileBlueprint;

        public void Initialize(HostileBlueprint hostileBlueprint)
        {
            _hostileBlueprint = hostileBlueprint;
        }

        public void Damage(float damage)
        {
            TakeDamage(damage);
        }

        public void DamageWithCaptainEffect(float damage, CaptainBlueprint captainBlueprint)
        {
            TakeDamage(damage);
            captainBlueprint.ApplyCaptainEffect(_hostileBlueprint);
        }

        private void TakeDamage(float damage)
        {
            if (!_hostileBlueprint.IsAlive)
            {
                return;
            }

            _hostileBlueprint.Health -= damage;
            if (!(_hostileBlueprint.Health <= 0))
            {
                return;
            }

            _hostileBlueprint.Health = 0f;
            _hostileBlueprint.IsAlive = false;
            _hostileBlueprint.Movement.OnDeath();
        }
    }
}