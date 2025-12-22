using System;
using System.Runtime.CompilerServices;

namespace Root.Scripts.Utilities.Guards
{
    public static class GuardNullExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Null<T>(this Guard _, T value, string name = "")
        {
            if (IsUnityObject<T>())
            {
                if (value == null)
                {
                    throw new ArgumentNullException(name, $"Argument '{name}' must not be null.");
                }
            }
            else
            {
                if (value is null)
                {
                    throw new ArgumentNullException(name, $"[UnityObject] Argument '{name}' must not be null.");
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotNull<T>(this Guard _, T value, string name = "")
        {
            if (IsUnityObject<T>())
            {
                if (value != null)
                {
                    throw new ArgumentNullException(name, $"Argument '{name}' must be null.");
                }
            }
            else
            {
                if (value is not null)
                {
                    throw new ArgumentNullException(name, $"[UnityObject] Argument '{name}' must be null.");
                }
            }
        }

        private static bool IsUnityObject<T>()
        {
            return typeof(UnityEngine.Object).IsAssignableFrom(typeof(T));
        }
    }
}