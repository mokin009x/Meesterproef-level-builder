using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace : MonoBehaviour
{
    public LevelBuilderManager lvlBuildManager;
    public GridManager grdManager;
    public GridSpaceValues values;
    public GameObject buildBlock;
    // Start is called before the first frame update
    void Start()
    {
        grdManager = GameObject.Find("Managers").GetComponent<GridManager>();
        grdManager = GameObject.Find("Managers").GetComponent<GridManager>();
    }

    public void PlaceBlock(int blockId)
    {
        values.blockId = blockId;
        values.hasBuildBlock = true;
        GameObject block = lvlBuildManager.buildBlocksPrefabs[blockId];
        buildBlock = Instantiate(block,transform.position,Quaternion.identity);
        buildBlock.GetComponent<BuildBlock>().gridSpacePair = this.gameObject;
        lvlBuildManager.levelBlocks.Add(buildBlock);
    }


}
