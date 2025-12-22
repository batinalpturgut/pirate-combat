using UnityEngine;

namespace Root.Scripts.Extensions
{
    public static class UnityObjectExtensions
    {
        public static bool IsDestroyed(this Object unityObject)
        {
            if (unityObject == null && !ReferenceEquals(unityObject, null))
            {
                return true;
            }

            return false;
        }
    }
}