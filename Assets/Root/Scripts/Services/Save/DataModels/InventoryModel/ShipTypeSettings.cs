using System.Collections.Generic;

namespace Root.Scripts.Services.Save.DataModels.InventoryModel
{
    public class ShipTypeSettings
    {
        public byte UpgradeLevel { get; set; }
        public List<string> OwnedSkins { get; set; }
        public List<ShipInstanceSettings> ShipInstances { get; set; }
    }
}