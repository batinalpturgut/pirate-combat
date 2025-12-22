using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects.Rarity.Types
{
    [CreateAssetMenu(fileName = Consts.SOFileNames.RareRarity, menuName = Consts.SOMenuNames.RareRarityMenu)]
    public class RareRaritySO : ARaritySO 
    {
        public override string ToString()
        {
            return "Rare Rarity:\n" + base.ToString();
        }
    }

}