public class GridSpaceValues
{
    public bool buildArea;
    public int gridId;
    public bool hasBuildBlock;
    public bool hasSpecial;
    public bool hasMarker;
    public bool hasTower;

    public GridSpaceValues()
    {
        Defaults();
    }

    public GridSpaceValues(int gridId)
    {
        this.gridId = gridId;
        Defaults();
    }

    private void Awake()
    {
        Defaults();
    }

    private void Defaults()
    {
        hasBuildBlock = false;
        hasMarker = false;
        buildArea = false;
        hasTower = false;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }


    // Update is called once per frame
    private void Update()
    {
    }
}