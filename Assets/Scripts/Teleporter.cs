﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Teleporter : MonoBehaviour
{

    public GameObject pair;
    // Start is called before the first frame update
    void Awake()
    {
       
    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignPairStage1()
    {
        LevelBuildAndPlayManager.Instance.teleporterPair = true; 
        LevelBuildAndPlayManager.Instance.pairTeleporterObj1 = this.gameObject;
    }

    public void AssignPairStage2()
    {
        //assign both pairs
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        LevelBuildAndPlayManager.Instance.pairTeleporterObj2 = this.gameObject;
        pair = LevelBuildAndPlayManager.Instance.pairTeleporterObj1;
        pair.GetComponent<Teleporter>().pair = LevelBuildAndPlayManager.Instance.pairTeleporterObj2;
        
        //reset
        LevelBuildAndPlayManager.Instance.teleporterPair = false;
        LevelBuildAndPlayManager.Instance.pairTeleporterObj1 = null;
        LevelBuildAndPlayManager.Instance.pairTeleporterObj2 = null;
    }

    public void Cancel()
    {
        LevelBuildAndPlayManager.Instance.pairTeleporterObj1 = null;
        LevelBuildAndPlayManager.Instance.teleporterPair = false; 
        DestroyPortals();
    }

    public void DestroyPortals()
    {
        Destroy(pair);
        Destroy(this.gameObject);
    }

    public void TransportUnit(GameObject unit)
    {
        Enemy enemy = unit.GetComponent<Enemy>();
        
        //enemy.TeleportUpdate();

        enemy.agent.Warp(pair.transform.position);

    }

 
}
