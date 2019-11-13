using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace : MonoBehaviour
{
    public LevelBuilderManager lvlBuildManager;
    public GridManager grdManager;
    public GridSpaceValues values;
    public GameObject buildBlock;
    public Vector3 currentPos;
    // Start is called before the first frame update
    void Start()
    {
        grdManager = GameObject.Find("Managers").GetComponent<GridManager>();
        lvlBuildManager = GameObject.Find("Managers").GetComponent<LevelBuilderManager>();
        currentPos = this.transform.position;
    }

    public void PlaceBlock(int buildBlockId , Vector3 position)
    {
        values.buildBlockId = buildBlockId;
        values.hasBuildBlock = true;
        Debug.Log(buildBlockId);
        GameObject block = lvlBuildManager.buildBlocksPrefabs[buildBlockId];
        buildBlock = Instantiate(block,position,Quaternion.identity);
        var test = buildBlock.GetComponent<BuildBlock>();
        Debug.Log(values.gridId);    
        test.gridSpacePair = grdManager.levelGrid[values.gridId].GetComponent<GridSpace>();
        buildBlock.GetComponent<BuildBlock>().pairId = values.gridId;
        lvlBuildManager.levelBlocks.Add(buildBlock);
    }
    

}
