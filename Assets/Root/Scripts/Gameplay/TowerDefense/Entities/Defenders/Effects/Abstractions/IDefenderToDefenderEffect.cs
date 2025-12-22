namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions
{
    public interface IDefenderToDefenderEffect
    {
        void RangeBoost(float rangeBoostPercentage);
        void RemoveRangeBoost(float rangeBoostPercentage);
    }
}