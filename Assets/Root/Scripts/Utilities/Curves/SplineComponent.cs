using System;
using PrimeTween;
using Root.Scripts.Helpers.Runtime;
using Root.Scripts.Utilities.Curves.Enums;
using UnityEditor;
using UnityEngine;

namespace Root.Scripts.Utilities.Curves
{
    [DisallowMultipleComponent]
    public class SplineComponent : MonoBehaviour
    {
        [field: SerializeField, Header("Settings")] 
        public Vector3[] Points { get; private set; }
        
        [field: SerializeField] 
        public ESplineType SplineType { get; private set; } = ESplineType.Centripetal;

        [field: SerializeField, Range(0f, 1f)] 
        public float StartValue { get; private set; } = 0f;

        [field: SerializeField, Range(0f, 1f)] 
        public float EndValue { get; private set; } = 1f;

        [field: SerializeField] 
        public float Duration { get; private set; } = 10f;

        [field: SerializeField] 
        public Ease Ease { get; private set; } = Ease.Linear;

        [field: SerializeField] 
        public int Cycles { get; private set; } = 0;

        [field: SerializeField] 
        public CycleMode CycleMode { get; private set; } = CycleMode.Restart;

        [field: SerializeField] 
        public bool IsClosed { get; private set; } = false;

        [field: SerializeField, Range(1, 100)] 
        public int Resolution { get; private set; } = 100;

        [field: SerializeField] 
        public bool LookAhead { get; private set; } = true;

        [field: SerializeField] 
        public AxisConstraint AxisConstraint { get; private set; } = AxisConstraint.None;
        
        [field: SerializeField]
        public float StartDelay { get; private set; } = 0f;

        [field: SerializeField] 
        public float EndDelay { get; private set; } = 0f;

        [field: SerializeField] 
        public bool UseUnscaledTime { get; private set; } = false;

        [Header("Gizmo Settings")] 
        [SerializeField] private Color waypointColor = Color.red;
        [SerializeField] private float waypointSize = 1f;
        [SerializeField] private Color splineColor = Color.blue;
        [SerializeField] private float splineThickness = 5f;

        [SerializeField, HideInInspector] 
        private SplineCurve splineCurve;
        public SplineCurve SplineCurve => splineCurve;

        public Tween GetTween(Transform target, Action<Transform,int> onWaypointChange = null)
        {
            return PrimeTweenHelpers.Path(
                target: target,
                startValue: StartValue,
                endValue: EndValue,
                ease: Ease,
                duration: Duration,
                splineCurve: splineCurve,
                cycles: Cycles,
                cycleMode: CycleMode,
                startDelay: StartDelay,
                endDelay: EndDelay,
                useUnscaledTime: UseUnscaledTime,
                lookAhead: LookAhead,
                axisConstraint: AxisConstraint,
                onWaypointChange: onWaypointChange);
        }

        private void OnValidate()
        {
            if (Points == null || Points.Length <= 0)
            {
                return;
            }
            
            UpdateSplineCurve();
        }

        private void UpdateSplineCurve()
        {
            Vector3[] worldPoints = new Vector3[Points.Length];
            for (int i = 0; i < Points.Length; i++)
            {
                worldPoints[i] = transform.TransformPoint(Points[i]);
            }

            splineCurve = new SplineCurve(worldPoints, IsClosed, SplineType, Resolution);
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Points == null || Points.Length <= 0)
            {
                return;
            }

            Gizmos.color = waypointColor;
            foreach (Vector3 curPoint in Points)
            {
                Gizmos.DrawSphere(transform.TransformPoint(curPoint), waypointSize);
            }

            if (Points.Length <= 1)
            {
                return;
            }

            SplineCurve splineComponent = new SplineCurve(Points, IsClosed, SplineType);
            Handles.color = splineColor;

            Vector3[] interpolatedPoints = splineComponent.GetPoints(Resolution);
            for (var index = 0; index < interpolatedPoints.Length; index++)
            {
                Vector3 curPoint = interpolatedPoints[index];
                interpolatedPoints[index] = transform.TransformPoint(curPoint);
            }

            Handles.DrawAAPolyLine(splineThickness, interpolatedPoints);
        }
#endif
    }
}
