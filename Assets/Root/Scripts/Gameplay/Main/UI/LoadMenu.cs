using Root.Scripts.Managers.Island;
using Root.Scripts.Managers.UI.Abstractions;
using UnityEngine;
using UnityEngine.UI;

namespace Root.Scripts.Gameplay.Main.UI
{
    public class LoadMenu : AMenu
    {
        [SerializeField] 
        private Button loadButton;
        
        private IslandManager _islandManager;
        protected override void OnInit()
        {
            _islandManager = Context.ResolveShared<IslandManager>();
            loadButton.onClick.AddListener(OnSceneLoadClick);
        }

        private void OnSceneLoadClick()
        {
            _islandManager.LoadIslandConfig("C_1");
            _islandManager.LoadIsland();
        }

        protected override void OnShow()
        {
            
        }

        protected override void OnHide()
        {
            
        }

        protected override void OnEscape()
        {
            
        }

        protected override void OnUIManagerDestroy()
        {
            loadButton.onClick.RemoveListener(OnSceneLoadClick);
        }
    }
}