using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Captain.CaptainSOs
{
    [CreateAssetMenu(fileName = Consts.SOFileNames.PoisonousCaptain, menuName = Consts.SOMenuNames.PoisonousCaptainMenu)]
    public class PoisonousCaptainSO : ACaptainSO
    {
        public override void ApplyEffect(HostileBlueprint hostileBlueprint)
        {
            hostileBlueprint.ApplyCaptain.PoisonousCaptainEffect();
        }
    }
}