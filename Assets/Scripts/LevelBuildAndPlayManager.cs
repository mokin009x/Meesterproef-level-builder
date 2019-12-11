using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelBuildAndPlayManager : MonoBehaviour
{
    public static LevelBuildAndPlayManager Instance;
    // lists for saving
    public List<int> levelBLocksIds = new List<int>();
    public List<int> gridIds = new List<int>();
    public List<int> monsterPath = new List<int>();
    public List<int> buildArea = new List<int>();
    //
    public LayerMask currentLayer;

    public enum ControlModes
    {
        BuildingLevel,
        PlayingLevel
    }
    public enum Tools
    {
        None,
        BuildBlockPlace,
        MonsterPathCreate,
        BuildAreaAssign
    }
    
    public enum BuildBlockCategory
    {
        BuildBLocks,
        Decorations,
        Special
    }

    // current level
    public List<GameObject> levelBlocks = new List<GameObject>();
    public List<GameObject> monsterPathPos = new List<GameObject>();
    public List<GameObject> monsterPrefabs = new List<GameObject>();
    
    //tools and prefabs
    public bool cameraRotating = false;
    public Quaternion newCameraRotation;
    public Quaternion oldCameraRotation;
    public float cameraSpeed = 0.5f;
    public GameObject cameraMarker;
    public GameObject monsterPathMarker;
    public GameObject buildAreaMarker;
    public List<GameObject> buildBlocksPrefabs = new List<GameObject>();
    public Tools currentTool;
    public BuildBlockCategory currentCategory;
    public ControlModes currentControlMode;
    public GameObject gridSpaceSelection = null;
    public int currentLayerId = 5;
    public int selectedBuildBlockId;
    
    //play Control mode
    public List<GameObject> defenceTowers;
    public GameObject currentTower;
    
    private void Awake()
    {
        currentLayerId = 5;
        if (Instance != null && Instance != this)
        {   
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        currentCategory = BuildBlockCategory.BuildBLocks;
        currentLayer = SwitchLayer(currentLayerId);
        currentTool = Tools.BuildBlockPlace;
        currentControlMode = ControlModes.BuildingLevel;
    }

    public LayerMask SwitchLayer(int layerId)
    {
        LayerMask newLayer = new LayerMask();
        
        if (layerId == 1)
        {
            newLayer = LayerMask.GetMask("Build layer 1");
        }
        
        if (layerId == 2)
        {
            newLayer = LayerMask.GetMask("Build layer 2");
        }
        
        if (layerId == 3)
        {
            newLayer = LayerMask.GetMask("Build layer 3");
        }
        
        if (layerId == 4)
        {
            newLayer = LayerMask.GetMask("Build layer 4");
        }
        
        if (layerId == 5)
        {
            newLayer = LayerMask.GetMask("Build layer 5");
        }

        return newLayer;
    }

    public void ChangeTool(Tools tool)
    {
        currentTool = tool;
    }

    public void ChangeMode(ControlModes mode)
    {
        currentControlMode = mode;
    }


    // Update is called once per frame
    void Update()
    {
        Controls();
    }

    void Controls()
    {
        if (currentControlMode == ControlModes.BuildingLevel)
        {
            //tool controls and tool debug
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //later check if you are not clicking the ui
                if (currentTool != Tools.None)
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        ToolEffects();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                SelectSpace();
                gridSpaceSelection.GetComponent<GridSpace>().RemoveBlock();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (currentTool == Tools.MonsterPathCreate)
                {
                    RemovePath();
                }
            }

            // camera controls
            if (Input.GetKey(KeyCode.W))
            {
                MoveCamera("Forwards");
            }

            if (Input.GetKey(KeyCode.A))
            {
                MoveCamera("Left");
            }

            if (Input.GetKey(KeyCode.S))
            {
                MoveCamera("Backwards");
            }

            if (Input.GetKey(KeyCode.D))
            {
                MoveCamera("Right");
            }

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                MoveCamera("Left and Forwards");
            }

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                MoveCamera("Right and Forwards");
            }

            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
            {
                MoveCamera("Right and Backwards");
            }

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S)) //camera movement
            {
                MoveCamera("left and Backwards");
            }

            if (Input.GetKeyDown(KeyCode.Q)) // camera rotate
            {
                RotateCamera("Clockwise");
            }

            if (Input.GetKeyDown(KeyCode.E)) // camera rotate
            {
                RotateCamera("Counter Clockwise");
            }

            if (Input.GetKeyDown(KeyCode.M)) // switch mode
            {
                Debug.Log("press m");
                UserInterfaceManager.Instance.mainUi.SetActive(false);
                UserInterfaceManager.Instance.playUi.SetActive(true);
                ChangeMode(ControlModes.PlayingLevel);
                return;
            }


            // save level( to be put in a method for button later)
            if (Input.GetKeyDown(KeyCode.P))
            {
                foreach (var levelBlock in levelBlocks)
                {
                    BuildBlock instance = levelBlock.GetComponent<BuildBlock>();
                    var blockId = instance.buildBlockId;
                    var gridId = instance.pairId;

                    levelBLocksIds.Add(blockId);
                    gridIds.Add(gridId);
                }

                SaveLoad.Save();
            }

            // load level( to be put in a method for button later)

            if (Input.GetKeyDown(KeyCode.L))
            {
                SaveLoad.Load();
            }

            //if level is loaded this will reconstruct the level
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // placing building blocks
                for (int i = 0; i < levelBLocksIds.Count; i++)
                {
                    var blockId = levelBLocksIds[i];
                    var gridId = gridIds[i];
                    var gridSpace = GridManager.Instance.levelGrid[gridId].GetComponent<GridSpace>();
                    gridSpace.PlaceBlock(buildBlocksPrefabs,blockId, gridSpace.currentPos);
                    // still need to give states to the blocks after loading level
                }
                // place monster path
                for (int i = 0; i < monsterPath.Count; i++)
                {
                    var markerPos = monsterPath[i];
                    var gridSpace = GridManager.Instance.levelGrid[markerPos].GetComponent<GridSpace>();
                    
                    gridSpace.MonsterPathMarkerPlace(monsterPathMarker, gridSpace.transform.position);
                }
                // assign build area
                for (int i = 0; i < buildArea.Count; i++)
                {
                    var buildAreaPos = buildArea[i];
                    var gridSpace = GridManager.Instance.levelGrid[buildAreaPos].GetComponent<GridSpace>();

                    gridSpace.values.buildArea = true;
                    Debug.Log(gridSpace.values.buildArea);

                }
                
            }
            //camera and layers

            if (Input.GetKeyDown(KeyCode.Equals))
            {
                if (currentLayerId >= 1 && currentLayerId < 5)
                {
                    currentLayerId = currentLayerId + 1;

                    var newLayer = currentLayerId;
                    currentLayer = SwitchLayer(newLayer);
                    ChangeGridVisibility();
                }
            }

            if (Input.GetKeyDown(KeyCode.Minus))
            {
                if (currentLayerId <= 5 && currentLayerId > 1)
                {
                    currentLayerId = currentLayerId - 1;
                    var newLayer = currentLayerId;
                    currentLayer = SwitchLayer(newLayer);
                    ChangeGridVisibility();
                }
            }
        }


        if (currentControlMode == ControlModes.PlayingLevel)
        {
            // playmode
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    PlaceTower(currentTower);
                }
            }

            // start monster wave test
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(StartMonsterWave());
            }
            
            if (Input.GetKeyDown(KeyCode.M)) // switch mode
            {
                UserInterfaceManager.Instance.playUi.SetActive(false);
                UserInterfaceManager.Instance.mainUi.SetActive(true);
                ChangeMode(ControlModes.BuildingLevel);
                return; // insurance if i write more code under this. 
            }
        }
        
    }

    //playMode methods 
    public void PlaceTower(GameObject tower)
    {
        SelectSpace();
        var space = gridSpaceSelection.GetComponent<GridSpace>().values;
        
        if (space.hasTower != true && space.buildArea == true)
        {
            Vector3 spawnOffset = gridSpaceSelection.transform.position + new Vector3(0,0.5f,0);
            Instantiate(tower, spawnOffset, tower.transform.rotation);
            space.hasTower = true;
        }
    }

    IEnumerator StartMonsterWave()// later needs monster wave in parameters
    {
        foreach (var monster in monsterPrefabs)
        {
            Debug.Log("Monsters will spawn");
            yield return new WaitForSeconds(1);
            Instantiate(monster, monsterPathPos[0].transform.position,monster.transform.rotation);
            yield return new WaitForSeconds(2);
        }
        Debug.Log("WAVE END");
    }
    //

    public void SelectTower(int towerId)
    {
        currentTower = defenceTowers[towerId];
    }

    void ToolEffects()
    {
        if (currentTool == Tools.BuildBlockPlace)
        {
            SelectSpace();
            // place block
            if (currentCategory == BuildBlockCategory.BuildBLocks)
            {
                gridSpaceSelection.GetComponent<GridSpace>().PlaceBlock(buildBlocksPrefabs,selectedBuildBlockId,gridSpaceSelection.transform.position);
            }
            // do the other categorys
            
        }

        if (currentTool == Tools.MonsterPathCreate)
        {
            SelectSpace();
            gridSpaceSelection.GetComponent<GridSpace>().MonsterPathMarkerPlace(monsterPathMarker,gridSpaceSelection.transform.position);
        }

        if (currentTool == Tools.BuildAreaAssign)
        { 
            SelectSpace();
            gridSpaceSelection.GetComponent<GridSpace>().BuildAreaAssign(buildAreaMarker);

        }
    }

    void RemovePath()
    {
        // Still needs to reverse the placed marker values on the spaces.
        monsterPath.RemoveAt(monsterPath.Count - 1);
    }

    void SelectSpace()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity,currentLayer))
        {
            Debug.Log("hit");
            gridSpaceSelection = hit.collider.gameObject;
        }
    }
    

    public void MoveCamera(string direction)
    {
        Vector3 forward = cameraMarker.transform.forward * cameraSpeed; ;
        Vector3 left = -cameraMarker.transform.right * cameraSpeed;;
        Vector3 backwards = -cameraMarker.transform.forward * cameraSpeed;;        
        Vector3 right = cameraMarker.transform.right * cameraSpeed;;

        
        Vector3 moveDirection = new Vector3();
        
        if (direction == "Forwards")
        {
            moveDirection =  forward;
        }
        
        if (direction == "Left")
        {
            moveDirection =  left;
        }
        
        if (direction == "Backwards")
        {
            moveDirection =   backwards;
        }
        
        if (direction == "Right")
        {
            moveDirection =  right;
        }
        
        if (direction == "Left and Forwards")
        {
            moveDirection =  left + forward;
        }
        
        if (direction == "Right and Forwards")
        {
            moveDirection =  right + forward;
        } 
        
        if (direction == "Right and Backwards")
        {
            moveDirection =  right + backwards;
        }
        
        if (direction == "left and Backwards")
        {
            moveDirection =  left + backwards;
        }
        
        
        cameraMarker.GetComponent<Rigidbody>().velocity = moveDirection;
    }

    public void RotateCamera(string direction)
    {
        if (direction == "Clockwise")
        {
            if (cameraRotating == false)
            {
                cameraRotating = true;
                StartCoroutine(RotateCam(Vector3.up * 90, 0.8f));
            }
        }
        
        if (direction == "Counter Clockwise")
        {
            if (cameraRotating == false)
            {
                cameraRotating = true;
                StartCoroutine(RotateCam(Vector3.up * -90, 0.8f));
            }
        }
    }

    public void SaveLevel()
    {
        foreach (var levelBlock in levelBlocks)
        {
            BuildBlock instance = levelBlock.GetComponent<BuildBlock>();
            var blockId = instance.buildBlockId;
            var gridId = instance.pairId;
                
            levelBLocksIds.Add(blockId);
            gridIds.Add(gridId);
        }
        SaveLoad.Save();
    }


    IEnumerator RotateCam(Vector3 byAngles, float inTime)
    {
        var fromAngle = cameraMarker.transform.rotation;
        var toAngle = Quaternion.Euler(cameraMarker.transform.eulerAngles + byAngles);
        for (var t = 0f; t <= 1; t += Time.deltaTime / inTime)
        {
            cameraMarker.transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }

        cameraMarker.transform.rotation = toAngle;
        cameraRotating = false;
    }

    public void ChangeGridVisibility()
    {
        foreach (var space in GridManager.Instance.levelGrid)
        {

            if ((currentLayer & 1 << space.gameObject.layer) != 1 << space.gameObject.layer)
            {
                space.GetComponent<MeshRenderer>().enabled = false;
                space.GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                space.GetComponent<MeshRenderer>().enabled = true;
                space.GetComponent<BoxCollider>().enabled = true;
            }

        }
    }
}

