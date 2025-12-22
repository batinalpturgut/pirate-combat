using System;
using System.Collections.Generic;
using NaughtyAttributes.Test;
using Root.Scripts.Gameplay.TowerDefense.Projectile;
using Root.Scripts.Gameplay.TowerDefense.Projectile.Enums;
using Root.Scripts.Gameplay.TowerDefense.Projectile.Structs;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Guards;
using UnityEngine;

namespace Root.Scripts.Factories
{
    public static class ProjectileControllerFactory
    {
        private static TickManager _tickManager;

        public static void Init(TickManager tickManager)
        {
            _tickManager = tickManager;
        }

        public static ProjectileController<TKey, T1> Create<TKey, T1>(
            float speed,
            List<ThemeKeyProjectileListPair<TKey>> visualDefinition,
            Vector3 shooterRightPosition, Vector3 shooterLeftPosition = default, Vector3 shooterFrontPosition = default,
            Vector3 shooterBackPosition = default, Vector3 shooterUpPosition = default,
            Vector3 shooterDownPosition = default,
            Func<Transform, Vector3, float, float, bool> movementType = null,
            EMotionType motionType = EMotionType.Tick
        )
        {
            Guard.Against.Null(_tickManager, nameof(TickManager));

            movementType ??= MovementType.Linear;

            ProjectileController<TKey, T1> projectileController = new ProjectileController<TKey, T1>();

            ((IInitializer<TickManager>)
                projectileController).Initialize(_tickManager);
            Vector3[] shootingPosArr = new Vector3[6];
            shootingPosArr[0] = shooterRightPosition;
            shootingPosArr[1] = shooterLeftPosition;
            shootingPosArr[2] = shooterFrontPosition;
            shootingPosArr[3] = shooterBackPosition;
            shootingPosArr[4] = shooterUpPosition;
            shootingPosArr[5] = shooterDownPosition;
            ((IInitializer<Vector3[], float, List<ThemeKeyProjectileListPair<TKey>>,
                    Func<Transform, Vector3, float, float, bool>
                    , EMotionType>)
                projectileController).Initialize(shootingPosArr, speed, visualDefinition, movementType, motionType);

            return projectileController;
        }
    }
}