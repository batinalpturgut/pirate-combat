using Root.Scripts.Managers.Game;
using UnityEngine;

namespace Root.Scripts.Utilities
{
    public class ParticleStateController : MonoBehaviour
    {
        [SerializeField, HideInInspector] 
        private ParticleSystem[] _particleSystems;

        private void SetParticleState(EGameState gameState)
        {
            if (gameState == EGameState.Paused)
            {
                foreach (ParticleSystem particle in _particleSystems)
                {
                    particle.Pause(true);
                }       
            }
            else
            {
                foreach (ParticleSystem particle in _particleSystems)
                {
                    particle.Play(true);
                }      
            }
        }

        private void OnEnable()
        {
            GameManager.OnGameStateChanged += SetParticleState;

        }

        private void OnDisable()
        {
            GameManager.OnGameStateChanged -= SetParticleState;
        }
        
        private void OnValidate()
        {
            _particleSystems = GetComponentsInChildren<ParticleSystem>(true);
        }
    }
}