using System;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.Logger.Enums;
using Root.Scripts.Utilities.Pools.Abstractions;
using Root.Scripts.Utilities.Pools.Structure;

namespace Root.Scripts.Utilities.Pools
{
    public class Pool<T1, T2> where T2 : class
    {
        private const int InitialSize = 5;
        private IStructure<T1> _poolType;
        private Func<T1> _createFunc;
        private Action<T1, T2> _onGet;
        private Action<T1> _onReturn;

        public Pool(Func<T1> createFunc, IStructure<T1> poolType, int initialSize = 0,
            Action<T1, T2> onGet = null,
            Action<T1> onReturn = null)
        {
            if (createFunc == null)
            {
                Log.Console("Creation function can't be null", LogType.Error);
            }

            if (poolType == null)
            {
                Log.Console("Pool type can't be null", LogType.Error);
            }

            var size = initialSize == 0 ? InitialSize : initialSize;
            _poolType = poolType;
            _createFunc = createFunc;
            poolType.Init(size, createFunc);
            _onGet = onGet;
            _onReturn = onReturn;
        }


        public T1 Get(T2 config)
        {
            T1 instance;
            if (_poolType.Count() > 0)
            {
                instance = _poolType.Remove();
            }
            else
            {
                instance = _createFunc();
            }

            _onGet?.Invoke(instance, config);
            return instance;
        }

        public void Return(T1 instance)
        {
            _onReturn?.Invoke(instance);
            _poolType.Add(instance);
        }
    }
}