using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpaceValues
{
    public int gridId;
    public int buildBlockId;
    public int xAxis;
    public int yAxis;
    public int zAxis;
    public bool hasBuildBlock;
    public int layer;
    
    public GridSpaceValues()
    {
        Defaults();
    }

    public GridSpaceValues(int buildBlockId, int xAxis, int yAxis, int zAxis,bool hasBuildBlock, int layer, int gridId)
    {
        this.buildBlockId = buildBlockId;
        this.xAxis = xAxis;
        this.yAxis = yAxis;
        this.zAxis = zAxis;
        this.hasBuildBlock = hasBuildBlock;
        this.layer = layer;
        this.gridId = gridId;

    }

    void Awake()
    {
        Defaults();
    }

    void Defaults()
    {
        xAxis = 0;
        yAxis = 0;
        zAxis = 0;
        buildBlockId = -1;
      
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
