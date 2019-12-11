using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpaceValues
{
    public int gridId;
    public bool hasBuildBlock;
    public bool hasMarker;
    public bool buildArea;
    public bool hasTower;
    
    public GridSpaceValues()
    {
        Defaults();
    }

    public GridSpaceValues( int gridId)
    {
        this.gridId = gridId;
        Defaults();
    }

    void Awake()
    {
        Defaults();
    }

    void Defaults()
    {
        
        hasBuildBlock = false;
        hasMarker = false;
        buildArea = false;
        hasTower = false;

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
