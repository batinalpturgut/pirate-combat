using System;
using PrimeTween;
using Root.Scripts.Extensions;
using Root.Scripts.Utilities.Curves;
using UnityEngine;

namespace Root.Scripts.Helpers.Runtime
{
    public static class PrimeTweenHelpers
    {
        // TODO: Delegate allocation'a bir cozum bulmak lazim.
        public static Tween Path(
            Transform target, 
            float startValue, 
            float endValue, 
            float duration, 
            Ease ease,
            SplineCurve splineCurve, 
            int cycles = 1, 
            CycleMode cycleMode = CycleMode.Restart, 
            float startDelay = 0f, 
            float endDelay = 0f, 
            bool useUnscaledTime = false, 
            bool lookAhead = true, 
            AxisConstraint axisConstraint = AxisConstraint.None,
            Action<Transform, int> onWaypointChange = null)
        {
            int previousWaypointIndex = -1;
            
            return
                Tween.Custom(
                    target: target,
                    startValue: startValue,
                    endValue: endValue,
                    duration: duration,
                    ease: ease,
                    cycles: cycles,
                    cycleMode: cycleMode,
                    startDelay: startDelay,
                    endDelay: endDelay,
                    useUnscaledTime: useUnscaledTime,
                    onValueChange: (transform, t) =>
                    {
                        Vector3 position = splineCurve.GetPoint(t);

                        if (!lookAhead)
                        {
                            transform.position = position;
                        }
                        else
                        {
                            Quaternion rotation = splineCurve.GetRotation(t);

                            if ((axisConstraint & AxisConstraint.X) == AxisConstraint.X)
                            {
                                rotation.x = transform.rotation.x;
                            }

                            if ((axisConstraint & AxisConstraint.Y) == AxisConstraint.Y)
                            {
                                rotation.y = transform.rotation.y;
                            }

                            if ((axisConstraint & AxisConstraint.Z) == AxisConstraint.Z)
                            {
                                rotation.z = transform.rotation.z;
                            }

                            transform.SetPositionAndRotation(position, rotation);
                        }

                        if (onWaypointChange == null)
                        {
                            return;
                        }

                        int currentWaypointIndex = Mathf.FloorToInt(
                            t.Remap(0, 1, 0, splineCurve.Waypoints.Length));

                        if (currentWaypointIndex >= splineCurve.Waypoints.Length)
                        {
                            currentWaypointIndex = 0;
                        }

                        if (currentWaypointIndex != previousWaypointIndex)
                        {
                            if (currentWaypointIndex - previousWaypointIndex > 1)
                            {
                                for (int waypointIndex = previousWaypointIndex + 1;
                                     waypointIndex < currentWaypointIndex;
                                     waypointIndex++)
                                {
                                    onWaypointChange.Invoke(transform, waypointIndex);
                                }
                            }

                            previousWaypointIndex = currentWaypointIndex;
                            onWaypointChange.Invoke(transform, currentWaypointIndex);
                        }
                    });
        }
    }

    [Flags]
    public enum AxisConstraint : byte
    {
        None = 0b_0000_0000,
        X    = 0b_0000_0001,
        Y    = 0b_0000_0010,
        Z    = 0b_0000_0100
    }
}