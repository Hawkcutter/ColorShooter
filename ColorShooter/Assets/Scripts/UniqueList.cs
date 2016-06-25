using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IUnique
{
    int UniqueId { get; set; }
}

public class UniqueList<T> where T : class, IUnique
{
    private List<T> objects;
    private Queue<int> freeIndices;

    public int Count { get { return objects.Count; } }

    public UniqueList(int capacity)
    {
        objects = new List<T>();
        freeIndices = new Queue<int>();

        for (int i = 0; i < capacity; i++)
        {
            objects.Add(null);
            freeIndices.Enqueue(i);
        }
    }

    public T Get(int index)
    {
        return objects[index];
    }

    public void Add(T obj)
    {
        int id = GetFreeIndex();

        Debug.Assert(obj.UniqueId == 0);
        Debug.Assert(id >= 0);
        Debug.Assert(objects[id] == null);
  
        obj.UniqueId = id;
        objects[id] = obj;
    }

    public void Remove(T obj)
    {
        Debug.Assert(obj.UniqueId != -1);
        Debug.Assert(objects[obj.UniqueId] == obj);

        freeIndices.Enqueue(obj.UniqueId);
        objects[obj.UniqueId] = null;
    }

    private int GetFreeIndex()
    {
        if (freeIndices.Count == 0)
        {
            int oldCount = objects.Count;

            for (int i = 0; i < 10; i++)
            {
                objects.Add(null);
                freeIndices.Enqueue(oldCount + i);
            }
        }

        int newId = freeIndices.Dequeue();
        return newId;
    }
}
