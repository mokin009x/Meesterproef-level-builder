using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    public static UserInterfaceManager Instance;


    // Categories
    [Header("category toggle buttons")]
    public GameObject buildBlocksToggle;
    public GameObject specialBlocksToggle;
    public GameObject decorationBlocksToggle;

    [Header("button handles")] 
    public GameObject buildBlockHandle;
    public GameObject specialBlockHandle;
    public GameObject decorationBlockHandle;
    
    [Header("CategoryUiElements")]
    public List<GameObject> specialBlockButtons = new List<GameObject>();
    public List<GameObject> buildBlockButtons = new List<GameObject>();
    public List<GameObject> decorationBlockButtons = new List<GameObject>();

    [Header("pop up ui elements")] 
    public GameObject cameraUiButtonsToggle;

    [Header("name identifier lists")] // this is stupid and i know their are better ways.
    public List<string> categoryNames = new List<string>();

    public List<string> buildBLockNames = new List<string>();


    [Header("main ui elements")]
    public GameObject mainUi;
    public GameObject mainCategorySelectionBackground;

    public Color notSelectedColor;
    public GameObject playUi;

    [Header("sub ui elements")]
    public GameObject subCategorySelectionBackground;

    [Header("tool ui elements")] 
    public GameObject buildToolHolder;
    public GameObject assignToolHolder;
    public GameObject pathToolHolder;
    public GameObject toolSelectionBackground;

    [Header("misc")]
    public Color selectionColor;

    public TextMeshProUGUI selectionText;
    public GameObject dummyCategory; // fallback if null
    public LevelBuildAndPlayManager buildManager;

    public List<GameObject> towerButtons = new List<GameObject>();

    public List<string> towerNames = new List<string>();
    // Start is called before the first frame update


    private void Awake()
    {
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
        //CatalogueReset();
        //buildBlocksToggle.SetActive(true);
        //decorationBlocksToggle.SetActive(true);
        //specialBlocksToggle.SetActive(true);
        
        DefaultInterfaceState();
        StartCoroutine(AssignMainCategoryBackgroundOnStart(LevelBuildAndPlayManager.Instance.currentCategory));
        StartCoroutine(AssignToolSelectionBackgroundOnStart());
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void ButtonMethods(int buttonId)
    {
        //Tool selection

        if (buttonId == 0) //Build block place tool
        {
            LevelBuildAndPlayManager.Instance.ChangeTool(LevelBuildAndPlayManager.Tools.BuildBlockPlace);
        }

        if (buttonId == 1) //Build area assign tool
        {
            LevelBuildAndPlayManager.Instance.ChangeTool(LevelBuildAndPlayManager.Tools.BuildAreaAssign);
        }

        if (buttonId == 2) //Monster path create tool
        {
            LevelBuildAndPlayManager.Instance.ChangeTool(LevelBuildAndPlayManager.Tools.MonsterPathCreate);
        }

        if (buttonId == 3) //Save button 
        {
            LevelBuildAndPlayManager.Instance.SaveLevel();
        }

        if (buttonId == 4) //Rotate Cam Clockwise
        {
            LevelBuildAndPlayManager.Instance.RotateCamera("Clockwise");
        }

        if (buttonId == 5) // Rotate Cam CounterClockwise
        {
            LevelBuildAndPlayManager.Instance.RotateCamera("Counter Clockwise");
        }

        if (buttonId == 6) //Camera Buttons Toggle
        {
            cameraUiButtonsToggle.SetActive(!cameraUiButtonsToggle.activeSelf);
        }

        //Categories
        if (buttonId == 7) //switch to buildBlocks
        { 
            CatalogueReset();
            buildBlockHandle.SetActive(true);
            ChangeMainCategoryBackground(0);
            LevelBuildAndPlayManager.Instance.currentCategory = LevelBuildAndPlayManager.BuildBlockCategory.BuildBlocks;
        }

        if (buttonId == 8) //switch to decorationBlocks
        {
            CatalogueReset();
            decorationBlockHandle.SetActive(true);
            ChangeMainCategoryBackground(1);
            LevelBuildAndPlayManager.Instance.currentCategory = LevelBuildAndPlayManager.BuildBlockCategory.Decorations;
        }

        if (buttonId == 9) //Switch to specialBlocks
        {
            CatalogueReset();
            specialBlockHandle.SetActive(true);
            ChangeMainCategoryBackground(2);
            LevelBuildAndPlayManager.Instance.currentCategory = LevelBuildAndPlayManager.BuildBlockCategory.Special;
        }
    }

    public void CatalogueReset()//resets
    {
        buildBlockHandle.SetActive(false);
        decorationBlockHandle.SetActive(false);
        specialBlockHandle.SetActive(false);
    }

    public void SelectBuildBlock(int prefabId)
    { 
        int selectedBlockId = prefabId;
        
        subCategorySelectionBackground.GetComponent<Image>().color = notSelectedColor; 
        LevelBuildAndPlayManager.Instance.selectedBuildBlockId = selectedBlockId; 
        subCategorySelectionBackground = buildBlockButtons[selectedBlockId]; 
        subCategorySelectionBackground.GetComponent<Image>().color = selectionColor; 
        UpdateSelectionText(0); 
        //LevelBuildAndPlayManager.Instance.selectedBuildBlock = LevelBuildAndPlayManager.Instance.buildBlocksPrefabs[prefabId];
    }

    public void SelectSpecialBlock(int prefabId)//button
    {
        int specialBlockId = prefabId;

        subCategorySelectionBackground.GetComponent<Image>().color = notSelectedColor;
        LevelBuildAndPlayManager.Instance.selectedSpecialBlockId = specialBlockId;
        subCategorySelectionBackground = specialBlockButtons[specialBlockId];
        subCategorySelectionBackground.GetComponent<Image>().color = selectionColor;
        UpdateSelectionText(0);
    }

    public void SelectTower(int id)//button
    {
        subCategorySelectionBackground.GetComponent<Image>().color = notSelectedColor;
        subCategorySelectionBackground = towerButtons[id];
        subCategorySelectionBackground.GetComponent<Image>().color = selectionColor;
        LevelBuildAndPlayManager.Instance.currentTower = LevelBuildAndPlayManager.Instance.defenceTowers[id];
        //UpdateSelectionText(1);
    }
    

    /*public void CatalogueReset()
    {
        buildBlocksToggle.SetActive(false);
        decorationBlocksToggle.SetActive(false);
        specialBlocksToggle.SetActive(false);
    }*/

    public void UpdateSelectionText(int mode)
    {
        LevelBuildAndPlayManager.BuildBlockCategory category;
        category = LevelBuildAndPlayManager.Instance.currentCategory;
        string blockName = "";

        if (mode == 0)
        {
            blockName = buildBLockNames[mode];
        }

        if (mode == 1)
        {
            blockName = towerNames[mode];
        }

        string categoryName = "no category";
        
        if (category == LevelBuildAndPlayManager.BuildBlockCategory.BuildBlocks)
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

    public void DefaultInterfaceState() // add category default selection and tool default selection
    {
        //default is grass block in build blocks if this is not the case check lists
        int selectedBlockId = LevelBuildAndPlayManager.Instance.selectedBuildBlockId;
        subCategorySelectionBackground = buildBlockButtons[selectedBlockId];
        subCategorySelectionBackground.GetComponent<Image>().color = selectionColor;
        //default category
    }

    public void AssignToolSelectionBackground(int toolId)//button
    {
        toolSelectionBackground.GetComponent<Image>().color = notSelectedColor;
        
        if (toolId == 0)
        {
            toolSelectionBackground = buildToolHolder;
        }

        if (toolId == 1)
        {
            toolSelectionBackground = assignToolHolder;
        }

        if (toolId == 2)
        {
            toolSelectionBackground = pathToolHolder;
        }
        
        toolSelectionBackground.GetComponent<Image>().color = selectionColor;
    }

    private IEnumerator AssignToolSelectionBackgroundOnStart()
    {
        yield return new WaitForSeconds(1f);
        toolSelectionBackground = buildToolHolder;
        toolSelectionBackground.GetComponent<Image>().color = selectionColor;

    }

    public void ChangeMainCategoryBackground(int categoryId)
    {
        
        mainCategorySelectionBackground.GetComponent<Image>().color = notSelectedColor;
        
        if (categoryId == 0)
        {
            mainCategorySelectionBackground = buildBlocksToggle;
        }
        
        if (categoryId == 1)
        {
            mainCategorySelectionBackground = decorationBlocksToggle;
        }
        
        if (categoryId == 2)
        {
            mainCategorySelectionBackground = specialBlocksToggle;
        }

        mainCategorySelectionBackground.GetComponent<Image>().color = selectionColor;

    }
    
    private IEnumerator AssignMainCategoryBackgroundOnStart(LevelBuildAndPlayManager.BuildBlockCategory category) //this button is not used yet
    {
        yield return new WaitForSeconds(1);
        if (category == LevelBuildAndPlayManager.BuildBlockCategory.BuildBlocks)
        {
            mainCategorySelectionBackground = buildBlocksToggle;
        }

        if (category == LevelBuildAndPlayManager.BuildBlockCategory.Decorations)
        {
            mainCategorySelectionBackground = decorationBlocksToggle;
        }

        if (category == LevelBuildAndPlayManager.BuildBlockCategory.Special)
        {
            mainCategorySelectionBackground = specialBlocksToggle;
        }

        if (mainCategorySelectionBackground == null)
        { 
            mainCategorySelectionBackground = dummyCategory;
        }

        mainCategorySelectionBackground.GetComponent<Image>().color = selectionColor;
    }
}