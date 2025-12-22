using System;
using System.Collections.Generic;
using Root.Scripts.Managers.Tick.Abstractions;
using Root.Scripts.Utilities.TriggerController.Abstractions;
using UnityEngine;
using UnityEngine.Rendering;

namespace Root.Scripts.Utilities.TriggerController
{
    /// <summary>
    /// This class handle all kinds of in range situations between defenders and hostiles. 
    /// </summary>
    /// <typeparam name="T1">
    /// T1, specifies the type of object to be used in the functions. 
    /// </typeparam>
    /// <typeparam name="T2">
    /// Represents a target hostile type for range detection by the target defender.
    /// </typeparam>
    public class DistanceTriggerController<T1, T2> : ITriggerController where T2 : MonoBehaviour
    {
        private readonly Dictionary<T2, bool> _triggerStates = new Dictionary<T2, bool>();
        private readonly T1 _reference;
        private readonly GameObject _targetObject;
        private readonly ObservableList<T2> _initialTriggerObjects;
        private float _triggerRange;
        private readonly Action<T1, T2> _onTriggerEnter;
        private readonly Action<T1, T2> _onTriggerExit;
        private readonly Action<T1, T2> _onTriggerStay;
        private readonly Action<T1, T2> _onClosestTriggerObjectInRange;
        private readonly Func<T1, T2, bool> _skipCondition;

        private float _newTriggerRange;
        private bool _willTriggerRangeBeUpdated;

        public int ExecutionOrder => 0;

        /// <param name="reference">This parameter represents which object do you want you use in delegates.</param>
        /// <param name="targetObject">This is the defender object checked for range detection.</param>
        /// <param name="initialTriggerObjects">This is the list of all T2's (target hostiles) in the Scene.</param>
        /// <param name="triggerRange">Defender's range.</param>
        /// <param name="onTriggerEnter">This delegate will trigger when a hostile first enters the defender's range.</param>
        /// <param name="onTriggerExit">This delegate will trigger when a hostile exits the defender's range.</param>
        /// <param name="onTriggerStay">This delegate will trigger when a hostile stays in the defender's range.</param>
        /// <param name="onClosestTriggerObjectInRange">This delegate will be triggered for the closest enemy within range.
        /// T2 represent closest in range hostile object.</param>
        /// <param name="skipCondition">If this delegate returns true, nothing will be done with that enemy.</param>
        public DistanceTriggerController(
            T1 reference,
            GameObject targetObject,
            ObservableList<T2> initialTriggerObjects,
            float triggerRange,
            Action<T1, T2> onTriggerEnter = null,
            Action<T1, T2> onTriggerExit = null,
            Action<T1, T2> onTriggerStay = null,
            Action<T1, T2> onClosestTriggerObjectInRange = null,
            Func<T1, T2, bool> skipCondition = null)
        {
            _reference = reference;
            _targetObject = targetObject;
            _initialTriggerObjects = initialTriggerObjects;
            _triggerRange = triggerRange;
            _onTriggerEnter = onTriggerEnter;
            _onTriggerExit = onTriggerExit;
            _onTriggerStay = onTriggerStay;
            _onClosestTriggerObjectInRange = onClosestTriggerObjectInRange;
            _skipCondition = skipCondition;
            _skipCondition ??= (_, _) => false;

            _willTriggerRangeBeUpdated = false;
            _newTriggerRange = triggerRange;

            _initialTriggerObjects.ItemAdded += OnItemAdd;
            _initialTriggerObjects.ItemRemoved += OnItemRemoved;

            GenerateStateDictionary();
        }

        private void GenerateStateDictionary()
        {
            for (int index = 0; index < _initialTriggerObjects.Count; index++)
            {
                T2 triggerObject = _initialTriggerObjects[index];
                _triggerStates.TryAdd(triggerObject, false);
            }
        }

        public void UpdateTriggerRange(float newRange)
        {
            _willTriggerRangeBeUpdated = true;
            _newTriggerRange = newRange;
        }

        public void Calculate()
        {
            // En tepeye tasindi.
            if (_willTriggerRangeBeUpdated)
            {
                _willTriggerRangeBeUpdated = false;
                _triggerRange = _newTriggerRange;
            }

            float closestDistance = Mathf.Infinity;
            T2 closestTriggerObject = null;
            for (int i = _initialTriggerObjects.Count - 1; i >= 0; i--)
            {
                T2 triggerElement = _initialTriggerObjects[i];

                if (triggerElement.gameObject == null)
                {
                    continue;
                }

                if (_skipCondition.Invoke(_reference, triggerElement))
                {
                    continue;
                }

                Vector3 offset = _targetObject.transform.position - triggerElement.transform.position;
                float sqrLen = offset.sqrMagnitude;

                if (sqrLen <= _triggerRange * _triggerRange)
                {
                    // In range.
                    if (sqrLen < closestDistance)
                    {
                        closestDistance = sqrLen;
                        closestTriggerObject = triggerElement;
                    }

                    if (!_triggerStates[triggerElement])
                    {
                        // New comer.
                        _triggerStates[triggerElement] = true;
                        _onTriggerEnter?.Invoke(_reference, triggerElement);
                    }
                    else
                    {
                        // Still in range.
                        _onTriggerStay?.Invoke(_reference, triggerElement);
                    }
                }
                else
                {
                    // Not in range.
                    if (_triggerStates[triggerElement])
                    {
                        // It was in range but not now.
                        _triggerStates[triggerElement] = false;
                        _onTriggerExit?.Invoke(_reference, triggerElement);
                    }
                }
            }

            _onClosestTriggerObjectInRange?.Invoke(_reference, closestTriggerObject);
        }

        private void OnItemAdd(ObservableList<T2> sender, ListChangedEventArgs<T2> e)
        {
            _triggerStates.TryAdd(e.item, false);
        }

        private void OnItemRemoved(ObservableList<T2> sender, ListChangedEventArgs<T2> e)
        {
            if (_triggerStates.ContainsKey(e.item))
            {
                _triggerStates.Remove(e.item);
            }
        }

        ~DistanceTriggerController()
        {
            _initialTriggerObjects.ItemAdded -= OnItemAdd;
            _initialTriggerObjects.ItemRemoved -= OnItemRemoved;
        }
    }
}