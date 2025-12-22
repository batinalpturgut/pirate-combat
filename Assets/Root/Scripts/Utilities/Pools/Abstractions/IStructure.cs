using System;

namespace Root.Scripts.Utilities.Pools.Abstractions
{
    public interface IStructure<T>
    {
        void Add(T instance);
        T Remove();
        int Count();
        void Init(int size, Func<T> createFunc);
    }
}