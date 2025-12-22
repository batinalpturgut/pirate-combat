using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship.ShipSOs
{
    [CreateAssetMenu(fileName = Consts.SOFileNames.Heavy, menuName = Consts.SOMenuNames.HeavyMenu)]
    public class HeavySO : AShipSO
    {
        public override string ToString()
        {
            return "Heavy\n" + base.ToString();
        }

        public override IDefenderToDefenderEffect GetDefenderEffect()
        {
            return new ShipDefenderEffect();
        }
    }
}