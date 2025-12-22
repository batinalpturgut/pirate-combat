using Root.Scripts.Services.Core;
using Root.Scripts.Services.Save.DataModels.InventoryModel;
using Root.Scripts.Services.Save.DataModels.PlayerStatsModel;

namespace Root.Scripts.Services.Save.DataModels
{
    public static class DataRoots
    {
        static DataRoots()
        {
            GameServices.Get<SaveService>().Load(ref Inventory);
            GameServices.Get<SaveService>().Load(ref PlayerStats);
        }

        public static readonly Inventory Inventory = new Inventory();
        public static readonly PlayerStats PlayerStats = new PlayerStats();
    }
}