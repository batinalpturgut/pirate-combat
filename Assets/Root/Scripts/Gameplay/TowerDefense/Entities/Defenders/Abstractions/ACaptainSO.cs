using System;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;
using Root.Scripts.ScriptableObjects;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions
{
    public abstract class ACaptainSO : ScriptableObject, ISoulEater
    {
        [field: SerializeField]
        public int SoulCost { get; private set; }

        [field: SerializeField]
        public Sprite PreviewSprite { get; private set; }

        [field: SerializeField]
        public Transform CaptainBlueprint { get; private set; }

        [SerializeField]
        private ThemeVisualPair[] themeVisualPairs;

        [Serializable]
        public struct ThemeVisualPair
        {
            [field: SerializeField]
            public EThemeType ThemeType { get; private set; }
            
            [field: SerializeField]
            public GameObject Prefab { get; private set; }
        }

        public abstract void ApplyEffect(HostileBlueprint hostileBlueprint);

        public GameObject GetVisual(EThemeType themeType)
        {
            
            for (int i = 0; i < themeVisualPairs.Length; i++)
            {
                if (themeType == themeVisualPairs[i].ThemeType)
                {
                    return themeVisualPairs[i].Prefab;
                }
            }
            return null;
        }
    }
}