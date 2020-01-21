using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    
    
    
    public static UserInterfaceManager Instance;
    [Header("main ui elements")]
    public GameObject mainUi;
    public GameObject playUi;
    public TextMeshProUGUI selectionText;
    
    [Header("pop up ui elements")]
    public GameObject cameraUiButtonsToggle;
    
    // Categories
    [Header("category toggle buttons")]
    public GameObject buildBlocksToggle;
    public GameObject decorationBlocksToggle;
    public GameObject specialBlocksToggle;

    [Header("category toggle button lists")]

    public List<GameObject> buildBlockButtons = new List<GameObject>();
    public List<GameObject> decorationBlockButtons = new List<GameObject>();
    public List<GameObject> specialBlockButtons = new List<GameObject>();

    [Header("name identifier lists")] // this is stupid and i know their are better ways.
    public List<string> categoryNames = new List<string>();
    public List<string> buildBLockNames = new List<string>();
    
    [Header("misc")] 
    public Color selectionColor;
    public Color notSelectedColor;


    public GameObject selectionBackground;
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
        DefaultInterfaceState();
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
        int selectedBlockId = prefabId;

        selectionBackground.GetComponent<Image>().color = notSelectedColor;
        LevelBuildAndPlayManager.Instance.selectedBuildBlockId = selectedBlockId;
        selectionBackground = buildBlockButtons[selectedBlockId];
        selectionBackground.GetComponent<Image>().color = selectionColor;
        UpdateSelectionText();


        
        //LevelBuildAndPlayManager.Instance.selectedBuildBlock = LevelBuildAndPlayManager.Instance.buildBlocksPrefabs[prefabId];
    }

    public void CatalogueReset()
    {
        buildBlocksToggle.SetActive(false);
        decorationBlocksToggle.SetActive(false);
        specialBlocksToggle.SetActive(false);
    }

    public void UpdateSelectionText()
    {
        LevelBuildAndPlayManager.BuildBlockCategory category;
        category = LevelBuildAndPlayManager.Instance.currentCategory;
        string blockName = buildBLockNames[LevelBuildAndPlayManager.Instance.selectedBuildBlockId];
        string categoryName = "no category";
        if (category == LevelBuildAndPlayManager.BuildBlockCategory.BuildBLocks)
        {
            categoryName = categoryNames[0];
        }

        if (category == LevelBuildAndPlayManager.BuildBlockCategory.Decorations)
        {
            categoryName = categoryNames[1];
        }

        if (category == LevelBuildAndPlayManager.BuildBlockCategory.Special)
        {
            categoryName = categoryNames[2];
        }
        
        selectionText.text = categoryName + blockName;
    }

    public void DefaultInterfaceState()
    {
        //default is grass block in build blocks if this is not the case check lists
        int selectedBlockId = LevelBuildAndPlayManager.Instance.selectedBuildBlockId;
        selectionBackground = buildBlockButtons[selectedBlockId];
        selectionBackground.GetComponent<Image>().color = selectionColor;
        //selectedBlock



    }
}