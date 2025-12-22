using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects
{
    public class ShipDefenderEffect : IShipDefenderEffect 
    {
        private ShipBlueprint _shipBlueprint;

        public void Initialize(ShipBlueprint shipBlueprint)
        {
            _shipBlueprint = shipBlueprint;
        }

        public void RangeBoost(float rangeBoostPercentage)
        {
            ((IShooter)_shipBlueprint).Range *= (100 + rangeBoostPercentage) / 100;
            _shipBlueprint.DistanceTriggerController.UpdateTriggerRange(((IShooter)_shipBlueprint).Range);
        }

        public void RemoveRangeBoost(float rangeBoostPercentage)
        {
            ((IShooter)_shipBlueprint).Range /= (100 + rangeBoostPercentage) / 100;
            _shipBlueprint.DistanceTriggerController.UpdateTriggerRange(((IShooter)_shipBlueprint).Range);
        }
    }
}