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

    public void PlaceBlock(List<GameObject> prefabList, int buildBlockId, Vector3 position, bool rotation) //prefablist is always buildblock prefabs.
    {
        if (values.hasBuildBlock != true)
        {
            values.hasBuildBlock = true;

            GameObject blockPrefab = prefabList[buildBlockId];
            buildBlock = Instantiate(blockPrefab, position, Quaternion.Euler(LevelBuildAndPlayManager.Instance.visualRotation));

            GridBlock gridBlockClass = buildBlock.GetComponent<GridBlock>();
            gridBlockClass.gridSpacePair = GridManager.Instance.levelGrid[values.gridId].GetComponent<GridSpace>();
            buildBlock.GetComponent<GridBlock>().pairId = values.gridId;
            buildBlock.GetComponent<GridBlock>().buildBlockId = buildBlockId;
            LevelBuildAndPlayManager.Instance.levelBlocks.Add(buildBlock);

            var blockRotation = buildBlock.transform.rotation;

            if (rotation == true)
            {
                LevelBuildAndPlayManager.Instance.levelBlockRotationX.Add(blockRotation.eulerAngles.x);
                LevelBuildAndPlayManager.Instance.levelBlockRotationY.Add(blockRotation.eulerAngles.y);
                LevelBuildAndPlayManager.Instance.levelBlockRotationZ.Add(blockRotation.eulerAngles.z);
            }
            
            buildBlock.GetComponent<NavMeshModifier>().area = 1;
            buildBlock.GetComponent<NavMeshModifier>().overrideArea = true;
            if (buildBlockId == 1) //soil block that is walkable
            {
                buildBlock.GetComponent<NavMeshModifier>().area = 0;
                buildBlock.GetComponent<NavMeshModifier>().overrideArea = true;
            }
        }
    }

    public void PlaceSpecial(List<GameObject> prefabList, int specialBlockId, Vector3 position, bool rotation)
    {
        if (values.hasSpecial != true)
        {
            values.hasSpecial = true;

            GameObject specialPrefab = prefabList[specialBlockId];

            specialBlock = Instantiate(specialPrefab, position, Quaternion.Euler(LevelBuildAndPlayManager.Instance.visualRotation));

            if (specialBlockId == 0)// teleporter
            {
                if (LevelBuildAndPlayManager.Instance.teleporterPair == false)
                {
                    // first teleporter
                    Debug.Log("first tp");
                    specialBlock.GetComponent<Teleporter>().AssignPairStage1();
                }
                else if (LevelBuildAndPlayManager.Instance.teleporterPair == true)
                {
                    Debug.Log("second tp");
                    specialBlock.GetComponent<Teleporter>().AssignPairStage2();
                }
            }
            
           
            GridBlock gridBlockClass = specialBlock.GetComponent<GridBlock>();
            gridBlockClass.gridSpacePair = GridManager.Instance.levelGrid[values.gridId].GetComponent<GridSpace>();
            specialBlock.GetComponent<GridBlock>().pairId = values.gridId;
            specialBlock.GetComponent<GridBlock>().specialBlockId = specialBlockId;
            

            Debug.Log("should instantiate portal");
            LevelBuildAndPlayManager.Instance.specialBlocks.Add(specialBlock);

            var specialBlockRotation = specialBlock.transform.rotation;

            if (rotation == true)
            {
                LevelBuildAndPlayManager.Instance.levelSpecialRotationX.Add(specialBlockRotation.eulerAngles.x);
                LevelBuildAndPlayManager.Instance.levelSpecialRotationY.Add(specialBlockRotation.eulerAngles.y);
                LevelBuildAndPlayManager.Instance.levelSpecialRotationZ.Add(specialBlockRotation.eulerAngles.z);  
            }
            
            // special block bla
            specialBlock.GetComponent<NavMeshModifier>().area = 0;
            specialBlock.GetComponent<NavMeshModifier>().overrideArea = true;
        }
    }

    public bool RemoveSpecialBlockCheck()
    {
        if (values.hasSpecial)
        {
            return true;
        }

        return false;
    }

    public void RemoveBlock()
    {
        if (RemoveSpecialBlockCheck())//special block removal
        {
            Destroy(specialBlock);
            LevelBuildAndPlayManager.Instance.specialBlocks.Remove(specialBlock);
            values.hasSpecial = false;
            return;
        }

        if (values.hasBuildBlock)
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

            Vector3 offset = new Vector3(position.x, position.y + 3, position.z);
            GameObject instance = Instantiate(marker, offset, Quaternion.identity);
            LevelBuildAndPlayManager.Instance.levelMonsterPathPosId.Add(values.gridId);
            LevelBuildAndPlayManager.Instance.levelMonsterPathObjList.Add(instance);
        }
    }

    public void BuildAreaAssign(GameObject buildAreaMarker, Vector3 position)
    {
        if (values.buildArea != true)
        {
            values.buildArea = true;
            Instantiate(buildAreaMarker, position, Quaternion.identity);
            
            LevelBuildAndPlayManager.Instance.buildArea.Add(values.gridId);
        }
    }
}