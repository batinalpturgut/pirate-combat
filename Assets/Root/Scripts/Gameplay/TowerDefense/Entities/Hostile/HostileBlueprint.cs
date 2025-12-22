using System;
using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Entities.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.EffectHandlers;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.UI.Gameplay.HealthBar;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.Tick.Abstractions;
using Root.Scripts.ScriptableObjects;
using Root.Scripts.ScriptableObjects.Island;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile
{
    public class HostileBlueprint : MonoBehaviour, IStandardTickable, ISoulSource, IShootable,
        IInitializer<TickManager, GridManager, HostileManager>,
        IInitializer<AHostileSO, Road>
    {
        [SerializeField]
        private Transform modelReference;

        [SerializeField] 
        private HealthBarController healthBarController;

        private TickManager _tickManager;
        private HostileManager _hostileManager;

        private AHostileSO _hostileSO;
        private AHostileModel _hostileModel;

        public List<NodePosition> Path { get; private set; } = new List<NodePosition>(); 

        private GridManager _gridManager;
        public Road Road { get; private set; }
        public bool IsAlive { get; set; }

        private float _health;
        public float Health
        {
            get => _health;
            set
            {
                _health = value;
                OnHealthChanged?.Invoke(_health);
            }
        }
        
        public int SoulDrop { get; private set; }
        public int ExecutionOrder => 0;
        public float MoveSpeed { get; set; }
        public float RotateSpeed { get; set; }

        public IMovement Movement { get; private set; }

        public ITowerEffect ApplyTower;
        public IShipEffect ApplyShip;
        public ICaptainEffect ApplyCaptain;
        public ISpellEffect ApplySpell;
        public event Action<float> OnHealthChanged;
        public TowerEffectHandler<HostileBlueprint> TowerEffectHandler { get; private set; }

        public MonoBehaviour MonoBehaviour => this;
        public float ColliderRange => _hostileSO.ColliderRange;

        public void Initialize(TickManager tickManager, GridManager gridManager, HostileManager hostileManager)
        {
            _tickManager = tickManager;
            _gridManager = gridManager;
            _hostileManager = hostileManager;
        }
        
        public void Initialize(AHostileSO hostileSO, Road road)
        {
            _hostileSO = hostileSO;
            Road = road;
            Path = road.Path;
            SoulDrop = hostileSO.SoulDrop;
            Build();
        }

        private void Start()
        {
            _tickManager.Register(this);
            TowerEffectHandler = new TowerEffectHandler<HostileBlueprint>(this, 
                (tower, hostile) => tower.ApplyEffect(hostile), 
                (tower, hostile) => tower.RemoveEffect(hostile));
        }

        private void Build()
        {
            IsAlive = true;
            Health = _hostileSO.Health;
            MoveSpeed = _hostileSO.MoveSpeed;
            RotateSpeed = _hostileSO.RotateSpeed;
            Movement = _hostileSO.GetMovement();
            Movement.Initialize(this, Path, _gridManager, _hostileManager);
            // Apply = _hostileSO.GetEffect();
            // Apply.Initialize(this);
            BuildEffects();
            BuildVisual();
            BuildHealthBar();
        }

        private void BuildEffects()
        {
            ApplyTower = _hostileSO.GetTowerEffect();
            ApplyShip = _hostileSO.GetShipEffect();
            ApplySpell = _hostileSO.GetSpellEffect();
            ApplyCaptain = _hostileSO.GetCaptainEffect();
            
            ApplyTower.Initialize(this);
            ApplySpell.Initialize(this);
            ApplyShip.Initialize(this);
            ApplyCaptain.Initialize(this);
        }

        private void BuildVisual()
        {
            _hostileModel = Spawner.Spawn<AHostileModel, TickManager, AHostileSO>(
                _hostileSO.GetVisual(EThemeType.Classic).transform, Vector3.zero, Quaternion.identity,
                _tickManager, _hostileSO);
            _hostileModel.Transform.SetParent(modelReference, false);
        }

        private void BuildHealthBar()
        {
            Spawner.Spawn<HealthBarController, HostileBlueprint, TickManager>(
                    healthBarController.transform, Vector3.zero, Quaternion.identity, this, _tickManager)
                .transform.SetParent(transform, false);
        }

        public void Tick()
        {
            Movement.Move(); // Tick ile degilde tween ile calisan enemy'ler olabilir mi? Tick yerine coroutine
            // kullanilabilir.
        }

        void IStandardTickable.FixedTick() { }
        void IStandardTickable.LateTick() { }

        public void Destroy()
        {
            if (_tickManager != null)
            {
                _tickManager.Unregister(this);
            }

            Destroy(gameObject);
        }
    }
}