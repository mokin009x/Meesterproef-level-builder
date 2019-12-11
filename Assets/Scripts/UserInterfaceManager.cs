using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    public static UserInterfaceManager Instance;
    public GameObject mainUi;
    public GameObject playUi;
    public GameObject cameraUiButtonsToggle;
    // Categories
    public GameObject buildBlocksToggle;
    public GameObject decorationBlocksToggle;
    public GameObject specialBlocksToggle;
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
        CatalogueReset();
        buildBlocksToggle.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonMethods(int buttonId)
    {
        //Tool selection
        
        if (buttonId == 0)//Build block place tool
        {
            LevelBuildAndPlayManager.Instance.ChangeTool(LevelBuildAndPlayManager.Tools.BuildBlockPlace);
            Debug.Log("test");
        }
        
        if (buttonId == 1)//Build area assign tool
        {
            LevelBuildAndPlayManager.Instance.ChangeTool(LevelBuildAndPlayManager.Tools.BuildAreaAssign);
            Debug.Log("test");
        }
        
        if (buttonId == 2)//Monster path create tool
        {
            LevelBuildAndPlayManager.Instance.ChangeTool(LevelBuildAndPlayManager.Tools.MonsterPathCreate);
            Debug.Log("test");
        }
        
        if (buttonId == 3)//Save button 
        {
            LevelBuildAndPlayManager.Instance.SaveLevel();
        }

        if (buttonId == 4)//Rotate Cam Clockwise
        {
            LevelBuildAndPlayManager.Instance.RotateCamera("Clockwise");
        }

        if (buttonId == 5)// Rotate Cam CounterClockwise
        {
            LevelBuildAndPlayManager.Instance.RotateCamera("Counter Clockwise");
        }

        if (buttonId == 6)//Camera Buttons Toggle
        {
            cameraUiButtonsToggle.SetActive(!cameraUiButtonsToggle.activeSelf);
        }
        
        //Categories
        if (buttonId == 7)//switch to buildBlocks
        {
            CatalogueReset();
            buildBlocksToggle.SetActive(true);
        }

        if (buttonId == 8)//switch to decorationBlocks
        {
            CatalogueReset();
            decorationBlocksToggle.SetActive(true);
        }

        if (buttonId == 9)//Switch to specialBlocks
        {
            CatalogueReset();
            specialBlocksToggle.SetActive(true);
        }
    }

    public void SelectBuildBlock(int prefabId)
    {
        LevelBuildAndPlayManager.Instance.selectedBuildBlockId = prefabId;
        //LevelBuildAndPlayManager.Instance.selectedBuildBlock = LevelBuildAndPlayManager.Instance.buildBlocksPrefabs[prefabId];
    }

    public void CatalogueReset()
    {
        buildBlocksToggle.SetActive(false);
        decorationBlocksToggle.SetActive(false);
        specialBlocksToggle.SetActive(false);
    }
}