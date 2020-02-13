using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Teleporter : MonoBehaviour
{
    private LevelBuildAndPlayManager bManager;

    public GameObject pair;
    // Start is called before the first frame update
    void Awake()
    {
       
    }

    void Start()
    {
        bManager = LevelBuildAndPlayManager.Instance;
        if (bManager.teleporterPair == false)
        {
            bManager.pairTeleporterObj = this.gameObject;
            bManager.teleporterPair = true;
        }

        if (bManager.teleporterPair == true && bManager.teleporterPair == true)
        {
            pair = bManager.pairTeleporterObj;
            pair.GetComponent<Teleporter>().pair = this.gameObject;
        }
        // later make it so that when the second teleporter
        // isnt placed the first one destroys itself
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cancel()
    {
        bManager.pairTeleporterObj = null;
        bManager.teleporterPair = false; 
    }

    public void DestroyPortals()
    {
        
    }

    public void TransportUnit(GameObject unit)
    {
        Enemy enemy = unit.GetComponent<Enemy>();
        
        enemy.TeleportUpdate();

        unit.transform.position = pair.transform.position;

    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Enemy"))
        {
            TransportUnit(coll.gameObject);
        }
    }
}
