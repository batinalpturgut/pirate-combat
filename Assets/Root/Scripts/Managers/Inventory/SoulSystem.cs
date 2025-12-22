using System;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using Root.Scripts.Managers.Inventory.Enums;
using Root.Scripts.Utilities.Logger;

namespace Root.Scripts.Managers.Inventory
{
    public class SoulSystem
    {
        public static int TotalSouls { get; private set; }
        public static event Action<int, SoulChangeReason> OnSoulChanged;

        public SoulSystem(int initialSoul)
        {
            TotalSouls = initialSoul;
            OnSoulChanged?.Invoke(TotalSouls, SoulChangeReason.Undefined);
        }

        public bool TryPlaceSoulEater(ISoulEater soulEater)
        {
            if (soulEater.SoulCost > TotalSouls)
            {
                return false;
            }

            TotalSouls -= soulEater.SoulCost;
            OnSoulChanged?.Invoke(TotalSouls, SoulChangeReason.Undefined);
            return true;
        }

        public void AddSouls(ISoulSource soulSource, SoulChangeReason soulChangeReason)
        {
            TotalSouls += soulSource.SoulDrop;
            OnSoulChanged?.Invoke(TotalSouls, soulChangeReason);
        }
    }
}