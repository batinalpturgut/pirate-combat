using Root.Scripts.Utilities.Abstractions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Root.Scripts.Utilities.Spawner
{
    public static partial class Spawner
    {
        public static T Spawn<T, T1, T2, T3, T4>(Transform obj, Vector3 worldPos, Quaternion rotation, T1 t1, T2 t2,
            T3 t3, T4 t4)
            where T : IInitializer<T1, T2, T3, T4>
        {
            Transform transform = Object.Instantiate(obj, worldPos, rotation);
            T t = transform.GetComponent<T>();
            t.Initialize(t1, t2, t3, t4);
            return t;
        }


        public static T Spawn<T, T1, T2, T3>(Transform obj, Vector3 worldPos, Quaternion rotation, T1 t1, T2 t2, T3 t3)
            where T : IInitializer<T1, T2, T3>
        {
            Transform transform = Object.Instantiate(obj, worldPos, rotation);
            T t = transform.GetComponent<T>();
            t.Initialize(t1, t2, t3);
            return t;
        }

        public static T Spawn<T, T1, T2>(Transform obj, Vector3 worldPos, Quaternion rotation, T1 t1, T2 t2)
            where T : IInitializer<T1, T2>
        {
            Transform transform = Object.Instantiate(obj, worldPos, rotation);
            T t = transform.GetComponent<T>();
            t.Initialize(t1, t2);
            return t;
        }

        public static T Spawn<T, T1>(Transform obj, Vector3 worldPos, Quaternion rotation, T1 t1
        ) where T : IInitializer<T1>
        {
            Transform transform = Object.Instantiate(obj, worldPos, rotation);
            T t = transform.GetComponent<T>();
            t.Initialize(t1);
            return t;
        }

        public static T Spawn<T>(Transform obj, Vector3 worldPos, Quaternion rotation)
        {
            Transform transform = Object.Instantiate(obj, worldPos, rotation);
            T t = transform.GetComponent<T>();
            return t;
        }
    }
}