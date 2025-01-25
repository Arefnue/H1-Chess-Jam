using System;
using System.Collections.Generic;

namespace __Project.Systems.GridSystem._PathSubSystem
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> _data = new();

        public void Enqueue(T item)
        {
            _data.Add(item);
            int childIndex = _data.Count - 1;

            while (childIndex > 0)
            {
                int parentIndex = (childIndex - 1) / 2;
                if (_data[childIndex].CompareTo(_data[parentIndex]) >= 0)
                {
                    break;
                }

                (_data[childIndex], _data[parentIndex]) = (_data[parentIndex], _data[childIndex]);
                childIndex = parentIndex;
            }
        }

        public T Dequeue()
        {
            int lastIndex = _data.Count - 1;
            T frontItem = _data[0];
            _data[0] = _data[lastIndex];
            _data.RemoveAt(lastIndex);

            --lastIndex;
            int parentIndex = 0;
            while (true)
            {
                int leftChildIndex = parentIndex * 2 + 1;
                if (leftChildIndex > lastIndex)
                {
                    break;
                }

                int rightChildIndex = leftChildIndex + 1;
                int minChildIndex = (rightChildIndex > lastIndex || _data[leftChildIndex].CompareTo(_data[rightChildIndex]) < 0) ? leftChildIndex : rightChildIndex;

                if (_data[parentIndex].CompareTo(_data[minChildIndex]) <= 0)
                {
                    break;
                }

                (_data[parentIndex], _data[minChildIndex]) = (_data[minChildIndex], _data[parentIndex]);
                parentIndex = minChildIndex;
            }

            return frontItem;
        }

        public int Count => _data.Count;

        public bool Contains(T item)
        {
            return _data.Contains(item);
        }
    }
}