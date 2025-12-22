using Root.Scripts.Factories;
using Root.Scripts.Gameplay.TowerDefense.Entities.Abstractions;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Logger;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;

namespace Root.Scripts.Gameplay.TowerDefense.Projectile
{
    public class Projectile<T1> : IInitializer<Transform, T1, IShootable, bool>
    {
        private Vector3 _targetPositionCache;
        public Transform ProjectileTransform { get; set; }
        public T1 Source { get; set; }
        public IShootable Target { get; set; }
        public bool IsHomed { get; set; }

        public Vector3 TargetPosition
        {
            get
            {
                if (!IsHomed)
                {
                    if (_targetPositionCache == default)
                    {
                        _targetPositionCache = Target.MonoBehaviour.transform.position;
                    }

                    return _targetPositionCache;
                }

                if (Target.MonoBehaviour)
                {
                    _targetPositionCache = Target.MonoBehaviour.transform.position;
                }

                return _targetPositionCache;
            }
        }

        public void Reset()
        {
            ProjectileTransform = null;
            Source = default;
            Target = null;
            IsHomed = false;
            ProjectileFactory.AddToPool(this);
        }

        public void Initialize(Transform projectileTransform, T1 source, IShootable target, bool isHomed)
        {
            ProjectileTransform = projectileTransform;
            Source = source;
            Target = target;
            IsHomed = isHomed;
        }
    }
}