using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class LevelBuildAndPlayManager : MonoBehaviour
{
    public enum BuildBlockCategory
    {
        BuildBLocks,
        Decorations,
        Special
    }

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
    
    public enum PlaceDirection
    {
       North = 0, 
       East = 1,
       South = 2,
       West = 3
    }

    public static LevelBuildAndPlayManager Instance;
    private bool _saturate;
    public List<int> buildArea = new List<int>();
    public GameObject buildAreaMarker;
    public List<GameObject> buildBlocksPrefabs = new List<GameObject>();
    public List<GameObject> specialBlocksPrefabs = new List<GameObject>();
    public GameObject cameraMarker;

    //tools and prefabs
    [Header("Tools Related")] 
    public bool cameraRotating;

    public float cameraSpeed = 0.5f;
    public BuildBlockCategory currentCategory;

    public ControlModes currentControlMode;

    //
    public LayerMask currentLayer;
    public int currentLayerId = 1;
    public Tools currentTool;
    public GameObject currentTower;
    public bool teleporterPair = false;
    public GameObject pairTeleporterObj;
    public PlaceDirection visualDirection;
    public Vector3 visualRotation;

    //play Control mode
    [Header("play mode with admin controls")]
    public List<GameObject> defenceTowers;

    public List<int> blockGridIds = new List<int>();

    public GameObject gridSelectionVisual;

    // current level
    [Header("Data of level")] 
    public List<GameObject> levelBlocks = new List<GameObject>();
    public List<GameObject> specialBlocks = new List<GameObject>();

    // lists for saving
    public List<int> levelBLocksIds = new List<int>();
    //public List<Vector3> levelBlockRotation = new List<Vector3>();
    public List<float> blockRotationX;
    public List<float> blockRotationY;
    public List<float> blockRotationZ;

    public List<float> specialRotationX;
    public List<float> specialRotationY;
    public List<float> specialRotationZ;
    
    public List<int> monsterPath = new List<int>();
    public GameObject monsterPathMarker;
    public List<GameObject> monsterPathPos = new List<GameObject>();
    public List<GameObject> monsterPrefabs = new List<GameObject>();
    //public Quaternion newCameraRotation;
    //public Quaternion oldCameraRotation;
    public float saturateSpeed = 0.01f;
    public int selectedBuildBlockId;
    public int selectedSpecialBlockId;
    public float selectionAlpha = 0.75f;
    public GameObject selectionGameObj;

    public GameObject TestCube;
    public List<NavMeshSurface> walkableSurfaces = new List<NavMeshSurface>();

    private void Awake()
    {
        visualDirection = PlaceDirection.South;
        currentLayerId = 1;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        selectedBuildBlockId = 0; // default it to grass block
        currentCategory = BuildBlockCategory.BuildBLocks;
        currentLayer = SwitchLayer(currentLayerId);
        currentTool = Tools.BuildBlockPlace;
        currentControlMode = ControlModes.BuildingLevel;
    }

    public void ColorUpdate()
    {
        if (_saturate == false)
        {
            if (selectionAlpha > 0f)
            {
                selectionAlpha = selectionAlpha - saturateSpeed;
                var test = gridSelectionVisual.GetComponent<Renderer>().material.color;
                var newColor = new Color(test.r, test.g, test.b, selectionAlpha);

                gridSelectionVisual.GetComponent<Renderer>().material.color = newColor;
                return;
            }

            _saturate = true;
        }

        if (_saturate)
        {
            if (selectionAlpha < 0.75f)
            {
                selectionAlpha = selectionAlpha + saturateSpeed;
                var test = gridSelectionVisual.GetComponent<Renderer>().material.color;
                var newColor = new Color(test.r, test.g, test.b, selectionAlpha);

                gridSelectionVisual.GetComponent<Renderer>().material.color = newColor;
                return;
            }

            _saturate = false;
        }
    }

    public LayerMask SwitchLayer(int layerId)
    {
        var newLayer = new LayerMask();

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
        if (teleporterPair == true)
        {
            pairTeleporterObj.GetComponent<Teleporter>().Cancel();
        }
    }

    public void ChangeMode(ControlModes mode)
    {
        currentControlMode = mode;
    }


    // Update is called once per frame
    private void Update()
    {
        Controls();
        GridVisualPositionUpdate();
    }

    private void GridVisualPositionUpdate()
    {
        SelectSpace();
        ColorUpdate();


        if (selectionGameObj != null)
        {
            gridSelectionVisual.transform.position = selectionGameObj.transform.position;
            gridSelectionVisual.transform.eulerAngles = visualRotation;
        }
    }
    public void GridVisualRotationUpdate()
    {
        if (visualDirection == PlaceDirection.North)
        {
            visualRotation = new Vector3(0, 180, 0);
        }

        if (visualDirection == PlaceDirection.East)
        {
            visualRotation = new Vector3(0, 270, 0);
        }

        if (visualDirection == PlaceDirection.South)
        {
            visualRotation = new Vector3(0, 0, 0);
        }

        if (visualDirection == PlaceDirection.West)
        {
            visualRotation = new Vector3(0, 90, 0);
        }
        
    }

    private void Controls()
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
                selectionGameObj.GetComponent<GridSpace>().RemoveBlock();
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

            if (Input.GetKeyDown(KeyCode.Period))
            {
                int directionId = (int) visualDirection;
                if (directionId != 0)
                {
                    directionId = directionId - 1;
                }
                else
                {
                    directionId = 3;
                }
                visualDirection = (PlaceDirection)directionId;
                GridVisualRotationUpdate();
            }

            if (Input.GetKeyDown(KeyCode.Comma))
            {
                int directionId = (int) visualDirection;
                if (directionId != 3)
                {
                    directionId = directionId + 1;
                }
                else
                {
                    directionId = 0;
                }
                
                visualDirection = (PlaceDirection)directionId;
                GridVisualRotationUpdate();
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
                    var instance = levelBlock.GetComponent<BuildBlock>();
                    var blockId = instance.buildBlockId;
                    var gridId = instance.pairId;

                    levelBLocksIds.Add(blockId);
                    blockGridIds.Add(gridId);
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
                for (var i = 0; i < levelBLocksIds.Count; i++)
                {
                    var blockId = levelBLocksIds[i];
                    var gridId = blockGridIds[i];
                    var gridSpace = GridManager.Instance.levelGrid[gridId].GetComponent<GridSpace>();
                    gridSpace.PlaceBlock(buildBlocksPrefabs, blockId, gridSpace.currentPos);
                    // still need to give states to the blocks after loading level
                }

                // change rotation of blocks.
                for (int i = 0; i < levelBLocksIds.Count -1; i++)
                {
                    
                    Vector3 newRotation = new Vector3(blockRotationX[i],blockRotationY[i],blockRotationZ[i]);
                    
                    levelBlocks[i].transform.rotation = Quaternion.Euler(newRotation);
                }

                // place monster path
                for (var i = 0; i < monsterPath.Count; i++)
                {
                    var markerPos = monsterPath[i];
                    var gridSpaceObj = GridManager.Instance.levelGrid[markerPos];
                    var gridSpace = gridSpaceObj.GetComponent<GridSpace>();


                    gridSpace.MonsterPathMarkerPlace(monsterPathMarker, gridSpace.transform.position);
                }

                // assign build area
                for (var i = 0; i < buildArea.Count; i++)
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
            }
        }
    }

    //playMode methods 
    public void PlaceTower(GameObject tower)
    {
        SelectSpace();
        var space = selectionGameObj.GetComponent<GridSpace>().values;

        if (space.hasTower != true && space.buildArea)
        {
            var spawnTransform = selectionGameObj.transform;
            var spawnOffset = spawnTransform.position + new Vector3(0, 0.5f, 0);
            Instantiate(tower, spawnOffset, Quaternion.Euler(visualRotation));
            space.hasTower = true;
        }
    }

    private IEnumerator StartMonsterWave() // later needs monster wave in parameters
    {
        /*foreach (var marker in monsterPathPos)
        {
            marker.GetComponent<MeshRenderer>().enabled = false;
        }*/
        foreach (var monster in monsterPrefabs)
        {
            Debug.Log("Monsters will spawn");
            yield return new WaitForSeconds(1);
            var ray = new Ray();

            var marker = monsterPathPos[0].transform;
            var monsterSpawn = new Vector3(marker.transform.position.x, currentLayerId, marker.transform.position.z);
            Instantiate(monster, monsterSpawn, monster.transform.rotation);


            yield return new WaitForSeconds(2);
        }

        Debug.Log("WAVE END");
    }
    //

    /*public void SelectTower(int towerId)
    {
        currentTower = defenceTowers[towerId];
    }*/

    private void ToolEffects()
    {
        if (currentTool == Tools.BuildBlockPlace)
        {
            SelectSpace();
            // place block
            if (currentCategory == BuildBlockCategory.BuildBLocks)
            {
                
                selectionGameObj.GetComponent<GridSpace>().PlaceBlock(buildBlocksPrefabs, selectedBuildBlockId, selectionGameObj.transform.position);
            }

            if (currentCategory == BuildBlockCategory.Decorations)
            {
                // to be implemented
            }

            if (currentCategory == BuildBlockCategory.Special)
            {
                selectionGameObj.GetComponent<GridSpace>().PlaceSpecial(specialBlocks,selectedSpecialBlockId,selectionGameObj.transform.position);
            }

            // do the other category
        }

        if (currentTool == Tools.MonsterPathCreate)
        {
            SelectSpace();
            selectionGameObj.GetComponent<GridSpace>().MonsterPathMarkerPlace(monsterPathMarker, selectionGameObj.transform.position);
        }

        if (currentTool == Tools.BuildAreaAssign)
        {
            SelectSpace();
            selectionGameObj.GetComponent<GridSpace>().BuildAreaAssign(buildAreaMarker);
        }
    }

    private void RemovePath()
    {
        // Still needs to reverse the placed marker values on the spaces.
        monsterPath.RemoveAt(monsterPath.Count - 1);
    }

    private void SelectSpace()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, currentLayer))
        {
            // Debug.Log("hit");

            selectionGameObj = hit.collider.gameObject;
        }
    }


    public void MoveCamera(string direction)
    {
        var forward = cameraMarker.transform.forward * cameraSpeed;
        var left = -cameraMarker.transform.right * cameraSpeed;
        var backwards = -cameraMarker.transform.forward * cameraSpeed;
        var right = cameraMarker.transform.right * cameraSpeed;


        var moveDirection = new Vector3();

        if (direction == "Forwards")
        {
            moveDirection = forward;
        }

        if (direction == "Left")
        {
            moveDirection = left;
        }

        if (direction == "Backwards")
        {
            moveDirection = backwards;
        }

        if (direction == "Right")
        {
            moveDirection = right;
        }

        if (direction == "Left and Forwards")
        {
            moveDirection = left + forward;
        }

        if (direction == "Right and Forwards")
        {
            moveDirection = right + forward;
        }

        if (direction == "Right and Backwards")
        {
            moveDirection = right + backwards;
        }

        if (direction == "left and Backwards")
        {
            moveDirection = left + backwards;
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
            var instance = levelBlock.GetComponent<BuildBlock>();
            var blockId = instance.buildBlockId;
            var gridId = instance.pairId;

            levelBLocksIds.Add(blockId);
            blockGridIds.Add(gridId);
        }

        SaveLoad.Save();
    }

    public void EnterPlayMode() // button
    {
        UserInterfaceManager.Instance.mainUi.SetActive(false);
        UserInterfaceManager.Instance.playUi.SetActive(true);
        ChangeMode(ControlModes.PlayingLevel);
    }

    public void EnterBuildMode()
    {
        UserInterfaceManager.Instance.mainUi.SetActive(true);
        UserInterfaceManager.Instance.playUi.SetActive(false);
        ChangeMode(ControlModes.BuildingLevel);
    }


    private IEnumerator RotateCam(Vector3 byAngles, float inTime)
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
            if ((currentLayer & (1 << space.gameObject.layer)) != 1 << space.gameObject.layer)
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