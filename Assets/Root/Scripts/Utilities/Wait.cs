using System;
using System.Collections.Generic;
using UnityEngine;

namespace Root.Scripts.Utilities
{
    // TODO: Ortak logic'ler 
    public static class Wait
    {
        private const int InitialSize = 5;

        private static readonly Stack<WaitForSeconds> WaitForSecondsPool;
        private static readonly Stack<WaitForSecondsRealtime> WaitForSecondsRealtimePool;
        private static readonly Dictionary<Type, object> WaitUntilNonAllocPools;
        private static readonly Dictionary<Type, object> WaitWhileNonAllocPools;

        static Wait()
        {
            WaitForSecondsPool = new Stack<WaitForSeconds>(InitialSize);
            WaitForSecondsRealtimePool = new Stack<WaitForSecondsRealtime>(InitialSize);

            for (int i = 0; i < InitialSize; i++)
            {
                WaitForSecondsPool.Push(new WaitForSeconds());
                WaitForSecondsRealtimePool.Push(new WaitForSecondsRealtime());
            }

            WaitUntilNonAllocPools = new Dictionary<Type, object>();
            WaitWhileNonAllocPools = new Dictionary<Type, object>();
        }

        public static CustomYieldInstruction ForSeconds(float seconds)
        {
            WaitForSeconds instance;
            if (WaitForSecondsPool.Count > 0)
            {
                instance = WaitForSecondsPool.Pop();
            }
            else
            {
                instance = new WaitForSeconds();
            }

            instance.Initialize(seconds);
            return instance;
        }

        public static CustomYieldInstruction ForSecondsRealtime(float seconds)
        {
            WaitForSecondsRealtime instance;
            if (WaitForSecondsRealtimePool.Count > 0)
            {
                instance = WaitForSecondsRealtimePool.Pop();
            }
            else
            {
                instance = new WaitForSecondsRealtime();
            }

            instance.Initialize(seconds);
            return instance;
        }

        public static CustomYieldInstruction UntilNonAlloc<T>(T target, Func<T, bool> predicate)
        {
            Stack<WaitUntilNonAlloc<T>> pool;
            Type type = typeof(T);

            if (WaitUntilNonAllocPools.TryGetValue(type, out var poolObject))
            {
                pool = (Stack<WaitUntilNonAlloc<T>>)poolObject;
            }
            else
            {
                pool = new Stack<WaitUntilNonAlloc<T>>(InitialSize);
                for (int i = 0; i < InitialSize; i++)
                {
                    pool.Push(new WaitUntilNonAlloc<T>());
                }

                WaitUntilNonAllocPools[type] = pool;
            }

            WaitUntilNonAlloc<T> instance;
            if (pool.Count > 0)
            {
                instance = pool.Pop();
            }
            else
            {
                instance = new WaitUntilNonAlloc<T>();
            }

            instance.Initialize(target, predicate);
            return instance;
        }

        public static CustomYieldInstruction WhileNonAlloc<T>(T target, Func<T, bool> predicate)
        {
            Stack<WaitWhileNonAlloc<T>> pool;
            Type type = typeof(T);

            if (WaitWhileNonAllocPools.TryGetValue(type, out var poolObject))
            {
                pool = (Stack<WaitWhileNonAlloc<T>>)poolObject;
            }
            else
            {
                pool = new Stack<WaitWhileNonAlloc<T>>(InitialSize);
                for (int i = 0; i < InitialSize; i++)
                {
                    pool.Push(new WaitWhileNonAlloc<T>());
                }

                WaitWhileNonAllocPools[type] = pool;
            }

            WaitWhileNonAlloc<T> instance;
            if (pool.Count > 0)
            {
                instance = pool.Pop();
            }
            else
            {
                instance = new WaitWhileNonAlloc<T>();
            }

            instance.Initialize(target, predicate);
            return instance;
        }

        private sealed class WaitForSeconds : CustomYieldInstruction
        {
            private float _waitUntil;

            public override bool keepWaiting
            {
                get
                {
                    if (Time.time < _waitUntil)
                    {
                        return true;
                    }

                    WaitForSecondsPool.Push(this);
                    return false;
                }
            }

            public void Initialize(float seconds)
            {
                _waitUntil = Time.time + seconds;
            }
        }

        private sealed class WaitForSecondsRealtime : CustomYieldInstruction
        {
            private float _waitUntil;

            public override bool keepWaiting
            {
                get
                {
                    if (Time.realtimeSinceStartup < _waitUntil)
                    {
                        return true;
                    }

                    WaitForSecondsRealtimePool.Push(this);
                    return false;
                }
            }

            public void Initialize(float seconds)
            {
                _waitUntil = Time.realtimeSinceStartup + seconds;
            }
        }

        private sealed class WaitUntilNonAlloc<T> : CustomYieldInstruction
        {
            private Func<T, bool> _predicate;
            private T _target;

            public override bool keepWaiting
            {
                get
                {
                    if (!_predicate(_target))
                    {
                        return true;
                    }

                    Reset();
                    Stack<WaitUntilNonAlloc<T>> pool = (Stack<WaitUntilNonAlloc<T>>)WaitUntilNonAllocPools[typeof(T)];
                    pool.Push(this);
                    return false;
                }
            }

            public void Initialize(T target, Func<T, bool> predicate)
            {
                _predicate = predicate;
                _target = target;
            }

            public override void Reset()
            {
                _predicate = null;
                _target = default;
            }
        }

        private sealed class WaitWhileNonAlloc<T> : CustomYieldInstruction
        {
            private Func<T, bool> _predicate;
            private T _target;

            public override bool keepWaiting
            {
                get
                {
                    if (_predicate(_target))
                    {
                        return true;
                    }

                    Reset();
                    Stack<WaitWhileNonAlloc<T>> pool = (Stack<WaitWhileNonAlloc<T>>)WaitWhileNonAllocPools[typeof(T)];
                    pool.Push(this);
                    return false;
                }
            }

            public void Initialize(T target, Func<T, bool> predicate)
            {
                _predicate = predicate;
                _target = target;
            }

            public override void Reset()
            {
                _predicate = null;
                _target = default;
            }
        }
    }
}