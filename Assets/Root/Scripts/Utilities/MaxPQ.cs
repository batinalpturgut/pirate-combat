using System;
using System.Collections.Generic;
using System.Linq;

namespace Root.Scripts.Utilities
{
    public class MaxPQ<TKey>
    {
        private TKey[] _pq;
        private int _n;
        private IComparer<TKey> _comparer;

        private const int InitialSize = 5;

        public MaxPQ() : this(InitialSize)
        {
        }

        public MaxPQ(int initialSize)
        {
            _pq = new TKey[initialSize + 1];
            _n = 0;
        }


        public MaxPQ(Comparer<TKey> comparer) : this(InitialSize, comparer)
        {
        }

        public MaxPQ(int initialSize, Comparer<TKey> comparer)
        {
            _comparer = comparer;
            _pq = new TKey[initialSize + 1];
            _n = 0;
        }


        public bool IsEmpty()
        {
            return _n == 0;
        }

        public int Size()
        {
            return _n;
        }

        public TKey Max()
        {
            return _pq[1];
        }

        private void Resize(int capacity)
        {
            TKey[] temp = new TKey[capacity];
            for (int i = 0; i <= _n; i++)
            {
                temp[i] = _pq[i];
            }

            _pq = temp;
        }

        public void Insert(TKey x)
        {
            if (_n == _pq.Length - 1)
            {
                Resize(_pq.Length * 2);
            }

            _pq[++_n] = x;
            Swim(_n);

#if UNITY_EDITOR
            if (!IsMaxHeap())
            {
                throw new Exception($"{this} Priority queue is broken");
            }
#endif
        }


        public TKey DelMax()
        {
            if (IsEmpty())
            {
                throw new Exception("Priority queue underflow");
            }

            TKey max = _pq[1];

            Exch(1, _n--);

            Sink(1);

            _pq[_n + 1] = default; // to avoid loitering and help with garbage collection
            // _pq[_n + 1] = null; 

            if ((_n > 0) && (_n == (_pq.Length - 1) / 4)) Resize(_pq.Length / 2);

#if UNITY_EDITOR
            if (!IsMaxHeap())
            {
                throw new Exception($"{this} Priority queue is broken");
            }
#endif
            return max;
        }

        private void Swim(int k)
        {
            while (k > 1 && Less(k / 2, k))
            {
                Exch(k / 2, k);
                k /= 2;
            }
        }

        private void Sink(int k)
        {
            while (2 * k <= _n)
            {
                int j = 2 * k;

                if (j < _n && Less(j, j + 1))
                {
                    j++;
                }

                if (!Less(k, j))
                {
                    break;
                }

                Exch(k, j);
                k = j;
            }
        }

        private bool IsMaxHeap()
        {
            for (int i = 1; i <= _n; i++)
            {
                if (_pq[i] == null) return false;
            }

            for (int i = _n + 1; i < _pq.Length; i++)
            {
                if (_pq[i] != null) return false;
            }

            if (_pq[0] != null)
            {
                return false;
            }

            return IsMaxHeapOrdered(1);
        }

        private bool IsMaxHeapOrdered(int k)
        {
            if (k > _n)
            {
                return true;
            }

            int left = 2 * k;
            int right = 2 * k + 1;
            if (left <= _n && Less(k, left))
            {
                return false;
            }

            if (right <= _n && Less(k, right))
            {
                return false;
            }

            return IsMaxHeapOrdered(left) && IsMaxHeapOrdered(right);
        }

        private void Exch(int i, int j)
        {
            TKey swap = _pq[i];
            _pq[i] = _pq[j];
            _pq[j] = swap;
        }

        private bool Less(int i, int j)
        {
            if (_comparer == null)
            {
                if (_pq[i] is IComparable<TKey> comparable)
                {
                    return comparable.CompareTo(_pq[j]) < 0;
                }

                throw new Exception($"{this} You must provide a Comparator or Comparable Key for a PriorityQueue");
            }

            return _comparer.Compare(_pq[i], _pq[j]) < 0;
        }
        
        // TODO: Enumarable yapilabilir.
    }
}