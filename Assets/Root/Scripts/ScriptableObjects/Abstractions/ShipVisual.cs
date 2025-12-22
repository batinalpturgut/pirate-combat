using System;
using Plugins.InspectorType;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects.Abstractions
{
    [Serializable]
    public struct ShipVisual
    {
        [field: SerializeField, InspectorType(typeof(ARaritySO))]
        public InspectorTypeRef RarityType { get; private set; }
        
        // TODO: Tip veya array gelecek.
        [field: SerializeField]
        public Material Material { get; private set; }
        
        [field: SerializeField]
        public Transform Particle { get; private set; }
        
        [field: SerializeField]
        public ShipModel ShipModel { get; private set; }
    }
}