using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Managers.Tick;
using Root.Scripts.ScriptableObjects;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Captain
{
    public class CaptainBlueprint : MonoBehaviour, IDeployable
    {
        [SerializeField]
        private Transform modelReference;

        private TickManager _tickManager;
        private HostileManager _hostileManager;
        public ACaptainSO CaptainSO { get; private set; }
        public int ExecutionOrder => 0;

        public void Initialize(TickManager tickManager, HostileManager hostileManager)
        {
            _tickManager = tickManager;
            _hostileManager = hostileManager;
        }

        public void Initialize(ACaptainSO captainSO)
        {
            CaptainSO = captainSO;
            Build();
        }

        public void Build()
        {
            GameObject visualGo = CaptainSO.GetVisual(EThemeType.Classic);
            Spawner.Spawn<Transform>(visualGo.transform, Vector3.zero, Quaternion.identity)
                .SetParent(modelReference, false);
        }


        public void ApplyCaptainEffect(HostileBlueprint hostileBlueprint)
        {
            CaptainSO.ApplyEffect(hostileBlueprint);
        }

        public void Tick()
        {
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
    }
}