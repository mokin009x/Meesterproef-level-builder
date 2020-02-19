using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    //buildArea (accessed by number in grid)
    public List<int> buildArea =new List<int>();

    //block positions
    public List<int> buildBlockGridIds = new List<int>();//build blocks
    public List<int> specialBlockGridIds = new List<int>();//special blocks
    
    //block rotation build blocks
    public List<float> blockRotationX = new List<float>();
    public List<float> blockRotationY = new List<float>();
    public List<float> blockRotationZ = new List<float>();

    //block rotation special blocks
    public List<float> specialRotationX = new List<float>();
    public List<float> specialRotationY = new List<float>();
    public List<float> specialRotationZ = new List<float>();

    //prefab id
    public List<int> levelBlocksIds = new List<int>();
    public List<int> specialBlockIds = new List<int>();

    //Monster path markers (accessed by number in grid)
    public List<int> monsterPath = new List<int>();
}