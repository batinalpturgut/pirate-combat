using System;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Projectile
{
    public static class MovementType
    {
        public static Func<Transform, Vector3, float, float, bool> Linear { get; } = LinearLogic;
        private static bool LinearLogic(Transform projectile, Vector3 target, float speed, float colliderRange)
        {
            Vector3 position = projectile.position;
            Vector3 direction = (target - position).normalized;

            projectile.transform.forward = direction;

            Vector3 moveAmount = direction * (speed * Time.deltaTime);

            position += moveAmount;
            projectile.position = position;

            // TODO: Asagidaki 0.1f degerini SO'dan aliriz. (Collider capi)
            return (target - position).sqrMagnitude < colliderRange * colliderRange;
        }
    }
}