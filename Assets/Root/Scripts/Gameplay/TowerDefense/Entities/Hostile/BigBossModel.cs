using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile
{
    public class BigBossModel : AHostileModel
    {
        public override Transform Transform => transform;
        protected override void Init() { }
    }
}