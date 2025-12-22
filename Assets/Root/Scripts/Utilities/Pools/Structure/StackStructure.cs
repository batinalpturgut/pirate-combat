using System;
using System.Collections.Generic;
using Root.Scripts.Utilities.Pools.Abstractions;

namespace Root.Scripts.Utilities.Pools.Structure
{
    public class StackStructure<T> : IStructure<T> 
    {
        private Stack<T> _pool;

        public void Init(int size, Func<T> createFunc)
        {
            _pool = new Stack<T>(size);
            for (int i = 0; i < size; i++)
            {
                _pool.Push(createFunc());
            }
        }

        public void Add(T instance)
        {
            _pool.Push(instance);
        }

        public T Remove()
        {
            return _pool.Pop();
        }

        public int Count()
        {
            return _pool.Count;
        }
    }
}