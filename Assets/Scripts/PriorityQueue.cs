using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PriorityQueue<T> {
    public List<T> list;
    public int Count { get { return list.Count; } }

    private IComparer<T> comparator;
    public PriorityQueue() {
        list = new List<T>();
    }

    public PriorityQueue(IComparer<T> comparator) {
        list = new List<T>();
        this.comparator = comparator;
    }

    public void Enqueue(T x) {
        list.Add(x);
        int i = Count - 1;
         
        while (i > 0) {
            int p = (i - 1) / 2;
            if (comparator.Compare(list[p], x) < 1) break;
            list[i] = list[p];
            i = p;
        }

        if (Count > 0) list[i] = x;
    }

    public T Dequeue() {
        T min = Peek();
        T root = list[Count - 1];
        list.RemoveAt(Count - 1);

        int i = 0;
        while (i * 2 + 1 < Count) {
            int a = i * 2 + 1;
            int b = i * 2 + 2;
            int c = b < Count && (comparator.Compare(list[b],list[a]) < 0) ? b : a;

            if (comparator.Compare(list[c],root) > -1) break;
            list[i] = list[c];
            i = c;
        }

        if (Count > 0) list[i] = root;
        return min;
    }

    public T Peek() {
        return list[0];
    }

    public void Clear() {
        list.Clear();
    }
}