using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.TowerEffects;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Tower
{
    public class MusicTowerDefenderEffect : ITowerDefenderEffect 
    {
        private MusicTowerEffect _musicTowerEffect;

        public void Initialize(TowerBlueprint towerBlueprint)
        {
            _musicTowerEffect = (MusicTowerEffect)towerBlueprint.TowerEffect;
        }

        public void RangeBoost(float rangeBoostPercentage)
        {
        }

        public void RemoveRangeBoost(float rangeBoostPercentage)
        {
        }
    }
}