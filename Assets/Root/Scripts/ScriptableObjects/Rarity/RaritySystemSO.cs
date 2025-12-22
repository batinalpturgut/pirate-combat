using System;
using System.Collections.Generic;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Logger;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;
using Random = UnityEngine.Random;

namespace Root.Scripts.ScriptableObjects.Rarity
{
    [CreateAssetMenu(fileName = Consts.SOFileNames.RaritySystem, menuName = Consts.SOMenuNames.RaritySystemMenu)]
    public class RaritySystemSO : ScriptableObject
    {
        [SerializeField] 
        private ARaritySO defaultRarity;
        
        [field: SerializeField] 
        public RarityConfig[] RarityConfigs { get; private set; }

        [SerializeField, HideInInspector]
        private Dictionary<Type, ARaritySO[]> raritySOMap = new Dictionary<Type, ARaritySO[]>();

        public ARaritySO GetRandomRaritySO()
        {
#if UNITY_EDITOR
            float sum = 0;
            foreach (RarityConfig temp in RarityConfigs)
            {
                sum += temp.ProbabilityPercentage;
            }

            if (Math.Abs(sum - 100f) > 0.1f)
            {
                Log.Console("The total of the Probability Percentages must be %100.", LogType.Error);
            }
#endif

            RarityConfig selectedRarityType = GetRandomRarityType(); 
            ARaritySO[] currentRarities = raritySOMap[selectedRarityType.RarityType.GetType];
            return currentRarities[Random.Range(0, currentRarities.Length)];
        }

        public ARaritySO GetRaritySOById(string id)
        {
            foreach (RarityConfig config in RarityConfigs)
            {
                foreach (ARaritySO raritySo in config.Rarities)
                {
                    if (raritySo.Id == id)
                    {
                        return raritySo;
                    }
                }
            }

            return defaultRarity;
        }

        public bool IsDefault(ARaritySO raritySO)
        {
            return raritySO == defaultRarity;
        }

        private RarityConfig GetRandomRarityType()
        {
            float rand = Random.value;
            float cumulativeProbability = 0.0f;

            foreach (RarityConfig rarity in RarityConfigs)
            {
                cumulativeProbability += rarity.ProbabilityPercentage / 100;
                if (rand < cumulativeProbability)
                {
                    return rarity;
                }
            }

            return null;
        }

        private void OnValidate()
        {
            raritySOMap.Clear();
            foreach (RarityConfig rarityConfig in RarityConfigs)
            {
                if (rarityConfig.RarityType == null || rarityConfig.RarityType.GetType == null)
                {
                    return;
                }

                raritySOMap[rarityConfig.RarityType.GetType] = rarityConfig.Rarities;
            }
        }
    }
}