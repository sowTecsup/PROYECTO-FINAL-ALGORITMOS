using System.Collections.Generic;

public class CustomLinkedList<T>
{
    private Node<T> head;
    private int count;

    public int Count => count;

    public void Add(T value)
    {
        Node<T> newNode = new Node<T>(value);

        if (head == null)
        {
            head = newNode;
        }
        else
        {
            Node<T> temp = head;
            while (temp.Next != null)
            {
                temp = temp.Next;
            }
            temp.Next = newNode;
        }

        count++;
    }

    public bool Remove(T value)
    {
        if (head == null) return false;

        if (EqualityComparer<T>.Default.Equals(head.Value, value))
        {
            head = head.Next;
            count--;
            return true;
        }

        Node<T> prev = head;
        Node<T> curr = head.Next;

        while (curr != null)
        {
            if (EqualityComparer<T>.Default.Equals(curr.Value, value))
            {
                prev.Next = curr.Next;
                count--;
                return true;
            }

            prev = curr;
            curr = curr.Next;
        }

        return false;
    }

    public IEnumerable<T> GetAll()
    {
        Node<T> temp = head;
        while (temp != null)
        {
            yield return temp.Value;
            temp = temp.Next;
        }
    }

    public void Clear()
    {
        head = null;
        count = 0;
    }
}
