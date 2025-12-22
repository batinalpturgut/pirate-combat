using System;
using System.Collections.Generic;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Utilities.Logger;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Movements
{
    public class GridBaseMovement : IMovement
    {
        private HostileBlueprint _hostileBlueprint;
        private Transform _hostileTransform;
        private int _currentNodeIndex;
        private GridManager _gridManager;
        private Vector3 _currentPosition;
        private Vector3 _targetPosition;
        private Vector3 _lookDir;
        private float _moveFactor;
        private bool _crossMovement;
        private HostileManager _hostileManager;
        private List<NodePosition> _path;
        public float CurrentSpeed { get; private set; }


        public void Initialize(HostileBlueprint hostileBlueprint, List<NodePosition> path, GridManager gridManager,
            HostileManager hostileManager)
        {
            _hostileBlueprint = hostileBlueprint;
            _hostileTransform = _hostileBlueprint.transform;
            _path = path;
            _gridManager = gridManager;
            _hostileManager = hostileManager;
            Reset();
        }


        void IMovement.Move()
        {
            HandleMovement();
            HandleRotation();
        }

        public void ApplyPortal()
        {
            _hostileManager.HostilePortalEffect(_hostileBlueprint, _path[_currentNodeIndex]);
            // OnResetPath?.Invoke(_hostile, _path[_currentNodeIndex]);
        }

        private void HandleMovement()
        {
            // Iki node arasi mesafeyi 1 / _hostile.MoveSpeed saniye sonra alacaktir. Ama carpaz node'lar icinde bu gecerlidir.
            // Dolayisiyla asagidaki formul 1 / _hostile.MoveSpeed saniyede her zaman cellSize kadar yol almasini saglar.
            AdjustSpeed();
            _moveFactor += Time.deltaTime * CurrentSpeed;
            _hostileTransform.position = Vector3.Lerp(_currentPosition, _targetPosition, _moveFactor);

            if (_hostileTransform.position == _targetPosition)
            {
                CheckForNewNode();
            }
        }

        private void CheckForNewNode()
        {
            if (_currentNodeIndex < _path.Count - 2)
            {
                _moveFactor = 0f;
                _currentNodeIndex++;
                _currentPosition = _gridManager.GetWorldPosition(_path[_currentNodeIndex]);
                _targetPosition = _gridManager.GetWorldPosition(_path[_currentNodeIndex + 1]);
                _crossMovement = CheckForCrossMovement(_path[_currentNodeIndex], _path[_currentNodeIndex + 1]);
                _lookDir = (_targetPosition - _hostileTransform.position).normalized;
                _hostileManager.HostileNodeChanged(_hostileBlueprint, _path[_currentNodeIndex],
                    _path[_currentNodeIndex - 1]);
            }
            else
            {
                PathFinished();
            }
        }

        private bool CheckForCrossMovement(NodePosition a, NodePosition b)
        {
            return Math.Abs(a.x - b.x) == 1 && Math.Abs(a.z - b.z) == 1;
        }

        private void PathFinished()
        {
            Log.Console("Path finished.");
            _hostileManager.HostilePathFinished(_hostileBlueprint, _path[_currentNodeIndex + 1],
                _path[_currentNodeIndex]);
            // OnPathFinished?.Invoke(_hostile, _path[_currentNodeIndex + 1], _path[_currentNodeIndex]);
        }

        private void HandleRotation()
        {
            //TODO: Formulu incele.
            _hostileTransform.forward =
                Vector3.Lerp(_hostileTransform.forward, _lookDir, Time.deltaTime * _hostileBlueprint.RotateSpeed);
        }

        public void Reset()
        {
            _currentNodeIndex = 0;
            Vector3 startingPos = _gridManager.GetWorldPosition(_path[0]);
            _hostileTransform.position = startingPos;
            _currentPosition = startingPos;
            _targetPosition = _gridManager.GetWorldPosition(_path[1]);
            _lookDir = (_targetPosition - _hostileTransform.position).normalized;
            _moveFactor = 0f;
        }

        public void OnDeath()
        {
            _hostileManager.HostileDeath(_hostileBlueprint, _path[_currentNodeIndex], EMovementType.Grid);
            // OnDeath?.Invoke(_hostile, _path[_currentNodeIndex]);
        }

        private void AdjustSpeed()
        {
            if (_currentNodeIndex < _path.Count - 1 && _currentNodeIndex >= 0 &&
                _gridManager.TryGetNodeObjectEntity(_path[_currentNodeIndex + 1], out HostileBlueprint hostile) &&
                CurrentSpeed >= hostile.Movement.CurrentSpeed)
            {
                CurrentSpeed = hostile.Movement.CurrentSpeed;
            }
            else if (_crossMovement)
            {
                CurrentSpeed = _hostileBlueprint.MoveSpeed / Mathf.Sqrt(2);
            }
            else
            {
                CurrentSpeed = _hostileBlueprint.MoveSpeed;
            }
        }
    }
}