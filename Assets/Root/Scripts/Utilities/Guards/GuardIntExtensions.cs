using System;
using System.Runtime.CompilerServices;

namespace Root.Scripts.Utilities.Guards
{
    public static class GuardIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Greater(this Guard _, int value, int max, string name = "")
        {
            if (value > max)
            {
                throw new ArgumentOutOfRangeException(name,
                    $"Argument '{name}' must not be greater than '{max}'. Current value: '{value}'");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Less(this Guard _, int value, int min, string name)
        {
            if (value < min)
            {
                throw new ArgumentOutOfRangeException(name,
                    $"Argument '{name}' must not be less than '{min}'. Current value: '{value}'");
            }
        }
    }
}