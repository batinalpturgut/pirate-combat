using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects.Rarity.Types
{
    [CreateAssetMenu(fileName = Consts.SOFileNames.UncommonRarity, menuName = Consts.SOMenuNames.UncommonRarityMenu)]
    public class UncommonRaritySO : ARaritySO 
    {
        public override string ToString()
        {
            return "Uncommon Rarity:\n" + base.ToString();
        }
    }
}