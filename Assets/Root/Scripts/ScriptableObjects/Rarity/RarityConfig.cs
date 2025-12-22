using System;
using Plugins.InspectorType;
using Root.Scripts.ScriptableObjects.Abstractions;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects.Rarity
{
    [Serializable]
    public class RarityConfig
    {
        [field: SerializeField, InspectorType(typeof(ARaritySO))]
        public InspectorTypeRef RarityType { get; private set; }
        [field: SerializeField]
        public float ProbabilityPercentage { get; private set; }
        [field: SerializeField]
        public ARaritySO[] Rarities { get; private set; }
    }
}