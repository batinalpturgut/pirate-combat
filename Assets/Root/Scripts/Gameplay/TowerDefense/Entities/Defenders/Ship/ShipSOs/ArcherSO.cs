using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship.ShipSOs
{
    [CreateAssetMenu(fileName = Consts.SOFileNames.Archer, menuName = Consts.SOMenuNames.ArcherMenu)]
    public class ArcherSO : AShipSO
    {
        Material[] _allMaterials;

        public override string ToString()
        {
            return "Archer\n" + base.ToString();
        }

        public override IDefenderToDefenderEffect GetDefenderEffect()
        {
            return new ShipDefenderEffect();
        }
    }
}