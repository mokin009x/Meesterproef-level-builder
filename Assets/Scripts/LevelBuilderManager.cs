using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBuilderManager : MonoBehaviour
{
    public static LevelBuilderManager Instance;
    // lists for saving
    public List<int> levelBLocksIds = new List<int>();
    public List<int> gridIds = new List<int>();
    

    public List<int> monsterPath = new List<int>();
    //
    public LayerMask currentLayer;

    public enum Tools
    {
        None,
        BuildBlockPlace,
        MonsterPathCreate,
        BuildAreaAssign
    }

    // current level
    public List<GameObject> levelBlocks = new List<GameObject>();
    
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
    public GameObject gridSpaceSelection = null;
    public int currentLayerId = 5;

    // Start is called before the first frame update
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
        currentLayer = SwitchLayer(currentLayerId);
        currentTool = Tools.BuildBlockPlace;
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



    // Update is called once per frame
    void Update()
    {
        Controls();
    }

    void Controls()
    {
        //tool controls and tool debug
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //later check if you are not clicking the ui
            if (currentTool != Tools.None)
            {
                ToolEffects();
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
        
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
        {
            MoveCamera("left and Backwards");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateCamera("Clockwise");
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            RotateCamera("Counter Clockwise");

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

            for (int i = 0; i < levelBLocksIds.Count; i++)
            {
                var blockId = levelBLocksIds[i];
                var gridId = gridIds[i];
                var gridSpace = GridManager.Instance.levelGrid[gridId].GetComponent<GridSpace>();
                gridSpace.PlaceBlock(blockId, gridSpace.currentPos);
                // still need to give states to the blocks after loading level
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
            }
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            if (currentLayerId <= 5 && currentLayerId > 1)
            {
                currentLayerId = currentLayerId - 1;
                var newLayer = currentLayerId;
                currentLayer = SwitchLayer(newLayer);
            }
        }
    }

    void ToolEffects()
    {
        if (currentTool == Tools.BuildBlockPlace)
        {
            SelectSpace();
            // place block
            gridSpaceSelection.GetComponent<GridSpace>().PlaceBlock(0, gridSpaceSelection.transform.position);
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
        
        
        Debug.Log("end of move camera");
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

    IEnumerator RotateCam(Vector3 byAngles, float inTime) 
    {    var fromAngle = cameraMarker.transform.rotation;
        var toAngle = Quaternion.Euler(cameraMarker.transform.eulerAngles + byAngles);
        for(var t = 0f; t <= 1; t += Time.deltaTime/inTime) {
            cameraMarker.transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
        cameraMarker.transform.rotation = toAngle;
        cameraRotating = false;
    }
}

