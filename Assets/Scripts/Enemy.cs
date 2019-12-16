using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int markerNumber = 0;
    public List<GameObject> path;
    public Transform currentMarker;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(14,14);
        path = LevelBuildAndPlayManager.Instance.monsterPathPos;
        // not 0 because 0 is spawn
        markerNumber = 1;
        currentMarker = path[markerNumber].transform;
        agent = GetComponent<NavMeshAgent>();
        NavMeshHit hit;
        NavMesh.SamplePosition(currentMarker.position,out hit,100,-1);
        agent.destination = hit.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 0.1f)
        {
            if (markerNumber == path.Count - 1) 
            {
                // enemy reached end
                Destroy(this.gameObject);
            }
            
            if (markerNumber < path.Count - 1)
            {
                markerNumber++;
                currentMarker = path[markerNumber].transform;
                NavMeshHit hit;
                NavMesh.SamplePosition(currentMarker.position,out hit,100,-1);
                agent.destination = hit.position;
            }
        }
    }
}
