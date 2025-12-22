using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Projectile.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Projectile.Structs;
using Root.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.UIElements;

namespace Root.Scripts.Gameplay.TowerDefense.Projectile.Structures
{
    public class ThemeKeyStructure<TKey> : IProjectileStructure<TKey>
    {
        private EThemeType _currentTheme = EThemeType.Classic;
        private List<ThemeKeyProjectileListPair<TKey>> _themeKeyProjectileListPairList;

        public ThemeKeyStructure(List<ThemeKeyProjectileListPair<TKey>> visualList)
        {
            _themeKeyProjectileListPairList = visualList;
        }

        public GameObject GetVisual(TKey key)
        {
            foreach (var themeKeyProjectileVisual in _themeKeyProjectileListPairList)
            {
                if (themeKeyProjectileVisual.Theme != _currentTheme)
                {
                    continue;
                }

                foreach (var keyProjectileVisualPair in themeKeyProjectileVisual.KeyVisualPairList)
                {
                    if (keyProjectileVisualPair.Key == null)
                    {
                        if (key == null)
                        {
                            return keyProjectileVisualPair.Visual;
                        }

                        continue;
                    }

                    if (Equals(keyProjectileVisualPair.Key, key))
                    {
                        return keyProjectileVisualPair.Visual;
                    }
                }
            }

            return null;
        }

        public GameObject GetDefaultVisual()
        {
            foreach (var themeKeyProjectileVisual in _themeKeyProjectileListPairList)
            {
                if (themeKeyProjectileVisual.Theme != _currentTheme)
                {
                    continue;
                }

                return themeKeyProjectileVisual.KeyVisualPairList[0].Visual;
            }

            return null;
        }
    }
}