using UnityEngine;

interface ISearchAlgorithm 
{
    void Init();

    Node StartSearch(Vector2 position, Vector2 target, Direction direction);
}
