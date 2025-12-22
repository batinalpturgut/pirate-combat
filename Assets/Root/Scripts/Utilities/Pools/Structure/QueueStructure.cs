using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Root.Scripts.Utilities.Pools.Abstractions;
using TMPro;

namespace Root.Scripts.Utilities.Pools.Structure
{
    public class QueueStructure<T> : IStructure<T> 
    {
        private Queue<T> _pool;

        public void Init(int size, Func<T> createFunc)
        {
            _pool = new Queue<T>(size);

            for (int i = 0; i < size; i++)
            {
                _pool.Enqueue(createFunc());
            }
        }

        public void Add(T instance)
        {
            _pool.Enqueue(instance);
        }

        public T Remove()
        {
            return _pool.Dequeue();
        }

        public int Count()
        {
            return _pool.Count;
        }
    }
}