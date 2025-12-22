using System;
using System.Collections.Generic;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects.Abstractions
{
    public abstract class ASpellSO : ScriptableObject
    {
        [field: SerializeField]
        public float CooldownDuration { get; private set; }

        [field: SerializeField]
        public float SpellDuration { get; private set; }

        [field: SerializeField]
        public Sprite PreviewSprite { get; set; }

        [SerializeField, Header("General Settings:")]
        private ThemeVisualPair[] themeVisualPairList;

        private Dictionary<EThemeType, Transform> _themeVisualPairDict = new Dictionary<EThemeType, Transform>();

        [Serializable]
        private struct ThemeVisualPair
        {
            [field: SerializeField]
            public EThemeType ThemeType { get; private set; }

            [field: SerializeField]
            public Transform VisualPrefab { get; private set; }
        }

        public abstract void Apply(NodePosition appliedNode, HostileManager hostileManager,
            GridManager gridManager);

        public Transform GetVisual(EThemeType themeType)
        {
            return _themeVisualPairDict[themeType];
        }

        private void OnValidate()
        {
            _themeVisualPairDict.Clear();
            foreach (ThemeVisualPair themeVisualPair in themeVisualPairList)
            {
                _themeVisualPairDict[themeVisualPair.ThemeType] = themeVisualPair.VisualPrefab;
            }
        }
    }
}