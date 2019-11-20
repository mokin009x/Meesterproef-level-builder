using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace : MonoBehaviour
{
    public GridSpaceValues values;
    public GameObject buildBlock;
    public Vector3 currentPos;
    // Start is called before the first frame update
    void Start()
    {
        currentPos = this.transform.position;
    }

    public void PlaceBlock(int buildBlockId , Vector3 position)
    {
        if (values.hasBuildBlock != true)
        {
            values.buildBlockId = buildBlockId;
            values.hasBuildBlock = true;
            
            GameObject blockPrefab = LevelBuilderManager.Instance.buildBlocksPrefabs[buildBlockId];
            buildBlock = Instantiate(blockPrefab,position,Quaternion.identity);
            
            BuildBlock buildBlockClass = buildBlock.GetComponent<BuildBlock>();
            buildBlockClass.gridSpacePair = GridManager.Instance.levelGrid[values.gridId].GetComponent<GridSpace>();
            buildBlock.GetComponent<BuildBlock>().pairId = values.gridId;
            LevelBuilderManager.Instance.levelBlocks.Add(buildBlock); 
        }
    }

    public void MonsterPathMarkerPlace(GameObject marker, Vector3 position)
    {
        if (values.hasMarker != true)
        {
            values.hasMarker = true;

            Vector3 offset = new Vector3(position.x, position.y + 1, position.z);
            Instantiate(marker, offset, Quaternion.identity);  
            LevelBuilderManager.Instance.monsterPath.Add(values.gridId);
        }
    }

    public void BuildAreaAssign(GameObject buildAreaMarker)
    {
        if (values.buildArea != true)
        {
            values.buildArea = true;
            gameObject.isStatic = true;
            
        }
        else
        {
            values.buildArea = false;
        }
    }


}
