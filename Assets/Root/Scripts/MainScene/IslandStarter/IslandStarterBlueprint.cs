using Root.Scripts.MainScene.IslandStarter.Abstractions;
using Root.Scripts.Managers.Island;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;

namespace Root.Scripts.MainScene.IslandStarter
{
    public class IslandStarterBlueprint : MonoBehaviour, IInitializer<AIslandStarterSO, IslandManager>, IInteractable
    {
        private AIslandStarterSO _islandStarterSO;

        [SerializeField]
        private Transform model;

        private IslandManager _islandManager;

        public void Initialize(AIslandStarterSO islandStarterSO, IslandManager islandManager)
        {
            _islandStarterSO = islandStarterSO;
            _islandManager = islandManager;
        }

        public void Start()
        {
            Transform visual =
                Spawner.Spawn<Transform>(_islandStarterSO.Visual.transform, Vector3.zero, Quaternion.identity);

            visual.SetParent(model, false);
        }

        public void Interact()
        {
            _islandManager.LoadIslandd(_islandStarterSO);
        }
    }
}