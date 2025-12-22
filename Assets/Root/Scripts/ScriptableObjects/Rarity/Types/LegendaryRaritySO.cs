using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects.Rarity.Types
{
    [CreateAssetMenu(fileName = Consts.SOFileNames.LegendaryRarity, menuName = Consts.SOMenuNames.LegendaryRarityMenu)]
    public class LegendaryRaritySO : ARaritySO 
    {
        public override string ToString()
        {
            return "Legendary Rarity:\n" + base.ToString();
        }
    }
}