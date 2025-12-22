using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects.Rarity.Types
{
    [CreateAssetMenu(fileName = Consts.SOFileNames.EpicRarity, menuName = Consts.SOMenuNames.EpicRarityMenu)]
    public class EpicRaritySO : ARaritySO 
    {
        public override string ToString()
        {
            return "Epic Rarity:\n" + base.ToString();
        }
    }
}