using Root.Scripts.Gameplay.TowerDefense.Entities.Abstractions;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship
{
    public class ShipModel : MonoBehaviour, IFloatable
    {
        [field: SerializeField]
        public Vector3 CaptainPosition { get; private set; }
        
        [field: SerializeField]
        public Vector3 RarityParticlePosition { get; private set; }
        
        [field: SerializeField]
        public MeshRenderer MeshRenderer { get; private set; }

        Transform IBehaviour.Transform => transform;
        float IFloatable.WaveOffset { get; set; }
        float IFloatable.WaveSpeed { get; set; }
        int IFloatable.DirectionMultiplier { get; set; }
        
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                return;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(CaptainPosition, 0.1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(RarityParticlePosition, 0.1f);
        }
    }
}