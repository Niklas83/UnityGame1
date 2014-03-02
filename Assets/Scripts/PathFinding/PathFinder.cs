using System;
using UnityEngine;
using System.Collections.Generic;

public class PathFinder
{
	private static bool DEBUG = false;

	IPriorityQueue<Node> PriorityQueue { get; set; }
	HashSet<int> ClosedSet = new HashSet<int>();

	private BaseTile[] mNeighbors = new BaseTile[4];
	private List<BaseTile> mTilesForQueries;
	private GridManager mGridManager;

	private static Vector2[] DIRECTIONS = { Vector2.up, -Vector2.up, Vector2.right, -Vector2.right };

	public PathFinder(GridManager iGridManager) {
		mGridManager = iGridManager;
	}

	public Vector2[] GetPathTo(Vector3 iStartPosition, Vector3 iEndPosition, BaseUnit iUnit, out int oCost)
	{
		Vector2 startIndices = mGridManager.PositionToIndices(iStartPosition);
		Vector2 endIndices = mGridManager.PositionToIndices(iEndPosition);

		Node n = GoTo(startIndices, endIndices, iUnit);
		oCost = n.GetTotalCost();

		bool foundTarget = n.Indices == endIndices;
		if (!foundTarget) 
			return null;
		
		List<Vector2> indices = new List<Vector2>();
		Node node = n; // Starts at the target and backtrace through parents back to beginning.
		while (node != null)
		{
			indices.Add(node.Indices);
			node = node.Parent;
		}

		// Construct a path where each entry is one move in some direction.
		Vector2[] path = new Vector2[indices.Count-1];
		int counter = 0;
		for (int i = indices.Count-1; i > 0; i--) {
			path[counter] = indices[i-1] - indices[i]; // The direction is the difference in indices.
			counter++;
		}

		return path;
	}

	// Converts indices in the grid to a uniqe number
	private int GetAddress(Vector2 iIndices) {
		int x = (int)iIndices.x * 1000; // Implies that we cant have rows/columns with more than 1000 tiles.
		int y = (int)iIndices.y;
		return x + y;
	}
	
	private Node GoTo(Vector2 iStart, Vector2 iTarget, BaseUnit iUnit)
	{
		if (DEBUG) Debug.Log(iStart + " " + iTarget);
		int h1 = GetManhattanDistance(iStart, iTarget);
		
		IPriorityQueue<Node> priorityQueue = new BinaryHeap<Node>(25);
		PriorityQueue = priorityQueue;
		priorityQueue.Enqueue(new Node(iStart, null, 0, h1));
		
		Node bestGuess = null;
		int heuristic = int.MaxValue;
		while (heuristic > 0 && !priorityQueue.IsEmpty()) 
		{
			bestGuess = priorityQueue.Dequeue();
			heuristic = bestGuess.Heuristic;
			if (DEBUG) Debug.Log("Heruistic " + heuristic + " " + bestGuess.GetTotalCost());
			if (heuristic == 0)
				break;
			
			Expand(bestGuess, ref iTarget, iUnit);
			
			if (DEBUG) Debug.Log("Size " + priorityQueue.IsEmpty());
		}
		ClosedSet.Clear();
		priorityQueue.Clear();
		return bestGuess;
	}
	
	private static int GetManhattanDistance(Vector2 a, Vector2 b) 
	{
		return (int) Mathf.Abs(a.x - b.x) + (int) Mathf.Abs(a.y - b.y);
	}
	
	private void Expand(Node iParent, ref Vector2 iTarget, BaseUnit iUnit)
	{
		Vector2 currentIndices = iParent.Indices;
		if (DEBUG) Debug.Log("Expand node at " + currentIndices);
		
		bool okToAdd = ClosedSet.Add(GetAddress(currentIndices));
		
		if (!okToAdd) 
			return;
		
		for (Direction i = Direction.Up; i <= Direction.Left; i++) 
		{ 
			Vector2 nextNodeIndices = currentIndices + DIRECTIONS[(int)i];
			AddNode(nextNodeIndices, iTarget, iParent, iUnit);
		}
	}
	
	private void AddNode(Vector2 iIndices, Vector2 iTarget, Node iParent, BaseUnit iUnit) 
	{
		int address = GetAddress(iIndices);
		if (DEBUG) Debug.Log("Address " + address + " " + iIndices);

		if (!ClosedSet.Contains(address))
		{
			BaseTile t = mGridManager.GetTile((int)iIndices.x, (int)iIndices.y);
			if (t != null && t.CanWalkOn(iUnit)) 
			{
				int cost = (iParent == null ? 0 : iParent.Cost) + 1; // TODO: Add different costs for different tiles
				int heuristic = GetManhattanDistance(iIndices, iTarget);
				if (DEBUG) Debug.Log("Adding node " + iIndices + ", heuristic " + heuristic + ", cost " + cost);
				
				PriorityQueue.Enqueue(new Node(iIndices, iParent, cost, heuristic));
			} 
			else 
			{
				ClosedSet.Add(GetAddress(iIndices));
			}
		}
	}
}

