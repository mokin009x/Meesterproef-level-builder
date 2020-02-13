using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    //buildArea (accessed by number in grid)
    public List<int> buildArea;

    //block positions
    public List<int> gridIds;
    
    //block rotation
    public List<Vector3> blockRotation;
    public List<float> blockRotationX;
    public List<float> blockRotationY;
    public List<float> blockRotationZ;
    

    //prefab id
    public List<int> levelBlocksIds;

    //Monster path markers (accessed by number in grid)
    public List<int> monsterPath;
}