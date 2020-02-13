using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    public static NavMeshManager Instance;

    [SerializeField] private NavMeshSurface navMeshSurface;

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
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //navMeshSurfaces = LevelBuildAndPlayManager.Instance.walkableSurfaces.ToArray();
            navMeshSurface.BuildNavMesh();
        }
    }
}