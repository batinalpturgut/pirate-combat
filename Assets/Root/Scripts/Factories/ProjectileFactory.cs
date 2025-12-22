using System;
using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Entities.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Projectile;
using UnityEngine;

namespace Root.Scripts.Factories
{
    public static class ProjectileFactory
    {
        private const int InitialSize = 10;
        private static Dictionary<Type, object> _pool = new Dictionary<Type, object>();

        public static Projectile<T1> GetProjectile<T1>(Transform transform, T1 source, IShootable target,
            bool isHomed)
        {
            Queue<Projectile<T1>> queue;
            
            if (_pool.TryGetValue(typeof(T1), out var queueObject))
            {
                queue = (Queue<Projectile<T1>>)queueObject;
            }
            else
            {
                Type type = typeof(T1);

                queue = new Queue<Projectile<T1>>(InitialSize);
                for (int i = 0; i < InitialSize; i++)
                {
                    queue.Enqueue(new Projectile<T1>());
                }

                _pool[type] = queue;
            }

            Projectile<T1> instance;

            if (queue.Count > 0)
            {
                instance = queue.Dequeue();
            }
            else
            {
                instance = new Projectile<T1>();
            }

            instance.ProjectileTransform = transform;
            instance.Source = source;
            instance.Target = target;
            instance.IsHomed = isHomed;

            return instance;
        }

        public static void AddToPool<T1>(Projectile<T1> projectile)
        {
            ((Queue<Projectile<T1>>)_pool[typeof(T1)]).Enqueue(projectile);
        }
    }
}