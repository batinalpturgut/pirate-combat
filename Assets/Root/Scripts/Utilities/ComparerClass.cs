using System.Collections.Generic;
using Root.Scripts.Managers.Tick.Abstractions;

namespace Root.Scripts.Utilities
{
    public class ComparerClass // IComparer interface'ini implement eden siniflar icin kullanilan genel sinif.
    {
        public static IComparer<ITickable> DescendingTickComparer { get; } = new ByDescendingExecutionOrder();

        public static IComparer<ITickable> AscendingTickComparer { get; } = new ByAscendingExecutionOrder();

        private class ByDescendingExecutionOrder : IComparer<ITickable>
        {
            public int Compare(ITickable x, ITickable y)
            {
                return x.ExecutionOrder.CompareTo(y.ExecutionOrder);
            }
        }

        private class ByAscendingExecutionOrder : IComparer<ITickable>
        {
            public int Compare(ITickable x, ITickable y)
            {
                return y.ExecutionOrder.CompareTo(x.ExecutionOrder);
            }
        }
    }
}