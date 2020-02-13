using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GridSpace : MonoBehaviour
{
    public GameObject buildBlock;
    public GameObject specialBlock;
    public Vector3 currentPos;

    public GridSpaceValues values;

    // Start is called before the first frame update
    private void Start()
    {
        currentPos = transform.position;
    }

    public void PlaceBlock(List<GameObject> prefabList, int buildBlockId, Vector3 position) //prefablist is always buildblock prefabs.
    {
        if (values.hasBuildBlock != true)
        {
            values.hasBuildBlock = true;

            GameObject blockPrefab = prefabList[buildBlockId];
            buildBlock = Instantiate(blockPrefab, position, Quaternion.Euler(LevelBuildAndPlayManager.Instance.visualRotation));

            BuildBlock buildBlockClass = buildBlock.GetComponent<BuildBlock>();
            buildBlockClass.gridSpacePair = GridManager.Instance.levelGrid[values.gridId].GetComponent<GridSpace>();
            buildBlock.GetComponent<BuildBlock>().pairId = values.gridId;
            buildBlock.GetComponent<BuildBlock>().buildBlockId = buildBlockId;
            LevelBuildAndPlayManager.Instance.levelBlocks.Add(buildBlock);

            var blockRotation = buildBlock.transform.rotation;
            
            LevelBuildAndPlayManager.Instance.blockRotationX.Add(blockRotation.eulerAngles.x);
            LevelBuildAndPlayManager.Instance.blockRotationY.Add(blockRotation.eulerAngles.y);
            LevelBuildAndPlayManager.Instance.blockRotationZ.Add(blockRotation.eulerAngles.z);
            
            buildBlock.GetComponent<NavMeshModifier>().area = 1;
            buildBlock.GetComponent<NavMeshModifier>().overrideArea = true;
            if (buildBlockId == 1) //soil block that is walkable
            {
                buildBlock.GetComponent<NavMeshModifier>().area = 0;
                buildBlock.GetComponent<NavMeshModifier>().overrideArea = true;
            }
        }
    }

    public void PlaceSpecial(List<GameObject> prefabList, int specialBlockId, Vector3 position)
    {
        if (values.hasSpecial != true)
        {
            values.hasSpecial = true;

            GameObject specialPrefab = prefabList[specialBlockId];
            Vector3 offSetAmount = new Vector3(0,1,0);
            Vector3 offSetPosition = position + offSetAmount;
            specialBlock = Instantiate(specialPrefab, offSetPosition, Quaternion.Euler(LevelBuildAndPlayManager.Instance.visualRotation));
            
            LevelBuildAndPlayManager.Instance.specialBlocks.Add(specialBlock);

            var specialBlockRotation = specialBlock.transform.rotation;
                
            LevelBuildAndPlayManager.Instance.specialRotationX.Add(specialBlockRotation.eulerAngles.x);
            LevelBuildAndPlayManager.Instance.specialRotationY.Add(specialBlockRotation.eulerAngles.y);
            LevelBuildAndPlayManager.Instance.specialRotationZ.Add(specialBlockRotation.eulerAngles.z);
            // special block bla
            buildBlock.GetComponent<NavMeshModifier>().area = 0;
            buildBlock.GetComponent<NavMeshModifier>().overrideArea = true;
        }
    }

    public void RemoveSpecialBlock()
    {
        if (values.hasSpecial)
        {
            Destroy(specialBlock);
        }
    }

    public void RemoveBlock()
    {
        if (values.hasBuildBlock)
        {
            Destroy(buildBlock);
            values.hasBuildBlock = false;
            LevelBuildAndPlayManager.Instance.levelBlocks.Remove(buildBlock);
        }
    }

    public void RemoveSpecial()
    {
        if (values.hasSpecial)
        {
            Destroy(specialBlock);
            values.hasSpecial = false;
            LevelBuildAndPlayManager.Instance.specialBlocks.Remove(specialBlock);
        }
        // remove special here
    }

    public void MonsterPathMarkerPlace(GameObject marker, Vector3 position)
    {
        if (values.hasMarker != true)
        {
            values.hasMarker = true;

            Vector3 offset = new Vector3(position.x, position.y + 3, position.z);
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