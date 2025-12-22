using NaughtyAttributes;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Movements;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.HostileSOs
{
    [CreateAssetMenu(menuName = Consts.SOMenuNames.EnemyShipMenu, fileName = Consts.SOFileNames.EnemyShip)]
    public class HostileShipSO : AHostileSO
    {
        [field: SerializeField, BoxGroup(Consts.InspectorTitles.ANIMATION_SETTINGS), MinMaxSlider(1f, 10f)] 
        public Vector2 WaveOffset { get; private set; }
        
        [field: SerializeField, BoxGroup(Consts.InspectorTitles.ANIMATION_SETTINGS)] 
        public float WaveSpeed { get; private set; }
        
        public override IMovement GetMovement()
        {
            return new GridBaseMovement();
        }

        public override ITowerEffect GetTowerEffect() =>
            new Effects.Tower.HostileShip();

        public override IShipEffect GetShipEffect() =>
            new Effects.Ship.HostileShip();


        public override ISpellEffect GetSpellEffect() =>
            new Effects.Spell.HostileShip();


        public override ICaptainEffect GetCaptainEffect() => 
            new Effects.Captain.HostileShip();
    }
}