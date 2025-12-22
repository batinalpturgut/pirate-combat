using System;
using NaughtyAttributes;
using Root.Scripts.MainScene.IslandStarter;
using Root.Scripts.MainScene.IslandStarter.Abstractions;
using Root.Scripts.ScriptableObjects.Island;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Root.Scripts.Managers.Island
{
    public class IslandManager : MonoBehaviour
    {
        [SerializeField, Required]
        private IslandConfigLookupTable islandConfigLookupTable;

        [SerializeField, Scene]
        private int islandScene;

        [SerializeField, Scene]
        private int mainScene;

        [SerializeField, Scene]
        private int plainScene;

        [SerializeField]
        private IslandStarterBlueprint islandStarterBlueprint;

        [SerializeField]
        private AIslandStarterSO[] islandStarterArr;


        public IslandConfigSO CurrentIslandConfig { get; private set; }

        public void Awake()
        {
            foreach (var islandStarterSO in islandStarterArr)
            {
                Spawner.Spawn<IslandStarterBlueprint, AIslandStarterSO, IslandManager>(islandStarterBlueprint.transform,
                    Vector3.zero,
                    Quaternion.identity, islandStarterSO, this);
            }
        }

        public void LoadIslandConfig(string id)
        {
            CurrentIslandConfig = islandConfigLookupTable.GetIslandConfig(id);
        }

        public void LoadIsland()
        {
            SceneManager.LoadScene(islandScene);
        }

        public void LoadIslandd(AIslandStarterSO islandStarterSo)
        {
            CurrentIslandConfig = islandStarterSo.IslandConfigSo; 
            SceneManager.LoadScene(plainScene);
            
        }

        public void UnloadIslandConfig()
        {
            CurrentIslandConfig = null;
        }

        public void UnloadIsland()
        {
            SceneManager.LoadScene(mainScene);
        }

        public void OnFailure()
        {
        }
    }
}