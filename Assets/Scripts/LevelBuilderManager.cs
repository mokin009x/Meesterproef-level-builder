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
    public List<int> levelBlocksXAxis = new List<int>();
    public List<int> levelBlocksYAxis = new List<int>(); 
    public List<int> levelBlocksZAxis = new List<int>();

    public List<int> monsterPath = new List<int>();
    
    public LayerMask currentLayer;

    public enum Tools
    {
        None,
        BuildBlockPlace,
        MonsterPathCreate,
        BuildAreaAssign
    }

    public List<GameObject> levelBlocks = new List<GameObject>();
    public GameObject monsterPathMarker;
    public List<GameObject> buildBlocksPrefabs = new List<GameObject>();
    
    public Tools currentTool;
    public GameObject gridSpaceSelection = null;
    //public GameObject selectedBuildBlockId;
    
    // Start is called before the first frame update
    private void Awake()
    {
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
        currentLayer = SwitchLayer(5);
        currentTool = Tools.BuildBlockPlace;
    }

    public LayerMask SwitchLayer(int layerId)
    {
        LayerMask newLayer = new LayerMask();
        if (layerId == 1)
        {
            
        }
        
        if (layerId == 2)
        {
            
        }
        
        if (layerId == 3)
        {
            
        }
        
        if (layerId == 4)
        {
            
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //later check if you are not clicking the ui
            if (currentTool != Tools.None)
            {
                ToolEffects();
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (currentTool == Tools.MonsterPathCreate)
            {
                RemovePath();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (var levelBlock in levelBlocks)
            {
                BuildBlock instance = levelBlock.GetComponent<BuildBlock>();
                var blockId = instance.buildBlockId;
                var gridId = instance.pairId;
                var pos = instance.transform.position;
                var xAxis = pos.x;
                var yAxis = pos.y;
                var zAxis = pos.z;
                
                levelBlocksXAxis.Add(Mathf.FloorToInt(xAxis));
                levelBlocksYAxis.Add(Mathf.FloorToInt(yAxis));
                levelBlocksZAxis.Add(Mathf.FloorToInt(zAxis));
                levelBLocksIds.Add(blockId);
                gridIds.Add(gridId);
            }
            SaveLoad.Save();
        }

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

}

