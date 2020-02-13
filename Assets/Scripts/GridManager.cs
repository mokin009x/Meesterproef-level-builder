using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public enum LevelSizes
    {
        Small = 10,
        Medium = 15,
        Huge = 20
    }

    public static GridManager Instance;
    public int gridSize;
    public GameObject gridSpacePrefab;
    public List<GameObject> levelGrid = new List<GameObject>();
    public LevelSizes levelSize;
    public int xAxisGrid;
    public int yAxisGrid = 5;
    public int zAxisGrid;


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


    // Start is called before the first frame update
    private void Start()
    {
        yAxisGrid = 5;
        levelSize = LevelSizes.Small;
        SizeCheck();
        xAxisGrid = gridSize;
        zAxisGrid = gridSize;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GenerateGrid();
            LevelBuildAndPlayManager.Instance.ChangeGridVisibility();
        }
    }

    private void SizeCheck()
    {
        int size = 0;
        if (levelSize == LevelSizes.Small)
        {
            size = 10;
        }

        if (levelSize == LevelSizes.Medium)
        {
            size = 15;
        }

        if (levelSize == LevelSizes.Huge)
        {
            size = 15;
        }

        gridSize = size;
    }

    private void GenerateGrid()
    {
        int id = 0;
        for (int i = 0; i < yAxisGrid; i++)
        {
            for (int j = 0; j < zAxisGrid; j++)
            {
                for (int k = 0; k < xAxisGrid; k++)
                {
                    GameObject newSpace = gridSpacePrefab;
                    GameObject instance = Instantiate(newSpace, new Vector3(k, i, j), Quaternion.identity);
                    int gridId = id;
                    instance.layer = 8 + i;
                    GridSpaceValues newSpaceValuesInstance = new GridSpaceValues(gridId);
                    instance.GetComponent<GridSpace>().values = newSpaceValuesInstance;
                    levelGrid.Add(instance);
                    id++;
                }
            }
        }
    }

    public int GetGridSize()
    {
        int size = 0;
        if (levelSize == LevelSizes.Small)
        {
            size = 10;
        }

        if (levelSize == LevelSizes.Medium)
        {
            size = 15;
        }

        if (levelSize == LevelSizes.Huge)
        {
            size = 15;
        }

        return size;
    }
}