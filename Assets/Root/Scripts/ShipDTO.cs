using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.ScriptableObjects.Abstractions;

namespace Root.Scripts
{
    public class ShipDTO 
    {
        public AShipSO ShipSO { get; set; }
        public ARaritySO RaritySO { get; set; }
        public byte UpgradeLevel { get; set; }
    }
}