using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities.Curves;
using Root.Scripts.Utilities.Logger;
using UnityEngine;

namespace Root.Scripts.ScriptableObjects.Island
{
    /// <summary>
    /// Path'i ve path'lerden gececek wave'leri tutan sinif.
    /// </summary>
    [CreateAssetMenu(fileName = "Road", menuName = "Island/Road")]
    public class Road : ScriptableObject
    {
        [field: SerializeField]
        public EMovementType MovementType { get; private set; }

        [field: SerializeField, ShowIf(nameof(CanShowSplineComponent))]
        public SplineComponent SplineComponent { get; private set; }

        [field: SerializeField, ShowIf(nameof(CanShowPath))]
        public List<NodePosition> Path { get; private set; }

        [field: SerializeField]
        public List<WaveConfig> WaveConfigList { get; private set; } = new List<WaveConfig>();

        private bool CanShowSplineComponent => MovementType == EMovementType.Spline;
        private bool CanShowPath => MovementType == EMovementType.Grid;

        [Serializable]
        public struct WaveConfig
        {
            [field: SerializeField]
            public List<HostileConfig> HostileConfigList { get; private set; }
        }

        [Serializable]
        public struct HostileConfig
        {
            [field: SerializeField]
            public ESpawnCondition Condition { get; private set; }

            [field: SerializeField, AllowNesting, ShowIf(nameof(CanShowIntParameter))]
            public int IntParameter { get; private set; }

            [field: SerializeField]
            public AHostileSO Hostile { get; private set; }

            private bool CanShowIntParameter =>
                Condition == ESpawnCondition.WaitForSeconds;
        }

        private void OnValidate()
        {
            if (MovementType != EMovementType.Spline)
            {
                return;
            }

            if (SplineComponent.SplineCurve.Waypoints == null)
            {
                return;
            }
            
            Path.Clear();
            const float stepSize = 0.001f;
            float currentStep = 0;
            while (currentStep <= 1)
            {
                Vector3 point = SplineComponent.SplineCurve.GetPoint(currentStep);
                NodePosition operand = new NodePosition();
                operand.Set(point.x, point.y, point.z);
                Path.Add(operand);
                currentStep += stepSize;
            }
        }
    }
}