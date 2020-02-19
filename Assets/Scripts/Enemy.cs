using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform currentMarker;
    public int markerNumber;

    public List<GameObject> path;

    // Start is called before the first frame update
    private void Start()
    {
        Physics.IgnoreLayerCollision(14, 14);
        path = LevelBuildAndPlayManager.Instance.levelMonsterPathObjList;
        // not 0 because 0 is spawn
        markerNumber = 1;
        currentMarker = path[markerNumber].transform;
        agent = GetComponent<NavMeshAgent>();
        NavMeshHit hit;
        NavMesh.SamplePosition(currentMarker.position, out hit, 100, -1);
        agent.destination = hit.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (agent.remainingDistance < 0.1f)
        {
            if (markerNumber == path.Count - 1)
            {
                // enemy reached end
                Destroy(gameObject);
            }

            if (markerNumber < path.Count - 1)
            {
                markerNumber++;
                currentMarker = path[markerNumber].transform;
                NavMeshHit hit;
                NavMesh.SamplePosition(currentMarker.position, out hit, 100, -1);
                agent.destination = hit.position;
            }
        }
    }

    public void TeleportUpdate()
    {
        markerNumber++;
        currentMarker = path[markerNumber].transform;
        NavMeshHit hit;
        NavMesh.SamplePosition(currentMarker.position, out hit, 100, -1);
        agent.destination = hit.position;
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collision with portal");
            var test = coll.gameObject;
            Teleporter teleporterInstance = test.GetComponent<Teleporter>();
            teleporterInstance.TransportUnit(this.gameObject);
        }
    }
}