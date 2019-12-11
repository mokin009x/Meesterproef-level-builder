using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<GameObject> path;
    public Transform currentMarker;
    // Start is called before the first frame update
    void Start()
    {
        path = LevelBuildAndPlayManager.Instance.monsterPathPos;
        // not 0 because 0 is spawn
        currentMarker = path[1].transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
