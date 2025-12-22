using Root.Scripts.Factories;
using Root.Scripts.Gameplay.TowerDefense.Entities.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Captain;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Effects.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.EffectHandlers;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Projectile;
using Root.Scripts.Gameplay.TowerDefense.Projectile.Enums;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.Tick.Abstractions;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.Spawner;
using Root.Scripts.Utilities.TriggerController;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship
{
    public class ShipBlueprint : MonoBehaviour, IInitializer<TickManager, HostileManager>,
        IInitializer<AShipSO, ARaritySO, NodeObject>,
        IStandardTickable, IDeployable, IShooter, IDefenderEffectable
    {
        [SerializeField]
        private Transform modelReference;

        [SerializeField]
        private Transform effectReference;

        public CaptainBlueprint CaptainBlueprint
        {
            get => _captainBlueprint;

            set
            {
                _captainBlueprint = value;
                _projectileController.UpdateVisual(_captainBlueprint.CaptainSO);
                _hasCaptain = value != null;
            }
        }

        public AShipSO ShipSO { get; private set; }

        Transform IBehaviour.Transform => transform;
        HostileBlueprint IShooter.Target { get; set; }
        float IShooter.Range { get; set; }
        float IShooter.ShooterRotateSpeed => 2f; //TODO: SO'ya eklenebilir.
        float IShooter.Angle => 80f; //TODO: Shoot noktalari ve acilari ayri instance'lar olacak.
        Vector3 IShooter.ShootPosition => -transform.right;

        private float _bps;
        private float _targetTime;
        private float _damage;
        private bool _hasCaptain;
        private bool _canShoot = true;
        private bool _inShootingRange;

        private TickManager _tickManager;
        private HostileManager _hostileManager;


        private ProjectileController<ACaptainSO, ShipBlueprint> _projectileController;

        private ARaritySO _raritySO;

        private CaptainBlueprint _captainBlueprint;

        public ShipModel ShipModel { get; private set; }

        public int ExecutionOrder => 0;

        public DistanceTriggerController<ShipBlueprint, HostileBlueprint> DistanceTriggerController
        {
            get;
            private set;
        }

        public IDefenderToDefenderEffect Apply { get; private set; }

        public NodeObject NodeObject { get; private set; }
        public TowerEffectHandler<IDefenderEffectable> TowerEffectHandler { get; private set; }

        public void Initialize(TickManager tickManager, HostileManager hostileManager)
        {
            _tickManager = tickManager;
            _hostileManager = hostileManager;
        }

        public void Initialize(AShipSO shipSO, ARaritySO raritySO, NodeObject nodeObject)
        {
            ShipSO = shipSO;
            _raritySO = raritySO;
            UpdateStats();
            Apply = ShipSO.GetDefenderEffect();
            NodeObject = nodeObject;
            ((IShipDefenderEffect)Apply).Initialize(this);
        }

        public void Build()
        {
            if (modelReference.childCount > 0)
            {
                Destroy(modelReference.GetChild(0).gameObject);
            }

            LoadModel();
            PrepareWaveEffect(ShipModel);
            ActivateRarityEffect(ShipModel);
            SetTriggerController();
            SetProjectileController();
            SetTowerEffectHandler();
        }

        private void LoadModel()
        {
            foreach (var shipVisual in ShipSO.ShipVisualList)
            {
                if (shipVisual.RarityType.GetType == _raritySO.GetType())
                {
                    ShipModel =
                        Spawner.Spawn<ShipModel>(shipVisual.ShipModel.transform, Vector3.zero, Quaternion.identity);
                }
            }

            ShipModel.transform.SetParent(modelReference, false);
        }

        private void ActivateRarityEffect(ShipModel shipModel)
        {
            foreach (ShipVisual shipVisual in ShipSO.ShipVisualList)
            {
                if (shipVisual.RarityType.GetType == _raritySO.GetType())
                {
                    Spawner.Spawn<Transform>(shipVisual.Particle,
                            transform.TransformPoint(shipModel.RarityParticlePosition), Quaternion.identity)
                        .SetParent(effectReference, true);
                }
            }
        }

        private void PrepareWaveEffect(IFloatable floatableModel)
        {
            floatableModel.WaveOffset = Random.Range(ShipSO.WaveOffset.x, ShipSO.WaveOffset.y);
            floatableModel.WaveSpeed = ShipSO.WaveSpeed;
            floatableModel.DirectionMultiplier = Random.Range(0, 2) == 0 ? 1 : -1;
        }

        private void Start()
        {
            _tickManager.Register(this);
        }

        private void SetTowerEffectHandler()
        {
            TowerEffectHandler =
                new TowerEffectHandler<IDefenderEffectable>(this,
                    (tower, ship) =>
                    {
                        tower.ApplyEffectToDefender(ship);
                    },
                    (tower, ship) =>
                    {
                        tower.RemoveEffectFromDefender(ship);
                    });
        }

        private void SetProjectileController()
        {
            _projectileController = ProjectileControllerFactory
                .Create<ACaptainSO, ShipBlueprint>(
                    15f,
                    ShipSO.ProjectileVisualList,
                    transform.TransformPoint(ShipModel.CaptainPosition))
                .SetHomed(true)
                .OnHit((ship, hostile) =>
                {
                    ((HostileBlueprint)hostile).ApplyShip.Damage(ship._damage);
                    if (ship._hasCaptain)
                    {
                        ship._captainBlueprint.ApplyCaptainEffect((HostileBlueprint)hostile);
                    }
                });
        }


        private void SetTriggerController()
        {
            DistanceTriggerController = new DistanceTriggerController<ShipBlueprint, HostileBlueprint>(
                this,
                gameObject,
                _hostileManager.HostileList, ((IShooter)this).Range,
                onClosestTriggerObjectInRange: (self, closestEnemy) =>
                {
                    if (!closestEnemy)
                    {
                        // Eger dusman olduyse
                        self._inShootingRange = false;
                        return;
                    }

                    ((IShooter)self).Target = closestEnemy;
                    self._inShootingRange = ShipBehaviours.InShootingLimits(self);
                    ShipBehaviours.RotateToTarget(self);

                    if (self._canShoot)
                    {
                        self._projectileController.Shoot(self, closestEnemy, EShootingPosition.Right);

                        self._canShoot = false;
                        self._targetTime = Time.time + 1 / self._bps;
                    }
                },
                skipCondition: (_, hostile) => !hostile.IsAlive);
        }

        private void UpdateStats()
        {
            _damage = ShipSO.Damage * (100 + _raritySO.DamagePercentage) / 100;
            ((IShooter)this).Range = ShipSO.Range * (100 + _raritySO.RangePercentage) / 100;
            _bps = ShipSO.Bps * (100 + _raritySO.BpsPercentage) / 100;
            Log.Console(this);
        }

        public override string ToString()
        {
            return ShipSO + "\n" + _raritySO + "\nCurrent Stats: " +
                   "\nDamage: " + _damage +
                   "\nRange: " + ((IShooter)this).Range +
                   "\nBps: " + _bps;
        }


        void IDeployable.Place()
        {
        }

        void IDeployable.Remove()
        {
            Destroy(gameObject);
        }

        void IStandardTickable.Tick()
        {
            DistanceTriggerController.Calculate();

            if (Time.time >= _targetTime && _inShootingRange)
            {
                _canShoot = true;
            }

            ShipBehaviours.PlaySwimEffect(ShipModel);
        }

        void IStandardTickable.FixedTick()
        {
        }

        void IStandardTickable.LateTick()
        {
        }

        private void OnDestroy()
        {
            if (_tickManager != null)
            {
                _tickManager.Unregister(this);
            }
        }
    }
}