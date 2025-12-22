using System;
using System.Runtime.CompilerServices;
using Root.Scripts.Utilities.Logger.Enums;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;


namespace Root.Scripts.Utilities.Logger
{
    public static class Log
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Console(object obj, LogType type = LogType.Debug,
            LogContext context = LogContext.Development, Prefix prefix = Prefix.GamePlay)
        {
            if (!CanLog(context))
            {
                return;
            }
            
            string prefixKey = prefix == Prefix.GamePlay ? string.Empty : $"{prefix}: ";

            switch (type)
            {
                case LogType.Debug:
                    Debug.Log($"<color=white>{prefixKey}{obj}</color>");
                    break;
                case LogType.Warning:
                    Debug.LogWarning($"<color=orange>{prefixKey}{obj}</color>");
                    break;
                case LogType.Error:
                    Debug.LogError($"<color=red>{prefixKey}{obj}</color>");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static bool CanLog(LogContext context)
        {
            switch (context)
            {
                case LogContext.Always:
                    return true;
                case LogContext.Development:
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                    return true;
#else
                    return false;
#endif
                case LogContext.Release:
#if !DEVELOPMENT_BUILD || !UNITY_EDITOR
                    return true;
#else
                    return false;
#endif
                default:
                    return false;
            }
        }
    }
}