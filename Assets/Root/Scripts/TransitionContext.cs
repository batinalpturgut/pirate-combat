using Root.Scripts.Managers.Island;
using UnityEngine;

namespace Root.Scripts
{
    [DefaultExecutionOrder(-9998)]
    public class TransitionContext : AContext
    {
        [SerializeField] 
        private IslandManager islandManager;
        protected override bool IsPersistent => true;
        protected override void RegisterServices()
        {
            Register(islandManager);
        }

        protected override void InitServices() { }
    }
}