using System.Collections;
using System.Net.Sockets;

namespace OOD_Project;

public interface IMyCollection<T>// : IEnumerable<T>
    where T : class
{
    public IEnumerator<T> GetForwardEnumerator();

    public IEnumerator<T> GetReverseEnumerator();

    public void Add(T value);

    public void Remove(T value);
}

public class Vector<T> : IMyCollection<T>
    where T : class
{
    private List<T> elements = new();

    public IEnumerator<T> GetForwardEnumerator()
    {
        return elements.GetEnumerator();
    }

    public IEnumerator<T> GetReverseEnumerator()
    {
        for (var i = elements.Count - 1; i >= 0; i--)
        {
            yield return elements[i];
        }
    }

    public void Add(T value)
    {
        elements.Add(value);
    }

    public void Remove(T value)
    {
        elements.Remove(value);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return GetForwardEnumerator();
    }
}

public class LLNode<T>
{
    public T value;
    public LLNode<T>? next = null;
    public LLNode<T>? prev = null;

    public LLNode(T value, LLNode<T> prev = null, LLNode<T> next = null)
    {
        this.value = value;
        this.prev = prev;
        this.next = next;
    }
}

public class DoublyLinkedList<T> : IMyCollection<T>
    where T : class
{
    private LLNode<T>? head;
    private LLNode<T>? tail;

    public IEnumerator<T> GetForwardEnumerator()
    {
        for (var el = head; el != null; el = el.next)
        {
            yield return el.value;
        }
    }

    public IEnumerator<T> GetReverseEnumerator()
    {
        for (var el = tail; el != null; el = el.prev)
        {
            yield return el.value;
        }
    }

    public void Add(T value)
    {
        if (head == null)
        {
            head = new LLNode<T>(value);
            tail = head;
        }
        else
        {
            tail.next = new LLNode<T>(value)
            {
                prev = tail
            };
            tail = tail.next;
        }
    }

    public void Remove(T value)
    {
        for (var el = head; el != null; el = el.next)
        {
            if (el.value != value) continue;
            if (el == head && el == tail)
            {
                head = null;
                tail = null;
            }
            else
            {
                if (el.prev != null) el.prev.next = el.next;
                if (el.next != null) el.next.prev = el.prev;
            }
            return;
        }
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        return GetForwardEnumerator();
    }
}

public class MaxHeap<T> : IMyCollection<T>
    where T : class
{
    private List<T> data = new();
    private Func<T, T, bool> IsBigger;

    private bool IsSmaller(T a, T b) => IsBigger(b, a);

    public MaxHeap(Func<T, T, bool> comparator)
    {
        this.IsBigger = comparator;
    }

    private int GetIndexOfBigger(int left, int right)
    {
        return IsBigger(data[right], data[left]) ? right : left;
    }

    private void UpHeap(int index)
    {
        //loop until current element is first
        while (index > 0)
        {
            var parent_index = (int)Math.Floor((double)((index - 1) / 2));
            //if the child is smaller/equal, do nothing
            if (!IsBigger(this.data[index], this.data[parent_index])) return;
            //else swap with parent and check the parent
            (data[index], data[parent_index]) = (data[parent_index], data[index]);
            index = parent_index;
        }
    }
    
    private void DownHeap(int index)
    {
        //loop until current element is first
        while (index > 0)
        {
            var left_child = 2 * index + 1;
            var right_child = 2 * index + 2;

            var bigger_child = GetIndexOfBigger(left_child, right_child);

            //if the bigger child is smaller/equal, do nothing
            if (!IsBigger(data[bigger_child], data[index])) return;
            //else swap and check from that child
            (data[index], data[bigger_child]) = (data[bigger_child], data[index]);
            index = bigger_child;
        }
    }

    public IEnumerator<T> GetForwardEnumerator()
    {
        for (var i = 0; i < data.Count; i++)
        {
            yield return data[i];
        }
    }

    public IEnumerator<T> GetReverseEnumerator()
    {
        for (var i = data.Count - 1; i >= 0; i--)
        {
            yield return data[i];
        }
    }

    public void Add(T value)
    {
        this.data.Add(value);
        UpHeap(this.data.Count - 1);
    }

    public void Remove(T value)
    {
        if(value != this.data[0]) return;
        //swap the elements
        (data[0], data[^1]) = (data[^1], data[0]);
        //remove the previous root
        data.RemoveAt(data.Count - 1);
        DownHeap(0);
    }

    public void Delete()
    {
        Remove(data[0]);
    }
}