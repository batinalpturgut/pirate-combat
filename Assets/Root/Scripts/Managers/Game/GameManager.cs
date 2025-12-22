using System;
using System.Diagnostics;
using PrimeTween;
using Root.Scripts.Factories;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.Tick.Abstractions;
using Root.Scripts.Services.Ads;
using Root.Scripts.Services.Core;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Abstractions;
using UnityEngine;

namespace Root.Scripts.Managers.Game
{
    public class GameManager : MonoBehaviour, IInitializer<AContext>, IRealtimeTickable
    {
        private TickManager _tickManager;
        private EGameState _gameState = EGameState.Playing;
        public Camera TowerDefenceCamera { get; private set; }
        public int ExecutionOrder { get; }
        public static event Action<EGameState> OnGameStateChanged;

        public EGameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
                OnGameStateChanged?.Invoke(_gameState);
            }
        }

        public void Initialize(AContext context)
        {
            _tickManager = context.Resolve<TickManager>();
            ProjectileControllerFactory.Init(_tickManager);
            TowerDefenceCamera = Camera.main;
        }

        void IRealtimeTickable.RealtimeTick()
        {
            StopGameCheat();

            if (Input.GetKeyUp(KeyCode.J))
            {
                GameServices.Get<AdService>().ShowBanner();
            }
            
            if (Input.GetKeyUp(KeyCode.K))
            {
                GameServices.Get<AdService>().HideBanner();
            }
            
            if (Input.GetKeyUp(KeyCode.L))
            {
                GameServices.Get<AdService>().ShowInterstitial();
            }
        }

        [Conditional(Consts.Preprocessors.ENABLE_CHEATS)]
        private void StopGameCheat()
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                GameState =
                    GameState == EGameState.Playing ? EGameState.Paused : EGameState.Playing;
            }
        }

        public void SetPhysicsState(bool isEnable)
        {
            Physics.autoSimulation = isEnable;
        }

        private void Start()
        {
            //TODO: Performance Manager'a tasinabilir.
            SetPhysicsState(true);
            PrimeTweenConfig.SetTweensCapacity(400);
            
            _tickManager.Register(this);
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