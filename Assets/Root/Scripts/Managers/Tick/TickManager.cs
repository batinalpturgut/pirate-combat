using System.Collections.Generic;
using Root.Scripts.Managers.Game;
using Root.Scripts.Managers.Tick.Abstractions;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Logger;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;

namespace Root.Scripts.Managers.Tick
{
    public class TickManager : MonoBehaviour, IInitializer<AContext>
    {
        // TODO: SortedSet kullanilabilir.
        private readonly List<IStandardTickable> _standardTicks = new List<IStandardTickable>();
        private readonly List<IExtendedTickable> _extendedTicks = new List<IExtendedTickable>();
        private readonly List<IRealtimeTickable> _realTimeTicks = new List<IRealtimeTickable>();

        private GameManager _gameManager;
        private bool _gameQuitting;

        public void Initialize(AContext context)
        {
            _gameManager = context.Resolve<GameManager>();
        }

        public void Register(ITickable instance)
        {
#if UNITY_EDITOR
            if (instance is IStandardTickable standardTickable && 
                _standardTicks.Contains(standardTickable))
            {
                return;
            }
#endif

            if (instance is IExtendedTickable extendedInstance)
            {
                _extendedTicks.Add(extendedInstance);
            }

            if (instance is IStandardTickable standardInstance)
            {
                _standardTicks.Add(standardInstance);
            }

            if (instance is IRealtimeTickable realtimeTickable)
            {
                _realTimeTicks.Add(realtimeTickable);
            }

            if (instance is { ExecutionOrder: default(int) })
            {
                return;
            }

            _realTimeTicks.Sort(ComparerClass.DescendingTickComparer);
            _extendedTicks.Sort(ComparerClass.DescendingTickComparer);
            _standardTicks.Sort(ComparerClass.DescendingTickComparer);
        }

        public void Unregister(ITickable instance)
        {
            if (_gameQuitting)
            {
                return;
            }
            
#if UNITY_EDITOR
            if (instance is IStandardTickable standardTickable && 
                !_standardTicks.Contains(standardTickable))
            {
                Log.Console(" Couldn't find an object to remove from the list.", LogType.Error);
                return;
            }
#endif
            if (instance is IExtendedTickable extendedInstance)
            {
                _extendedTicks.Remove(extendedInstance);
            }

            if (instance is IStandardTickable standardInstance)
            {
                _standardTicks.Remove(standardInstance);
            }

            if (instance is IRealtimeTickable realtimeTickable)
            {
                _realTimeTicks.Remove(realtimeTickable);
            }
        }

        private void FixedUpdate()
        {
            if (_gameManager.GameState == EGameState.Paused)
            {
                return;
            }

            for (int i = _standardTicks.Count - 1; i >= 0; i--)
            {
                _standardTicks[i].FixedTick();
            }
        }

        private void Update()
        {
            for (int i = _realTimeTicks.Count - 1; i >= 0; i--)
            {
                _realTimeTicks[i].RealtimeTick();
            }

            if (_gameManager.GameState == EGameState.Paused)
            {
                return;
            }

            for (int i = _extendedTicks.Count - 1; i >= 0; i--)
            {
                _extendedTicks[i].BeforeTick();
            }


            for (int i = _standardTicks.Count - 1; i >= 0; i--)
            {
                _standardTicks[i].Tick();
            }

            for (int i = _extendedTicks.Count - 1; i >= 0; i--)
            {
                _extendedTicks[i].AfterTick();
            }
        }

        private void LateUpdate()
        {
            if (_gameManager.GameState == EGameState.Paused)
            {
                return;
            }

            for (int i = _standardTicks.Count - 1; i >= 0; i--)
            {
                _standardTicks[i].LateTick();
            }
        }

        private void OnApplicationQuit()
        {
            _gameQuitting = true;
        }
    }
}