using System.Collections.Generic;
using Root.Scripts.Extensions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Managers.Hostile;
using Root.Scripts.Utilities.Logger;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Movements
{
    public class SplineBasedMovement : IMovement
    {
        public float CurrentSpeed { get; }
        private HostileManager _hostileManager;
        private GridManager _gridManager;
        private HostileBlueprint _hostileBlueprint;
        private List<NodePosition> _path;
        private float _moveFactor;
        private float _rotateFactor;
        private Transform _hostileTransform;
        private Vector3 _currentPosition;
        private Vector3 _targetPosition;
        private int _currentNodeIndex;

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

        public void Move()
        {
            _moveFactor += Time.deltaTime * _hostileBlueprint.MoveSpeed;
            _rotateFactor += Time.deltaTime * _hostileBlueprint.RotateSpeed;

            _hostileTransform.position = Vector3.Lerp(_currentPosition, _targetPosition, _moveFactor);

            float t = ((float)_currentNodeIndex).Remap(0f, _path.Count, 0, 1f);

            _hostileTransform.rotation = Quaternion.Lerp(_hostileTransform.rotation,
                _hostileBlueprint.Road.SplineComponent.SplineCurve.GetRotation(t), _rotateFactor);

            if (_hostileTransform.position == _targetPosition)
            {
                CheckForNewNode();
            }
        }

        private void CheckForNewNode()
        {
            if (_currentNodeIndex < _path.Count - 2)
            {
                _currentNodeIndex++;
                NodePosition currentNode = _path[_currentNodeIndex];
                NodePosition targetNode = _path[_currentNodeIndex + 1];
                _currentPosition = new Vector3(currentNode.x, currentNode.y, currentNode.z);
                _targetPosition = new Vector3(targetNode.x, targetNode.y, targetNode.z);
                _moveFactor = 0f;
            }
            else
            {
                _hostileManager.RemoveHostile(_hostileBlueprint);
            }
        }

        public void ApplyPortal()
        {
        }

        public void OnDeath()
        {
            _hostileManager.HostileDeath(_hostileBlueprint, default, EMovementType.Spline);
        }

        public void Reset()
        {
            _currentNodeIndex = 0;
            NodePosition currentNode = _path[_currentNodeIndex];
            NodePosition targetNode = _path[_currentNodeIndex + 1];
            _currentPosition = new Vector3(currentNode.x, currentNode.y, currentNode.z);
            _targetPosition = new Vector3(targetNode.x, targetNode.y, targetNode.z);
            _hostileTransform.position = _currentPosition;
        }
    }
}