using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    public GameObject cameraUiButtonsToggle;
    // Categories
    public GameObject buildBlocksToggle;
    public GameObject decorationBlocksToggle;
    public GameObject specialBlocksToggle;
    // Start is called before the first frame update
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
            LevelBuilderManager.Instance.ChangeTool(LevelBuilderManager.Tools.BuildBlockPlace);
            Debug.Log("test");
        }
        
        if (buttonId == 1)//Build area assign tool
        {
            LevelBuilderManager.Instance.ChangeTool(LevelBuilderManager.Tools.BuildAreaAssign);
            Debug.Log("test");
        }
        
        if (buttonId == 2)//Monster path create tool
        {
            LevelBuilderManager.Instance.ChangeTool(LevelBuilderManager.Tools.MonsterPathCreate);
            Debug.Log("test");
        }
        
        if (buttonId == 3)//Save button 
        {
            LevelBuilderManager.Instance.SaveLevel();
        }

        if (buttonId == 4)//Rotate Cam Clockwise
        {
            LevelBuilderManager.Instance.RotateCamera("Clockwise");
        }

        if (buttonId == 5)// Rotate Cam CounterClockwise
        {
            LevelBuilderManager.Instance.RotateCamera("Counter Clockwise");
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

    public void CatalogueReset()
    {
        buildBlocksToggle.SetActive(false);
        decorationBlocksToggle.SetActive(false);
        specialBlocksToggle.SetActive(false);
    }
}