namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions
{
    public interface IUpgradeDefinition : ISoulEater
    {
        //TODO: Range tum kuleler icin ortak bir ozellik degil. Ornek muzik kulesi
        float Range { get; set; }
    }
}