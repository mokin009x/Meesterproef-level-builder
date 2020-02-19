using UnityEngine;

public class GridBlock : MonoBehaviour
{
    public int buildBlockId;
    public int specialBlockId;
    public GridSpace gridSpacePair;
    public int pairId;
    public bool walkable;

    private void Start()
    {
        if (walkable)
        {
            gameObject.isStatic = true;
        }
    }
}