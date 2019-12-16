using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{

    [SerializeField] NavMeshSurface navMeshSurface;

    public static NavMeshManager Instance;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //navMeshSurfaces = LevelBuildAndPlayManager.Instance.walkableSurfaces.ToArray();
            navMeshSurface.BuildNavMesh();
        }
    }
}
