using Root.Scripts.Extensions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.HostileSOs;
using Root.Scripts.Managers.Tick.Abstractions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile
{
    public class HostileShipModel : AHostileModel, IFloatable, IStandardTickable
    {
        public override Transform Transform => transform;
        public float WaveOffset { get; set; }
        public float WaveSpeed { get; set; }
        public int DirectionMultiplier { get; set; }

        private void OnEnable()
        {
            this.WaitUntil(this, self => self.TickManager != null,
                self =>
                {
                    self.TickManager.Register(self);
                });
        }

        protected override void Init()
        {
            HostileShipSO hostileShipSo = (HostileShipSO)HostileSO;
            WaveOffset = Random.Range(hostileShipSo.WaveOffset.x, hostileShipSo.WaveOffset.y);
            WaveSpeed = hostileShipSo.WaveSpeed;
            DirectionMultiplier = Random.Range(0, 2) == 0 ? 1 : -1;
        }

        void IStandardTickable.Tick()
        {
            ShipBehaviours.PlaySwimEffect(this);
        }

        void IStandardTickable.FixedTick() { }
        void IStandardTickable.LateTick() { }

        private void OnDisable()
        {
            TickManager.Unregister(this);
        }
    }
}