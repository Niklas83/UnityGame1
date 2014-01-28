using System;
using UnityEngine;

sealed class Node : IComparable<Node> {

    public Vector2 Indices {get; private set;}
    public Node Parent {get; private set;}
    public int Cost {get; private set;}
    public int Heuristic {get; private set;}

    public Node(Vector2 iIndices, Node iParent, int iCost, int iHeuristic) 
	{
        Indices = iIndices;
        Parent = iParent;
        Cost = iCost;
        Heuristic = iHeuristic;
    }

    public int CompareTo(Node n) 
	{
        return GetTotalCost() < n.GetTotalCost() ? -1 : 1;
    }

    public int GetTotalCost() 
	{
        return Cost + Heuristic;
    }
}
