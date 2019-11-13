using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilderManager : MonoBehaviour
{
    public GridManager grdManager;
    public enum Tools
    {
        None,
        BuildBlockPlace,
        MonsterPathCreate,
        BuildAreaAssign
    }

    public List<GameObject> levelBlocks;
    
    public List<GameObject> buildBlocksPrefabs;
    
    public Tools tool;
    public GameObject gridSpaceSelection = null;
    public GameObject selectedBuildBlockId;
    
    // Start is called before the first frame update
    void Start()
    {
        
        tool = Tools.BuildBlockPlace;
        grdManager = GameObject.Find("Managers").GetComponent<GridManager>();
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
            if (tool != Tools.None)
            {
                ToolEffects();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveLoad.Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveLoad.Load();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < levelBlocks.Count; i++)
            {
                Instantiate(levelBlocks[i]);
            }
        }
    }

    void ToolEffects()
    {
        if (tool == Tools.BuildBlockPlace)
        {
            SelectSpace();
            // place block
            gridSpaceSelection.GetComponent<GridSpace>().PlaceBlock(0);
        }
    }

    void SelectSpace()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log("hit");
            gridSpaceSelection = hit.collider.gameObject;
        }
    }

}

