using System;
using System.Collections.Generic;
using Root.Scripts.Factories;
using Root.Scripts.Gameplay.TowerDefense.Entities.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Projectile.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Projectile.Enums;
using Root.Scripts.Gameplay.TowerDefense.Projectile.Structs;
using Root.Scripts.Gameplay.TowerDefense.Projectile.Structures;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.Tick.Abstractions;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;
using Object = UnityEngine.Object;

namespace Root.Scripts.Gameplay.TowerDefense.Projectile
{
    public class ProjectileController<TKey, T1> :
        IInitializer<Vector3[], float, List<ThemeKeyProjectileListPair<TKey>>,
            Func<Transform, Vector3, float, float, bool>, EMotionType>,
        IInitializer<TickManager>,
        IStandardTickable
    {
        private TickManager _tickManager;

        private readonly List<Projectile<T1>> _projectileList = new List<Projectile<T1>>();
        private Vector3[] _shooterPositions;
        private float _speed;
        private IProjectileStructure<TKey> _visualDefinition;
        private Func<Transform, Vector3, float, float, bool> _movementType;
        private EMotionType _motionType;
        private bool _isHomed;

        private GameObject _currentProjectileVisual;
        private ParticleSystem _currentProjectileParticle;

        private Action<T1, IShootable> _onShoot;
        private Action<T1, IShootable> _onFly;
        private Action<T1, IShootable> _onHit;
        private bool _isTicking;

        public int ExecutionOrder => 0;

        void IInitializer<TickManager>.Initialize(TickManager tickManager)
        {
            _tickManager = tickManager;
            _tickManager.Register(this);
        }

        void IInitializer<Vector3[], float, List<ThemeKeyProjectileListPair<TKey>>,
                Func<Transform, Vector3, float, float, bool>, EMotionType>.
            Initialize(
                Vector3[] shooterPositions,
                float speed,
                List<ThemeKeyProjectileListPair<TKey>> visualDefinition,
                Func<Transform, Vector3, float, float, bool> movementType,
                EMotionType motionType)
        {
            _shooterPositions = shooterPositions;
            _speed = speed;
            _visualDefinition = new ThemeKeyStructure<TKey>(visualDefinition);
            _movementType = movementType;
            _motionType = motionType;

            SetDefaultVisual();
        }

        public void Shoot(T1 source, IShootable target, EShootingPosition shootingPosition)
        {
            Transform projectile =
                Spawner.Spawn<Transform>(_currentProjectileVisual.transform, _shooterPositions[(byte)shootingPosition],
                    Quaternion.identity);

            switch (_motionType)
            {
                case EMotionType.Tick:
                    _projectileList.Add(ProjectileFactory.GetProjectile(projectile, source, target, _isHomed));
                    break;
                case EMotionType.Tween:
                    if (_isHomed)
                    {
                        Log.Console("Can't apply homing movement on Tween motion.", LogType.Error);
                    }

                    throw new NotImplementedException();
            }

            _onShoot?.Invoke(source, target);
        }

        //TODO: TickManager'da Tick'lenirken hata veriyor. 
        void IStandardTickable.Tick()
        {
            for (int index = _projectileList.Count - 1; index >= 0; index--)
            {
                Projectile<T1> projectile = _projectileList[index];

                _onFly?.Invoke(projectile.Source, projectile.Target);

                bool isCompleted = _movementType.Invoke(
                    projectile.ProjectileTransform,
                    projectile.TargetPosition,
                    _speed,
                    projectile.Target.ColliderRange);

                if (isCompleted)

                {
                    _onHit?.Invoke(projectile.Source, projectile.Target);
                    Object.Destroy(projectile.ProjectileTransform.gameObject);
                    _projectileList[index].Reset();
                    _projectileList.RemoveAt(index);
                }
            }
        }

        public void UpdateVisual(TKey key)
        {
            HandleVisual(_visualDefinition.GetVisual(key));
        }

        public void SetDefaultVisual()
        {
            HandleVisual(_visualDefinition.GetDefaultVisual());
        }

        private void HandleVisual(GameObject particleVisual)
        {
            _currentProjectileVisual = particleVisual;
            _currentProjectileParticle = particleVisual.GetComponentInChildren<ParticleSystem>();
        }

        public ProjectileController<TKey, T1> OnShoot(Action<T1, IShootable> onShoot)
        {
            _onShoot = onShoot;
            return this;
        }

        public ProjectileController<TKey, T1> OnFly(Action<T1, IShootable> onFly)
        {
            _onFly = onFly;
            return this;
        }

        public ProjectileController<TKey, T1> OnHit(Action<T1, IShootable> onHit)
        {
            _onHit = onHit;
            return this;
        }

        public ProjectileController<TKey, T1> SetHomed(bool isHomed)
        {
            _isHomed = isHomed;
            return this;
        }


        void IStandardTickable.FixedTick()
        {
        }

        void IStandardTickable.LateTick()
        {
        }
    }
}