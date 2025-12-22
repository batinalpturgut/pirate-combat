namespace Root.Scripts.Gameplay.TowerDefense.Entities.Abstractions
{
    public interface IFloatable : IBehaviour
    {
        float WaveOffset { get; set; }
        float WaveSpeed { get; set; }
        int DirectionMultiplier { get; set; }
    }
}