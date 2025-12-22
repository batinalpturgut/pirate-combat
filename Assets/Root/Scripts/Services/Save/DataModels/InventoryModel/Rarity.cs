using System;

namespace Root.Scripts.Services.Save.DataModels.InventoryModel
{
    public class Rarity
    {
        // Silinecek. Tanimlar hali hazirda var. Id kaydedilecek.
        public Type Type { get; set; }
        public float RangePercentage { get; set; }
        public float DamagePercentage { get; set; }
        public float BpsPercentage { get; set; }
    }
}