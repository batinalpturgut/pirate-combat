using Root.Scripts.Services.Save.Abstractions.Interfaces;

namespace Root.Scripts.Services.Save.DataModels.PlayerStatsModel
{
    public class PlayerStats: IDataRoot
    {
        public int Money { get; set; } = 0;
        public int Diamond { get; set; } = 0;
        public string FileName => nameof(PlayerStats);
        public bool IsCloudData => false;
    }
}