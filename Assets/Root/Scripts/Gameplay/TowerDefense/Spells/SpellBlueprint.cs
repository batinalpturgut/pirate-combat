using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.Tick.Abstractions;
using Root.Scripts.ScriptableObjects;
using Root.Scripts.ScriptableObjects.Abstractions;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Spells
{
    // SpellSO'lardaki Apply metodunun uygulanmasi degisebilir.
    public class SpellBlueprint : MonoBehaviour, IDeployable, IStandardTickable,
        IInitializer<TickManager, HostileManager, GridManager>
    {
        [SerializeField]
        private Transform model;

        private TickManager _tickManager;
        private HostileManager _hostileManager;
        private bool _isReady;
        private ESpellState _currentState;
        private float _cooldownTimer;
        private float _cooldownDuration;
        private float _spellTimer;
        private float _spellDuration;
        private NodePosition _deployedNode;
        private GridManager _gridManager;
        private NodePosition _appliedNode;
        public int ExecutionOrder => 0;
        public ASpellSO SpellSO { get; private set; }


        private void Awake()
        {
        }

        private void Start()
        {
            _tickManager.Register(this);
            Build();
        }

        private void Build()
        {
            Transform visual = SpellSO.GetVisual(EThemeType.Classic);
            Spawner.Spawn<Transform>(visual, Vector3.zero, Quaternion.identity).SetParent(model, false);
            HandleVisual();
        }


        public void Tick()
        {
            switch (_currentState)
            {
                case ESpellState.Cooldown:
                    _cooldownTimer += Time.deltaTime;
                    if (_cooldownTimer >= _cooldownDuration)
                    {
                        _spellTimer = 0;
                        _currentState = ESpellState.Ready;
                    }

                    break;
                case ESpellState.Active:

                    _spellTimer += Time.deltaTime;
                    
                    SpellSO.Apply(_appliedNode, _hostileManager, _gridManager);

                    if (_spellTimer >= _spellDuration)
                    {
                        _cooldownTimer = 0;
                        _currentState = ESpellState.Cooldown;
                        HandleVisual();
                    }

                    break;
            }
        }

        public void HandleVisual(bool isActive = false)
        {
            foreach (Transform child in model.transform)
            {
                child.gameObject.SetActive(isActive);
            }
        }

        public void TryActivate(NodePosition nodePosition)
        {
            if (_currentState != ESpellState.Ready)
            {
                Log.Console($"{SpellSO} is not ready. Be patient!");
                return;
            }

            _appliedNode = nodePosition;
            transform.position = _gridManager.GetWorldPosition(nodePosition);
            _currentState = ESpellState.Active;
            HandleVisual(true);
        }

        public void FixedTick()
        {
        }

        public void LateTick()
        {
        }

        public void Place()
        {
        }

        public void Remove()
        {
        }


        private void OnDestroy()
        {
            _tickManager.Unregister(this);
        }


        public void Initialize(TickManager tickManager, HostileManager hostileManager, GridManager gridManager)
        {
            _tickManager = tickManager;
            _hostileManager = hostileManager;
            _gridManager = gridManager;
        }

        public void Initialize(ASpellSO spellSO)
        {
            SpellSO = spellSO;
            _spellDuration = SpellSO.SpellDuration;
            _cooldownDuration = SpellSO.CooldownDuration;
        }
    }
}