using Root.Scripts.Utilities.Pools.Abstractions;
using Root.Scripts.Utilities.Pools.Structure;

namespace Root.Scripts.Utilities.Pools
{
    public static class PoolType<T1> where T1 : new()
    {
        public static IStructure<T1> Stack => new StackStructure<T1>();
        public static IStructure<T1> Queue => new QueueStructure<T1>();
    }
}