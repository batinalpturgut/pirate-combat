using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerEffects;
using Root.Scripts.Utilities.Logger;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Tower
{
    public class FreezingTowerDefenderEffect : ITowerDefenderEffect
    {
        private FreezingTowerEffect _freezingTowerEffect;

        public void Initialize(TowerBlueprint towerBlueprint)
        {
            _freezingTowerEffect = (FreezingTowerEffect)towerBlueprint.TowerEffect;
        }

        public void RangeBoost(float rangeBoostPercentage)
        {
            Log.Console("Eklendi");
            Log.Console($"Range: {_freezingTowerEffect.Range}");
            _freezingTowerEffect.Range *= (100 + rangeBoostPercentage) / 100;
            Log.Console($"Range: {_freezingTowerEffect.Range}");
            _freezingTowerEffect.DistanceTriggerController.UpdateTriggerRange(_freezingTowerEffect.Range);
        }

        public void RemoveRangeBoost(float rangeBoostPercentage)
        {
            Log.Console("Kaldirildi");
            Log.Console($"Range: {_freezingTowerEffect.Range}");
            _freezingTowerEffect.Range /= (100 + rangeBoostPercentage) / 100;
            Log.Console($"Range: {_freezingTowerEffect.Range}");
            _freezingTowerEffect.DistanceTriggerController.UpdateTriggerRange(_freezingTowerEffect.Range);
        }
    }
}