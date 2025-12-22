using UnityEditor;
using UnityEngine;

namespace Root.Scripts.Utilities.Curves.Editor
{
    [CustomEditor(typeof(SplineComponent))]
    public class SplineComponentEditor : UnityEditor.Editor
    {
        public void OnSceneGUI()
        {
            SplineComponent targetClass = (SplineComponent)target;

            if (targetClass == null || targetClass.Points == null || targetClass.Points.Length <= 0)
            {
                return;
            }

            Transform parent = targetClass.transform;

            for (int index = 0; index < targetClass.Points.Length; index++)
            {
                Vector3 point = targetClass.Points[index];
                Vector3 position = parent.position;
                targetClass.Points[index] = Handles.PositionHandle(point + position, Quaternion.identity) - position;
            }
        }
    }
}