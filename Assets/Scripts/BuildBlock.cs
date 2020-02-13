using UnityEngine;

public class BuildBlock : MonoBehaviour
{
    public int buildBlockId;
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