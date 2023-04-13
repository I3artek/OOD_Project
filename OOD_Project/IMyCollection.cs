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