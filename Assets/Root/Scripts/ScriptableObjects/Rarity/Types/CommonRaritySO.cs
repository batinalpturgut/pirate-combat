using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects.Rarity.Types
{
    [CreateAssetMenu(fileName = Consts.SOFileNames.CommonRarity, menuName = Consts.SOMenuNames.CommonRarityMenu)]
    public class CommonRaritySO : ARaritySO
    {
        public override string ToString()
        {
            return "Common Rarity:\n" + base.ToString();

        }
    }
}