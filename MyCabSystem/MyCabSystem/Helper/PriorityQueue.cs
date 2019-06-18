using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCabSystem.Helper
{
    //public class PriorityQueue<T> where T : IComparable
    //{
    //    private readonly object lockObject = new object();
    //    private readonly SortedList<int, Queue<T>> list = new SortedList<int, Queue<T>>();

    //    public int Count
    //    {
    //        get
    //        {
    //            lock (this.lockObject)
    //            {
    //                return list.Sum(keyValuePair => keyValuePair.Value.Count);
    //            }
    //        }
    //    }

    //    public void Push(int priority, T item)
    //    {
    //        lock (this.lockObject)
    //        {
    //            if (!this.list.ContainsKey(priority))
    //                this.list.Add(priority, new Queue<T>());
    //            this.list[priority].Enqueue(item);
    //        }
    //    }
    //    public T Pop()
    //    {
    //        lock (this.lockObject)
    //        {
    //            if (this.list.Count > 0)
    //            {
    //                T obj = this.list.First().Value.Dequeue();
    //                if (this.list.First().Value.Count == 0)
    //                    this.list.Remove(this.list.First().Key);
    //                return obj;
    //            }
    //        }
    //        return null;
    //    }
    //    public T PopPriority(int priority)
    //    {
    //        lock (this.lockObject)
    //        {
    //            if (this.list.ContainsKey(priority))
    //            {
    //                T obj = this.list[priority].Dequeue();
    //                if (this.list[priority].Count == 0)
    //                    this.list.Remove(priority);
    //                return obj;
    //            }
    //        }
    //        return null;
    //    }
    //    public IEnumerable<T> PopAllPriority(int priority)
    //    {
    //        List<T> ret = new List<T>();
    //        lock (this.lockObject)
    //        {
    //            if (this.list.ContainsKey(priority))
    //            {
    //                while (this.list.ContainsKey(priority) && this.list[priority].Count > 0)
    //                    ret.Add(PopPriority(priority));
    //                return ret;
    //            }
    //        }
    //        return ret;
    //    }
    //    public T Peek()
    //    {
    //        lock (this.lockObject)
    //        {
    //            if (this.list.Count > 0)
    //                return this.list.First().Value.Peek();
    //        }
    //        return null;
    //    }
    //    public IEnumerable<T> PeekAll()
    //    {
    //        List<T> ret = new List<T>();
    //        lock (this.lockObject)
    //        {
    //            foreach (KeyValuePair<int, Queue<T>> keyValuePair in list)
    //                ret.AddRange(keyValuePair.Value.AsEnumerable());
    //        }
    //        return ret;
    //    }
    //    public IEnumerable<T> PopAll()
    //    {
    //        List<T> ret = new List<T>();
    //        lock (this.lockObject)
    //        {
    //            while (this.list.Count > 0)
    //                ret.Add(Pop());
    //        }
    //        return ret;
    //    }
    //}


    class PriorityQueue<T> where T : IComparable
    {
        private List<T> list;
        public int Count { get { return list.Count; } }
        public readonly bool IsDescending;

        public PriorityQueue()
        {
            list = new List<T>();
        }

        public PriorityQueue(bool isdesc)
            : this()
        {
            IsDescending = isdesc;
        }

        public PriorityQueue(int capacity)
            : this(capacity, false)
        { }

        public PriorityQueue(IEnumerable<T> collection)
            : this(collection, false)
        { }

        public PriorityQueue(int capacity, bool isdesc)
        {
            list = new List<T>(capacity);
            IsDescending = isdesc;
        }

        public PriorityQueue(IEnumerable<T> collection, bool isdesc)
            : this()
        {
            IsDescending = isdesc;
            foreach (var item in collection)
                Enqueue(item);
        }


        public void Enqueue(T x)
        {
            list.Add(x);
            int i = Count - 1;

            while (i > 0)
            {
                int p = (i - 1) / 2;
                if ((IsDescending ? -1 : 1) * list[p].CompareTo(x) <= 0) break;

                list[i] = list[p];
                i = p;
            }

            if (Count > 0) list[i] = x;
        }

        public T Dequeue()
        {
            T target = Peek();
            T root = list[Count - 1];
            list.RemoveAt(Count - 1);

            int i = 0;
            while (i * 2 + 1 < Count)
            {
                int a = i * 2 + 1;
                int b = i * 2 + 2;
                int c = b < Count && (IsDescending ? -1 : 1) * list[b].CompareTo(list[a]) < 0 ? b : a;

                if ((IsDescending ? -1 : 1) * list[c].CompareTo(root) >= 0) break;
                list[i] = list[c];
                i = c;
            }

            if (Count > 0) list[i] = root;
            return target;
        }

        public T Peek()
        {
            if (Count == 0) throw new InvalidOperationException("Queue is empty.");
            return list[0];
        }

        public bool Contains(T x)
        {
            if (list.Contains(x))
                return true;
            else
                return false;
        }
        public void Clear()
        {
            list.Clear();
        }
    }
}
