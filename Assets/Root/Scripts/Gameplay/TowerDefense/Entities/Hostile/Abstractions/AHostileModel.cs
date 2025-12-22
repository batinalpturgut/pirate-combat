using Root.Scripts.Managers.Tick;
using Root.Scripts.Utilities.Abstractions;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions
{
    public abstract class AHostileModel : MonoBehaviour, IInitializer<TickManager,AHostileSO>
    {
        public abstract Transform Transform { get;}
        protected TickManager TickManager { get; private set; }
        protected AHostileSO HostileSO { get; private set; }
        public int ExecutionOrder => 0;
        public void Initialize(TickManager tickManager, AHostileSO hostileSO)
        {
            TickManager = tickManager;
            HostileSO = hostileSO;
            Init();
        }
        protected abstract void Init();
    }
}