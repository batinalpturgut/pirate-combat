using NaughtyAttributes;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects.Abstractions
{
    public abstract class ARaritySO : ScriptableObject
    {
        [field: SerializeField, ReadOnly]
        public string Id { get; private set; }
        
        [field: SerializeField]
        public float RangePercentage { get; private set; }

        [field: SerializeField]
        public float DamagePercentage { get; private set; }

        [field: SerializeField]
        public float BpsPercentage { get; private set; }
        public override string ToString()
        {
            return "Stats: " +
                   "\nDamage Percentage: " + DamagePercentage + 
                   "\nRange Percentage: " + RangePercentage +
                   "\nBps Percentage: " + BpsPercentage;
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);
            Id = UnityEditor.AssetDatabase.AssetPathToGUID(assetPath);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}