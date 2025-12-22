using System.Collections.Generic;
using NaughtyAttributes;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship;
using Root.Scripts.Gameplay.TowerDefense.Projectile.Structs;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions
{
    public abstract class AShipSO : ScriptableObject, ISoulEater
    {
        [field: SerializeField, BoxGroup(Consts.InspectorTitles.REFERENCES)] 
        public ShipBlueprint ShipBlueprintPrefab { get; private set; }
        
        [field: SerializeField, BoxGroup(Consts.InspectorTitles.REFERENCES)] 
        public Sprite PreviewSprite { get; private set; }
        
        [field: SerializeField, BoxGroup(Consts.InspectorTitles.ANIMATION_SETTINGS), MinMaxSlider(1f, 10f)] 
        public Vector2 WaveOffset { get; private set; }
        
        [field: SerializeField, BoxGroup(Consts.InspectorTitles.ANIMATION_SETTINGS)] 
        public float WaveSpeed { get; private set; }

        [field: SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)] 
        public float Damage { get; private set; }

        [field: SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)] 
        public float Range { get; private set; }

        [field: SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)] 
        public float Bps { get; private set; }

        public override string ToString()
        {
            return "Stats: " +
                   "\nBase Damage: " + Damage +
                   "\nBase Range: " + Range +
                   "\nBase Bps: " + Bps;
        }

        [field: SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)] 
        public int SoulCost { get; private set; }

        [field: SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)]
        public List<ThemeKeyProjectileListPair<ACaptainSO>> ProjectileVisualList { get; private set; }

        [field: SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)] 
        public ShipVisual[] ShipVisualList { get; private set; }

        public abstract IDefenderToDefenderEffect GetDefenderEffect();
    }
}