using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class LevelBuildAndPlayManager : MonoBehaviour
{
    public enum BuildBlockCategory
    {
        BuildBlocks,
        Decorations,
        Special
    }

    public enum ControlModes
    {
        BuildingLevel,
        PlayingLevel
    }

    public enum PlaceDirection
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public enum Tools
    {
        None,
        BuildBlockPlace,
        MonsterPathCreate,
        BuildAreaAssign
    }

    public static LevelBuildAndPlayManager Instance;
    private bool _saturate;
    
    [Header("prefabs and prefab lists")]
    public List<GameObject> buildBlocksPrefabs = new List<GameObject>();
    public List<GameObject> specialBlocksPrefabs = new List<GameObject>();
    public GameObject buildAreaMarker;
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

    //play Control mode
    [Header("play mode with admin controls")]
    public List<GameObject> defenceTowers;
    public GameObject gridSelectionVisual;
    
    // current level
    [Header("Data of level")]
    //build blocks
    public List<int> levelBlocksIds = new List<int>();
    public List<GameObject> levelBlocks = new List<GameObject>();
    public List<float> levelBlockRotationX = new List<float>();
    public List<float> levelBlockRotationY = new List<float>();
    public List<float> levelBlockRotationZ = new List<float>();
    
    //special blocks
    public List<GameObject> specialBlocks = new List<GameObject>();
    public List<int> specialBlockIds = new List<int>();
    public List<float> levelSpecialRotationX = new List<float>();
    public List<float> levelSpecialRotationY = new List<float>();
    public List<float> levelSpecialRotationZ = new List<float>();

    // monster path
    public List<int> levelMonsterPathPosId = new List<int>();

    //build area
    public List<int> buildArea = new List<int>();
    
    [Header("Lists for saving")]//----------------------------------------------------------------//
    
    // build blocks
    public List<int> buildBlockGridIds = new List<int>();
    public List<float> blockRotationX = new List<float>();
    public List<float> blockRotationY = new List<float>();
    public List<float> blockRotationZ = new List<float>();

    //special blocks
   
    public List<int> specialGridId = new List<int>();
    public List<float> specialRotationX = new List<float>();
    public List<float> specialRotationY = new List<float>();
    public List<float> specialRotationZ = new List<float>();
    
    
    //monster path
    public List<GameObject> levelMonsterPathObjList = new List<GameObject>();

    public GameObject monsterPathMarker;
    public List<int> monsterPathPosId = new List<int>(); // position id
    public List<GameObject> monsterPrefabs = new List<GameObject>();
    public List<GameObject> bossWave = new List<GameObject>();
    
    //teleporter exclusive
    public bool teleporterPair;//pairing mode
    
    public GameObject pairTeleporterObj1;//pair object 1
    public GameObject pairTeleporterObj2;//pair object 2
    
    public GameObject selectionGameObj;
    

    [Header("misc")]
    public PlaceDirection visualDirection;
    public Vector3 visualRotation;
    public int selectedBuildBlockId;
    public int selectedSpecialBlockId;
    public float selectionAlpha = 0.75f;
    public float saturateSpeed = 0.01f;
    
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
        currentCategory = BuildBlockCategory.BuildBlocks;
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
        if (teleporterPair)
        {
            pairTeleporterObj1.GetComponent<Teleporter>().Cancel();
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

                //checks and removes special blocks first
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

                visualDirection = (PlaceDirection) directionId;
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

                visualDirection = (PlaceDirection) directionId;
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
                SaveLevel();
            }
            
            if (Input.GetKeyDown(KeyCode.O))
            {
                SaveDemo();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                SaveLoad.LoadDemo();
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
                for (var i = 0; i < levelBlocksIds.Count; i++)
                {
                    var blockId = levelBlocksIds[i];
                    var gridId = buildBlockGridIds[i];
                    var gridSpace = GridManager.Instance.levelGrid[gridId].GetComponent<GridSpace>();
                    gridSpace.PlaceBlock(buildBlocksPrefabs, blockId, gridSpace.currentPos, false);
                    // still need to give states to the blocks after loading level
                }

                // change rotation of build blocks.
                for (int i = 0; i < levelBlocksIds.Count; i++)
                {
                    Vector3 newRotation = new Vector3(blockRotationX[i], blockRotationY[i], blockRotationZ[i]);

                    levelBlocks[i].transform.rotation = Quaternion.Euler(newRotation);
                }

                // placing special blocks
                for (int i = 0; i < specialBlockIds.Count; i++)
                {
                    Debug.Log(specialBlockIds[i] + " blockIds");
                    Debug.Log(specialGridId[i] + " gridIds");

                    var specialId = specialBlockIds[i];
                    var gridId = specialGridId[i];
                    var gridSpace = GridManager.Instance.levelGrid[gridId].GetComponent<GridSpace>();
                    gridSpace.PlaceSpecial(specialBlocksPrefabs, specialId, gridSpace.currentPos, false);
                }
                // special block rotation

                for (int i = 0; i < specialBlockIds.Count; i++)
                {
                    Vector3 newRotation = new Vector3(specialRotationX[i], specialRotationY[i], specialRotationZ[i]);
                    
                    specialBlocks[i].transform.rotation = Quaternion.Euler(newRotation);
                }
                
                
                // place monster path  

                for (var i = 0; i < monsterPathPosId.Count; i++)
                {
                    var markerPos = monsterPathPosId[i];
                    var gridSpaceObj = GridManager.Instance.levelGrid[markerPos];
                    var gridSpace = gridSpaceObj.GetComponent<GridSpace>();


                    gridSpace.MonsterPathMarkerPlace(monsterPathMarker, gridSpace.transform.position);
                }

                // assign build area
                for (var i = 0; i < buildArea.Count; i++)
                {
                    var buildAreaPos = buildArea[i];
                    var gridSpace = GridManager.Instance.levelGrid[buildAreaPos].GetComponent<GridSpace>();
                    var position = gridSpace.transform.position;
                    
                    gridSpace.BuildAreaAssign(buildAreaMarker, position);
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

            if (Input.GetKeyDown(KeyCode.T))
            {
                StartCoroutine(SpawnBoss());
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

            var marker = levelMonsterPathObjList[0].transform;
            var monsterSpawn = new Vector3(marker.transform.position.x, currentLayerId, marker.transform.position.z);
            Instantiate(monster, monsterSpawn, monster.transform.rotation);


            yield return new WaitForSeconds(2);
        }

        Debug.Log("WAVE END");
    }

    private IEnumerator SpawnBoss()
    {
        foreach (var monster in bossWave)
        {
            Debug.Log("Monsters will spawn");
            yield return new WaitForSeconds(1);
            var ray = new Ray();

            var marker = levelMonsterPathObjList[0].transform;
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
            if (currentCategory == BuildBlockCategory.BuildBlocks)
            {
                selectionGameObj.GetComponent<GridSpace>().PlaceBlock(buildBlocksPrefabs, selectedBuildBlockId, selectionGameObj.transform.position, true);
            }

            if (currentCategory == BuildBlockCategory.Decorations)
            {
                // to be implemented
            }

            if (currentCategory == BuildBlockCategory.Special)
            {
                selectionGameObj.GetComponent<GridSpace>().PlaceSpecial(specialBlocksPrefabs, selectedSpecialBlockId, selectionGameObj.transform.position, true);
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
            selectionGameObj.GetComponent<GridSpace>().BuildAreaAssign(buildAreaMarker, selectionGameObj.transform.position);
        }
    }

    private void RemovePath()
    {
        // Still needs to reverse the placed marker values on the spaces.
        monsterPathPosId.RemoveAt(monsterPathPosId.Count - 1);
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
        //build blocks
        levelBlocksIds.Clear();
        buildBlockGridIds.Clear();
        blockRotationX.Clear();
        blockRotationY.Clear();
        blockRotationZ.Clear();

        //special blocks
        specialBlockIds.Clear();
        specialGridId.Clear();
        levelSpecialRotationX.Clear();
        levelSpecialRotationY.Clear();
        levelSpecialRotationZ.Clear();

        // may change 
        monsterPathPosId.Clear();


        foreach (var levelBlock in levelBlocks)
        {
            GridBlock instance = levelBlock.GetComponent<GridBlock>();
            Quaternion rotation = levelBlock.transform.rotation;

            int blockId = instance.buildBlockId;
            int gridId = instance.pairId;

            blockRotationX.Add(rotation.eulerAngles.x);
            blockRotationY.Add(rotation.eulerAngles.y);
            blockRotationZ.Add(rotation.eulerAngles.z);

            levelBlocksIds.Add(blockId);
            buildBlockGridIds.Add(gridId);
        }

        foreach (var specialBlock in specialBlocks)
        {
            var instance = specialBlock.GetComponent<GridBlock>();
            Quaternion rotation = specialBlock.transform.rotation;
            
            var specialId = instance.specialBlockId;
            var gridId = instance.pairId;
            
            specialRotationX.Add(rotation.eulerAngles.x);
            specialRotationY.Add(rotation.eulerAngles.y);
            specialRotationZ.Add(rotation.eulerAngles.z);
            
            specialBlockIds.Add(specialId);
            specialGridId.Add(gridId);
        }

        foreach (var marker in levelMonsterPathPosId)
        {
            monsterPathPosId.Add(marker);
        }


        SaveLoad.Save();
    }

    public void SaveDemo()
    {
        //build blocks
        levelBlocksIds.Clear();
        buildBlockGridIds.Clear();
        blockRotationX.Clear();
        blockRotationY.Clear();
        blockRotationZ.Clear();

        //special blocks
        specialBlockIds.Clear();
        specialGridId.Clear();
        levelSpecialRotationX.Clear();
        levelSpecialRotationY.Clear();
        levelSpecialRotationZ.Clear();

        // may change 
        monsterPathPosId.Clear();


        foreach (var levelBlock in levelBlocks)
        {
            GridBlock instance = levelBlock.GetComponent<GridBlock>();
            Quaternion rotation = levelBlock.transform.rotation;

            int blockId = instance.buildBlockId;
            int gridId = instance.pairId;

            blockRotationX.Add(rotation.eulerAngles.x);
            blockRotationY.Add(rotation.eulerAngles.y);
            blockRotationZ.Add(rotation.eulerAngles.z);

            levelBlocksIds.Add(blockId);
            buildBlockGridIds.Add(gridId);
        }

        foreach (var specialBlock in specialBlocks)
        {
            var instance = specialBlock.GetComponent<GridBlock>();
            Quaternion rotation = specialBlock.transform.rotation;
            
            var specialId = instance.specialBlockId;
            var gridId = instance.pairId;
            
            specialRotationX.Add(rotation.eulerAngles.x);
            specialRotationY.Add(rotation.eulerAngles.y);
            specialRotationZ.Add(rotation.eulerAngles.z);
            
            specialBlockIds.Add(specialId);
            specialGridId.Add(gridId);
        }

        foreach (var marker in levelMonsterPathPosId)
        {
            monsterPathPosId.Add(marker);
        }


        SaveLoad.SaveDemo();
    }

    public void EnterPlayMode() // button
    {
        UserInterfaceManager.Instance.mainUi.SetActive(false);
        UserInterfaceManager.Instance.playUi.SetActive(true);
        ChangeMode(ControlModes.PlayingLevel);
    }

    public void EnterBuildMode()// button
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