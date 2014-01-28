using System;

sealed class BinaryHeap<T> : IPriorityQueue<T> where T : IComparable<T> {

    int CurrentSize;
    T[] Array;

    public BinaryHeap(int iCapacity) 
	{
        CurrentSize = 0;
        Array = new T[iCapacity];
    }

    public void Enqueue(T iElement) 
	{
        if (CurrentSize == Array.Length - 1) 
		{
            DoubleArray();
        }
    
        int hole = ++CurrentSize;
        Array[ 0 ] = iElement;
        for (; iElement.CompareTo(Array[hole / 2]) < 0; hole /= 2 ) 
		{
            Array[hole] = Array[hole / 2];
        }

        Array[hole] = iElement;
    }

    public T Peek() 
	{
        if (IsEmpty()) 
		{
            throw new InvalidOperationException("Queue is empty!");
        }
        return Array[1];
    }

    public T Dequeue( ) 
	{
        T minItem = Peek();
        Array[1] = Array[CurrentSize--];
        TrickleDown(1);
        return minItem;
    }

    public bool IsEmpty() 
	{
        return CurrentSize == 0;
    }

    public void Clear() 
	{
        CurrentSize = 0;
    }

    private void BuildHeap() 
	{
        for (int i = CurrentSize / 2; i > 0; i--) 
		{
            TrickleDown(i);
        }
    }

    private void TrickleDown(int hole) 
	{
        int child;
        T tmp = Array[hole];
        
        bool foundInsertionPoint = false;
        while (hole * 2 <= CurrentSize && !foundInsertionPoint) 
		{
            child = hole * 2;
            if (child != CurrentSize && Array[child + 1].CompareTo(Array[child]) < 0 ) 
			{
                child++;
            }

            foundInsertionPoint = Array[child].CompareTo(tmp) >= 0;
            if (!foundInsertionPoint) 
			{
                Array[hole] = Array[child];
                hole = child;
            }
        }
        Array[hole] = tmp;
    }

    private void DoubleArray( )
	{
        T [] newArray;
    
        newArray = new T[Array.Length * 2];
        for (int i = 0; i < Array.Length; i++) 
		{
            newArray[ i ] = Array[ i ];
        }
        Array = newArray;
    }
}