using System;

/// <summary>
/// This interface should be implemented by all priority queue in this game.
/// </summary>
/// <typeparam name="T">Any type which is comparable</typeparam>
interface IPriorityQueue<T> where T : IComparable<T> {

    /// <summary>
    /// Enqueues (or inserts) the given element it this queue.
    /// </summary>
    /// <param name="element">The element to insert</param>
    void Enqueue(T element);

    /// <summary>
    /// Returns the first element in the queue without removing it.
    /// </summary>
    /// <returns>The first element of the queue</returns>
    T Peek();

    /// <summary>
    /// Returns the first element in the queue and removes it.
    /// </summary>
    /// <returns>The first element of the queue</returns>
    T Dequeue();

    /// <summary>
    ///  Checks if this queue is empty
    /// </summary>
    /// <returns>True if it is empty, False otherwise</returns>
    bool IsEmpty();

    /// <summary>
    ///  Empties the queue.
    /// </summary>
    void Clear();
}
