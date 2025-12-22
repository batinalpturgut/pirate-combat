using Root.Scripts.Managers.Game;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Inventory;
using Root.Scripts.Managers.Spell;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.UI;
using UnityEngine;

namespace Root.Scripts
{
    public class TowerDefenseContext : AContext
    {
        [SerializeField]
        private TickManager tickManager;

        [SerializeField]
        private GridManager gridManager;

        [SerializeField]
        private UIManager uiManager;

        [SerializeField]
        private GameManager gameManager;

        [SerializeField]
        private InventoryManager inventoryManager;

        [SerializeField]
        private HostileManager hostileManager;

        [SerializeField]
        private SpellManager spellManager;
        protected override bool IsPersistent => false;
        protected override void RegisterServices()
        {
            Register(gameManager);
            Register(tickManager);
            Register(gridManager);
            Register(inventoryManager);
            Register(hostileManager);
            Register(spellManager);
            Register(uiManager);
        }

        protected override void InitServices()
        {
            Init(gameManager, this);
            Init(tickManager, this);
            Init(gridManager, this);
            Init(hostileManager, this);
            Init(spellManager, this);
            Init(uiManager, this);
        }
    }
}