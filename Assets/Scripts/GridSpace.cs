using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    public void PlaceBlock(List<GameObject> prefabList,int buildBlockId , Vector3 position)//prefablist is always buildblock prefabs.
    {
        if (values.hasBuildBlock != true)
        {
            values.hasBuildBlock = true;
            
            GameObject blockPrefab = prefabList[buildBlockId];
            buildBlock = Instantiate(blockPrefab,position,blockPrefab.transform.rotation);
            
            BuildBlock buildBlockClass = buildBlock.GetComponent<BuildBlock>();
            buildBlockClass.gridSpacePair = GridManager.Instance.levelGrid[values.gridId].GetComponent<GridSpace>();
            buildBlock.GetComponent<BuildBlock>().pairId = values.gridId;
            buildBlock.GetComponent<BuildBlock>().buildBlockId = buildBlockId;
            LevelBuildAndPlayManager.Instance.levelBlocks.Add(buildBlock);
            buildBlock.GetComponent<NavMeshModifier>().area = 1;
            buildBlock.GetComponent<NavMeshModifier>().overrideArea = true;
            if (buildBlockId == 1)//soil block that is walkable
            {
                buildBlock.GetComponent<NavMeshModifier>().area = 0;
                buildBlock.GetComponent<NavMeshModifier>().overrideArea = true;
                LevelBuildAndPlayManager.Instance.walkableSurfaces.Add(buildBlock.GetComponent<NavMeshSurface>());
            }
        }
    }

    public void RemoveBlock()
    {
        if (values.hasBuildBlock == true)
        {
            Destroy(buildBlock);
            values.hasBuildBlock = false;
            LevelBuildAndPlayManager.Instance.levelBlocks.Remove(buildBlock);
        }
    }

    public void MonsterPathMarkerPlace(GameObject marker, Vector3 position)
    {
        if (values.hasMarker != true)
        {
            values.hasMarker = true;

            Vector3 offset = new Vector3(position.x, position.y + 3 , position.z);
            GameObject instance = Instantiate(marker, offset, Quaternion.identity);  
            LevelBuildAndPlayManager.Instance.monsterPath.Add(values.gridId);
            LevelBuildAndPlayManager.Instance.monsterPathPos.Add(instance);
            
        }
    }

    public void BuildAreaAssign(GameObject buildAreaMarker)
    {
        if (values.buildArea != true)
        {
            values.buildArea = true;
            // make block block static for navMesh
            //gameObject.isStatic = true;
            LevelBuildAndPlayManager.Instance.buildArea.Add(values.gridId);
        }
        
    }


}
