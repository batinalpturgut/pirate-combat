using System;
using System.Collections.Generic;
using System.Linq;
using Root.Scripts.Grid;
using Root.Scripts.Managers.Grid;
using Root.Scripts.Utilities.TriggerController.Abstractions;

namespace Root.Scripts.Utilities.TriggerController
{
    // Ayni grid'de birden fazla T2 bulunamaz. Bulunursa yanlis calisacaktir. Eger yapilmak istenirse
    // NodeObject'te tek bir AEnemy tutmak yerine EnemyList tutulması gerekir.
    public class GridTriggerController<T1, T2> : ITriggerController
    {
        private NodePosition _offsetNode;
        private NodePosition _targetNode;
        private T1 _reference;
        private int _gridRange;
        private GridManager _gridManager;
        private Action<T1, T2> _onTriggerEnter;
        private Action<T1, T2> _onTriggerExit;
        private Action<T1, T2> _onTriggerStay;
        private Func<T1, T2, bool> _skipCondition;
        private Dictionary<T2, bool> _triggerEntities = new Dictionary<T2, bool>();
        private List<T2> _outOfRangeEntities = new List<T2>();
        private List<T2> _resetEntities = new List<T2>();
        private List<T2> _inRangeEntities = new List<T2>();
        private bool _willTriggerRangeBeUpdated;
        private int _newGridRange;
        public int ExecutionOrder => 0;


        public GridTriggerController(T1 reference, NodePosition offsetNode, int range, GridManager gridManager,
            Action<T1, T2> onTriggerEnter = null, Action<T1, T2> onTriggerExit = null,
            Action<T1, T2> onTriggerStay = null, Func<T1, T2, bool> skipCondition = null)
        {
            _reference = reference;
            _offsetNode = offsetNode;
            _gridRange = range;
            _gridManager = gridManager;
            _onTriggerEnter = onTriggerEnter;
            _onTriggerStay = onTriggerStay;
            _onTriggerExit = onTriggerExit;
            _skipCondition = skipCondition;
            _skipCondition ??= (_, _) => false;
        }

        public void Calculate()
        {
            if (_willTriggerRangeBeUpdated)
            {
                _willTriggerRangeBeUpdated = false;
                _gridRange = _newGridRange;
            }

            Reset();
            for (int x = -_gridRange; x <= _gridRange; x++)
            {
                for (int z = -_gridRange; z < _gridRange; z++)
                {
                    _targetNode.Set(_offsetNode.x + x, _offsetNode.z + z);

                    if (!_gridManager.IsValidNodePosition(_targetNode))
                    {
                        // It's not valid position. So do nothing.
                        continue;
                    }

                    if (_targetNode.Equals(_offsetNode))
                    {
                        // Kendi kendine etki uygulamamasi lazim.
                        continue;
                    }

                    if (!_gridManager.TryGetNodeObjectEntity(_targetNode, out T2 entity))
                    {
                        // Empty node.
                        continue;
                    }

                    // _targetNode has a T2 entity.

                    if (_skipCondition.Invoke(_reference, entity))
                    {
                        _triggerEntities[entity] = true;
                        continue;
                    }


                    if (_triggerEntities.TryGetValue(entity, out _))
                    {
                        // This entity still in range.
                        _onTriggerStay?.Invoke(_reference, entity);
                        _triggerEntities[entity] = true;
                    }
                    else
                    {
                        // New comer. 
                        if (_triggerEntities.TryAdd(entity, true))
                        {
                            _onTriggerEnter?.Invoke(_reference, entity);
                        }
                    }
                }
            }

            HandleOutOfRangeEntities();
        }

        public List<T2> AllInRangeEntities()
        {
            _inRangeEntities.Clear();
            foreach (var entity in _triggerEntities)
            {
                if (entity.Value)
                {
                    _inRangeEntities.Add(entity.Key);
                }
            }

            return _inRangeEntities;
        }

        public void ClearAllInRangeEntities()
        {
            _triggerEntities.Clear();
        }

        // TODO: Exit Null hatasi veriyor.

        public void UpdateTriggerRange(float newGridRange)
        {
            _willTriggerRangeBeUpdated = true;
            _newGridRange = (int)newGridRange;
        }

        private void Reset()
        {
            _resetEntities.Clear();

            foreach (T2 key in _triggerEntities.Keys)
            {
                _resetEntities.Add(key);
            }

            foreach (T2 key in _resetEntities)
            {
                _triggerEntities[key] = false;
            }
        }

        private void HandleOutOfRangeEntities()
        {
            _outOfRangeEntities.Clear();
            foreach (var entity in _triggerEntities)
            {
                if (!entity.Value)
                {
                    _outOfRangeEntities.Add(entity.Key);
                }
            }

            foreach (var entity in _outOfRangeEntities)
            {
                _onTriggerExit?.Invoke(_reference, entity);
                _triggerEntities.Remove(entity);
            }
        }
    }
}