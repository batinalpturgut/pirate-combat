using System;
using System.Collections;
using Root.Scripts.Utilities;
using UnityEngine;

namespace Root.Scripts.Extensions
{
    public static class MonoBehaviourExtensions
    {
        public enum UpdateType : byte
        {
            Update = 0,
            FixedUpdate = 1,
            RealtimeUpdate = 2,
        }

        public static void WaitForFrames<T>(this MonoBehaviour mb, T target, Action<T> action, int frameCount = 1)
        {
            mb.StartCoroutine(InvokeRoutine(target, action, frameCount));
        }

        public static void Invoke<T>(this MonoBehaviour mb, T target, Action<T> action, float delay,
            UpdateType updateType = UpdateType.Update)
        {
            mb.StartCoroutine(InvokeRoutine(target, action, delay, updateType));
        }

        public static void WaitUntil<T>(this MonoBehaviour mb, T target, Func<T, bool> condition, Action<T> action)
        {
            mb.StartCoroutine(WaitUntilRoutine(target, condition, action));
        }

        public static void WaitWhile<T>(this MonoBehaviour mb, T target, Func<T, bool> condition, Action<T> action)
        {
            mb.StartCoroutine(WaitWhileRoutine(target, condition, action));
        }

        private static IEnumerator InvokeRoutine<T>(T target, Action<T> action, int frameCount)
        {
            for (int i = 0; i < frameCount; i++)
            {
                yield return null;
            }

            action?.Invoke(target);
        }

        private static IEnumerator InvokeRoutine<T>(T target, Action<T> action, float delay, UpdateType updateType)
        {
            switch (updateType)
            {
                case UpdateType.Update:
                    yield return Wait.ForSeconds(delay);
                    break;
                case UpdateType.FixedUpdate:
                    yield return Wait.ForSecondsRealtime(delay);
                    break;
                case UpdateType.RealtimeUpdate:
                    yield return Wait.ForSecondsRealtime(delay);
                    break;
            }

            action?.Invoke(target);
        }

        private static IEnumerator WaitUntilRoutine<T>(T target, Func<T, bool> condition, Action<T> action)
        {
            yield return Wait.UntilNonAlloc(target, condition);
            action?.Invoke(target);
        }

        private static IEnumerator WaitWhileRoutine<T>(T target, Func<T, bool> condition, Action<T> action)
        {
            yield return Wait.WhileNonAlloc(target, condition);
            action?.Invoke(target);
        }
    }
}