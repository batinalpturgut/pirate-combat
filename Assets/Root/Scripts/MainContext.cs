using Root.Scripts.Managers.Game;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.UI;
using UnityEngine;

namespace Root.Scripts
{
    public class MainContext : AContext
    {
        [SerializeField] 
        private GameManager gameManager;
        
        [SerializeField] 
        private TickManager tickManager;

        [SerializeField] 
        private UIManager uiManager;
        protected override bool IsPersistent => false;

        protected override void RegisterServices()
        {
            Register(gameManager);
            Register(tickManager);
            Register(uiManager);
        }

        protected override void InitServices()
        {
            Init(gameManager, this);
            Init(tickManager, this);
            Init(uiManager, this);
        }
    }
}