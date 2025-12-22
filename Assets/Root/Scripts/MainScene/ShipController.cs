using System;
using Root.Scripts.MainScene.IslandStarter.Abstractions;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.Tick.Abstractions;
using Root.Scripts.Utilities.Logger;
using UnityEngine;

namespace Root.Scripts.MainScene
{
    public class ShipController : MonoBehaviour, IStandardTickable
    {
        [SerializeField]
        private float rayDistance = 5f;

        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private float moveSpeed = 10f;

        [SerializeField]
        private float turnSpeed = 100f;

        [SerializeField]
        private Floater floater;

        [SerializeField]
        private TickManager tickManager;

        private readonly Vector3 _boxHalfExtends = new Vector3(0.5f, 0.5f, 0.5f);
        public int ExecutionOrder => 0;

        private void Awake()
        {
            floater.Initialize(tickManager);
        }

        private void Start()
        {
            tickManager.Register(this);
        }

        void IStandardTickable.Tick()
        {
            HandleMovement();
            if (Input.GetKeyDown(KeyCode.E))
            {
                HandleRay();
            }
        }

        private void HandleRay()
        {
            if (Physics.BoxCast(transform.position, _boxHalfExtends, transform.forward, out RaycastHit hit,
                    Quaternion.identity,
                    rayDistance))
            {
                InteractWithObject(hit.collider.gameObject);
            }
        }

        private void InteractWithObject(GameObject colliderGameObject)
        {
            IInteractable interactable = colliderGameObject.transform.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }

        private void HandleMovement()
        {
            float moveDirection = Input.GetAxis("Vertical");
            transform.Translate(Vector3.forward * (moveSpeed * moveDirection * Time.deltaTime));

            float turnDirection = Input.GetAxis("Horizontal");
            if (turnDirection > 0)
            {
                floater.SetTurningStatus(true, false);
            }
            else if (turnDirection < 0)
            {
                floater.SetTurningStatus(true, true);
            }
            else
            {
                floater.SetTurningStatus(false, true);
            }

            transform.Rotate(Vector3.up, turnSpeed * turnDirection * Time.deltaTime);
        }

        void IStandardTickable.FixedTick()
        {
        }

        void IStandardTickable.LateTick()
        {
        }
    }
}