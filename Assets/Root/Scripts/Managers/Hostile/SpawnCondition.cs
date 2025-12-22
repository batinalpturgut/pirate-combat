using System;
using Root.Scripts.Utilities.Pools;
using UnityEngine;

namespace Root.Scripts.Managers.Hostile
{
    public class SpawnCondition
    {
        public Func<bool> Condition;
        private HostileSpawner _hostileSpawner;
        private int _parameter;
        private bool _isTimerActive;
        private float _targetTime;

        private static readonly Pool<SpawnCondition, ConditionConfig> _pool =
            new Pool<SpawnCondition, ConditionConfig>(() => new SpawnCondition(), PoolType<SpawnCondition>.Stack,
                onGet: (condition, config) =>
                {
                    switch (config.SpawnCondition)
                    {
                        case ESpawnCondition.WaitForSeconds:
                            condition.Condition += condition.WaitForSeconds;
                            break;
                        case ESpawnCondition.None:
                            condition.Condition += condition.None;
                            break;
                    }

                    condition._hostileSpawner = config.HostileSpawner;
                    condition._parameter = config.Parameter;
                },
                initialSize: 5,
                onReturn: (condition) => { condition.Reset(); });

        private static ConditionConfig _poolOperand = new ConditionConfig();


        public static SpawnCondition GetSpawnCondition(HostileSpawner hostileSpawner, ESpawnCondition spawnCondition,
            int parameter)
        {
            _poolOperand.Set(spawnCondition, hostileSpawner, parameter);
            return _pool.Get(_poolOperand);
        }

        public void ReturnSpawnCondition()
        {
            _pool.Return(this);
        }

        private bool None()
        {
            return true;
        }

        private bool WaitForSeconds()
        {
            if (!_isTimerActive)
            {
                _isTimerActive = true;
                _targetTime = _parameter + Time.time;
            }

            bool isFinished = Time.time >= _targetTime;
            _isTimerActive = !isFinished;
            return isFinished;
        }

        private void Reset()
        {
            Condition = null;
            _hostileSpawner = null;
            _parameter = 0;
            _isTimerActive = false;
            _targetTime = 0;
        }


        private class ConditionConfig
        {
            public ESpawnCondition SpawnCondition;
            public HostileSpawner HostileSpawner;
            public int Parameter;

            public void Set(ESpawnCondition condition, HostileSpawner hostileSpawner, int parameter)
            {
                SpawnCondition = condition;
                HostileSpawner = hostileSpawner;
                Parameter = parameter;
            }
        }
    }
}