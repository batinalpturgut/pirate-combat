using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Movements;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Curves;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.HostileSOs
{
    [CreateAssetMenu(menuName = "Hostiles/BigBoss", fileName = "BigBoss")]
    public class BigBossSO : AHostileSO
    {
        [field: SerializeField]
        public SplineComponent SplineComponent { get; private set; }
        public override IMovement GetMovement()
        {
            return new SplineBasedMovement();
        }

        public override ITowerEffect GetTowerEffect() => new Effects.Tower.HostileShip();
        public override IShipEffect GetShipEffect() => new Effects.Ship.HostileShip();
        public override ISpellEffect GetSpellEffect() => new Effects.Spell.HostileShip();
        public override ICaptainEffect GetCaptainEffect() => new Effects.Captain.HostileShip();

    }
}