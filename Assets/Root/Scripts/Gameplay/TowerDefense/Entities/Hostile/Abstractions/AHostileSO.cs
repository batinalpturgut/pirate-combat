using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Root.Scripts.ScriptableObjects;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions
{
    public abstract class AHostileSO : ScriptableObject
    {
        [field: SerializeField, BoxGroup(Consts.InspectorTitles.REFERENCES)]
        public Transform HostileBlueprint { get; private set; }

        [field: SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)]
        public float Health { get; private set; }

        [field: SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)]
        public int SoulDrop { get; private set; }

        [field: SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)]
        public float MoveSpeed { get; private set; }

        [field: SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)]
        public float RotateSpeed { get; private set; }

        [field: SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)]
        public float ColliderRange { get; private set; }

        [SerializeField, BoxGroup(Consts.InspectorTitles.SETTINGS)]
        private ThemeVisualPair[] themeVisualPairList;

        public abstract IMovement GetMovement();
        public abstract ITowerEffect GetTowerEffect();
        public abstract IShipEffect GetShipEffect();
        public abstract ISpellEffect GetSpellEffect();
        public abstract ICaptainEffect GetCaptainEffect();

        [Serializable]
        private struct ThemeVisualPair
        {
            [field: SerializeField]
            public EThemeType ThemeType { get; private set; }

            [field: SerializeField]
            public AHostileModel Visual { get; private set; }
        }

        protected readonly Dictionary<EThemeType, AHostileModel> _themeVisualPairDict =
            new Dictionary<EThemeType, AHostileModel>();


        public virtual AHostileModel GetVisual(EThemeType themeType)
        {
            return _themeVisualPairDict[themeType];
        }

        private void OnValidate()
        {
            _themeVisualPairDict.Clear();
            foreach (ThemeVisualPair themeVisualPair in themeVisualPairList)
            {
                _themeVisualPairDict[themeVisualPair.ThemeType] = themeVisualPair.Visual;
            }
        }
    }
}