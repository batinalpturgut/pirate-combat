using System;
using System.Collections.Generic;
using System.Linq;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Tower.Abstractions;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.Logger.Enums;
using TMPro;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.EffectHandlers
{
    public class TowerEffectHandler<T1>
    {
        private Dictionary<Type, SortedSet<ATowerEffect>> _towerEffects = // In range towers.
            new Dictionary<Type, SortedSet<ATowerEffect>>();

        private T1 _appliedEntity;
        private Action<ATowerEffect, T1> _applyEffect;
        private Action<ATowerEffect, T1> _removeEffect;
        private Dictionary<Type, SortedSet<ATowerEffect>> _allEffectors;

        public TowerEffectHandler(T1 appliedEntity, Action<ATowerEffect, T1> applyEffect,
            Action<ATowerEffect, T1> removeEffect)
        {
            _appliedEntity = appliedEntity;
            _applyEffect = applyEffect;
            _removeEffect = removeEffect;
        }

        public void AddTowerEffect<T>(ATowerEffect towerEffect)
        {
            if (_towerEffects.TryGetValue(typeof(T), out SortedSet<ATowerEffect> rangedEffects))
            {
                if (towerEffect.IsCumulative)
                {
                    // Cumulative ise strongest tower'in ne oldugundan bagimsiz range'inde olan tum tower'larin 
                    // etkisi uygulanir.
                    rangedEffects.Add(towerEffect);
                    _applyEffect?.Invoke(towerEffect, _appliedEntity);
                    return;
                }

                // Cumulative degilse yeni gelen tower rangedEffects'e eklenir. Ve strongest'mi diye kontrol edilir.
                ATowerEffect currentEffectingTower = rangedEffects.Max;
                rangedEffects.Add(towerEffect);
                ATowerEffect strongestTower = rangedEffects.Max;
                if (towerEffect == strongestTower)
                {
                    // Yeni eklenen tower artik strongest. Dolayisiyla eski strongest tower varsa etkisi kaldirilip
                    // yenisi eklendi.

                    if (currentEffectingTower != null)
                    {
                        _removeEffect?.Invoke(currentEffectingTower, _appliedEntity);
                    }

                    _applyEffect?.Invoke(strongestTower, _appliedEntity);
                }
            }
            else
            {
                _towerEffects[typeof(T)] = new SortedSet<ATowerEffect> { towerEffect };
                _applyEffect?.Invoke(_towerEffects[typeof(T)].Max, _appliedEntity);
            }
        }

        public void RemoveTowerEffect<T>(ATowerEffect towerEffect)
        {
            SortedSet<ATowerEffect> rangedEffects = _towerEffects[typeof(T)];

#if UNITY_EDITOR
            if (rangedEffects == null || rangedEffects.Count == 0 || !rangedEffects.Contains(towerEffect))
            {
                Log.Console($"There is no {nameof(T)} effect in {this}.");
                return;
            }
#endif
            if (towerEffect.IsCumulative)
            {
                // Eger cumulative degilse, strongest olmasindan bagimsiz effect'i uygulanmistir. Dolayisiyla effect
                // kaldirilir ve agactan cikarilir. 
                _removeEffect?.Invoke(towerEffect, _appliedEntity);
                rangedEffects.Remove(towerEffect);
                return;
            }

            // Eger cumulative degilse range'inden cikilan tower, strongest'mi diye kontrol edilir.
            ATowerEffect currentStrongestTower = rangedEffects.Max;
            if (currentStrongestTower == towerEffect)
            {
                // Range'inden cikilan tower, strongest tower ise, effect uyguladigi anlamina gelir. Bu yuzden
                // uygulanan effect kaldirilir.
                _removeEffect?.Invoke(currentStrongestTower, _appliedEntity); // Old removed strongest tower.
                rangedEffects.Remove(currentStrongestTower);

                if (rangedEffects.Count != 0)
                {
                    // Eger range'inde oldugu baska towerlar varsa o tower'larin strongest'inin
                    // effect'i uygulanir.
                    _applyEffect?.Invoke(rangedEffects.Max, _appliedEntity); // New strongest tower.
                }
            }
            else
            {
                // Eger range'inde oldugu tower strongest tower degilse zaten effect uygulamiyordur. Dolayisiyla
                // sadece agactan cikarilir.
                rangedEffects.Remove(towerEffect);
            }
        }

        public void AddForUpgrade<T>(ATowerEffect towerEffect)
        {
            if (_towerEffects.TryGetValue(typeof(T), out SortedSet<ATowerEffect> rangedEffects))
            {
                if (towerEffect.IsCumulative || rangedEffects.Count <= 0)
                {
                    // Etkisi guncellendikten sonra tekrardan uygulanir.
                    rangedEffects.Add(towerEffect);
                    _applyEffect?.Invoke(towerEffect, _appliedEntity);
                    return;
                }

                ATowerEffect currentMaxTower = rangedEffects.Max;
                rangedEffects.Add(towerEffect);
                if (rangedEffects.Max == towerEffect)
                {
                    // Upgrade isleminden sonra yeni strongest tower, upgrade edilmis tower olursa
                    // eski strongest tower'in effect'i kaldirilip yeni strongest tower'in effect'inin uygulanmasi
                    // saglanir.
                    _removeEffect?.Invoke(currentMaxTower, _appliedEntity);
                    _applyEffect?.Invoke(towerEffect, _appliedEntity);
                }
            }
            else
            {
                throw new Exception($"Related sorted set on {this} can't found.");
            }
        }

        public bool RemoveForUpgrade<T>(ATowerEffect towerEffect)
        {
            if (_towerEffects.TryGetValue(typeof(T), out SortedSet<ATowerEffect> rangedEffects))
            {
                if (!rangedEffects.Contains(towerEffect))
                {
                    // Agacta degilse yani enemy upgrade edilecek  tower'in range'inde degilse yapilacak bir
                    // islem yoktur.
                    return false;
                }

                if (towerEffect.IsCumulative || rangedEffects.Count == 1)
                {
                    // Cumulative ve agacta ise etkisi vardir. Etkisi kaldirilir ve agactan cikarilir.
                    _removeEffect?.Invoke(towerEffect, _appliedEntity);
                    rangedEffects.Remove(towerEffect);
                    return true;
                }

                if (towerEffect == rangedEffects.Max)
                {
                    // Upgrade edilecek tower, strongest tower ise effect'ini kaldirir. Daha sonra agactan cikarir.
                    // yeni strongest tower'in effect'ini uygular.
                    _removeEffect?.Invoke(towerEffect, _appliedEntity);
                    rangedEffects.Remove(towerEffect);
                    _applyEffect?.Invoke(rangedEffects.Max, _appliedEntity);
                    return true;
                }

                // Eger strongest tower degilse, islem cumulative olmadigi icin herhangi bir effect'i de yoktur.
                // Dolayisiyla agactan kaldirilir.
                rangedEffects.Remove(towerEffect);
                return true;
            }

            return false;
        }

        // Etki uygulanan tower, upgrade edilirse kullanilacak metot.
        // Tower upgrade edildiginde, ona uygulanan tum tower'larin effect'i kaldirilir. Ve etki saglanmasi beklenen
        // tum tower'larin bir kopyasi gonderilir.
        public Dictionary<Type, SortedSet<ATowerEffect>> GetAndRemoveAllEffects()
        {
            _allEffectors = new Dictionary<Type, SortedSet<ATowerEffect>>(_towerEffects);

            foreach (var towerEffectPair in _towerEffects)
            {
                SortedSet<ATowerEffect> towerEffectSet = towerEffectPair.Value;

                foreach (var towerEffect in towerEffectSet)
                {
                    if (towerEffect.IsCumulative)
                    {
                        _removeEffect?.Invoke(towerEffect, _appliedEntity);
                    }
                    else
                    {
                        _removeEffect?.Invoke(towerEffectSet.Max, _appliedEntity);
                        break;
                    }
                }
            }

            _towerEffects.Clear();
            return _allEffectors;
        }
        
        // Tum etkiler tekrardan uygulanir.
        public void AddAll(Dictionary<Type, SortedSet<ATowerEffect>> allTowers)
        {
            foreach (var towerEffectPair in allTowers)
            {
                Type type = towerEffectPair.Key;
                SortedSet<ATowerEffect> towerEffectSet = towerEffectPair.Value;

                if (towerEffectSet == null)
                {
                    Log.Console("Related sorted set can't found. Effects can't applied.", LogType.Error);
                    return;
                }

                _towerEffects[type] = new SortedSet<ATowerEffect>(towerEffectSet);
                
                foreach (var towerEffect in towerEffectSet)
                {
                    if (towerEffect.IsCumulative)
                    {
                        _applyEffect?.Invoke(towerEffect, _appliedEntity);
                    }
                    else
                    {
                        _applyEffect?.Invoke(towerEffectSet.Max, _appliedEntity);
                        break;
                    }
                }
            }
        }
    }
}