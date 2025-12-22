using UnityEngine;

namespace Root.Scripts.Utilities.GameObjectPool.Extensions
{
    public static class PoolExtensions
    {
        public static GameObject Get(this GameObject gameObject, Vector3 position = default,
            Quaternion rotation = default,
            Transform parent = null)
        {
            return ObjectPoolController.Get(gameObject, position, rotation, parent);
        }

        public static void Release(this GameObject gameObject, bool resetRotationAndScale = true, float delay = 0f)
        {
            ObjectPoolController.Release(gameObject, resetRotationAndScale, delay);
        }

        public static ParticleSystem ReleaseOnComplete(this ParticleSystem particleSystem,
            bool resetRotationAndScale = true)
        {
            ObjectPoolController.Release(particleSystem.gameObject, resetRotationAndScale,
                particleSystem.main.duration);
            return particleSystem;
        }

        public static void Clear(this GameObject gameObject)
        {
            ObjectPoolController.ClearPool(gameObject);
        }
    }
}