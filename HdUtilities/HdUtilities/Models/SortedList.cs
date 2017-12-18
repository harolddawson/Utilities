using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HdUtilities.Models
{
    public class SortedList<T> : IList<T>
    {
        private readonly List<T> internalList;
        private readonly IComparer<T> internalComparer; 

        public SortedList(IComparer<T> comparer)
        {
            internalList = new List<T>();
            internalComparer = comparer;
        }

        public SortedList(IEnumerable<T> enumerable, IComparer<T> comparer)
        {
            internalList = enumerable.ToList();
            internalComparer = comparer;
            internalList.Sort(internalComparer);
        }

        public IEnumerator<T> GetEnumerator()
        {
            internalList.Sort(internalComparer);
            return internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            internalList.Add(item);
            internalList.Sort(internalComparer);
        }

        public void Clear()
        {
            internalList.Clear();
        }

        public bool Contains(T item)
        {
            return internalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            internalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return internalList.Remove(item);
        }

        public int Count => internalList.Count;

        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            internalList.Sort(internalComparer);
            return internalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            internalList.Insert(index, item);
            internalList.Sort(internalComparer);
        }

        public void RemoveAt(int index)
        {
            internalList.RemoveAt(index);
            internalList.Sort(internalComparer);
        }

        public T this[int index]
        {
            get { return internalList[index]; }
            set { internalList[index] = value; }
        }
    }
}