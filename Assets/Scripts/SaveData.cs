using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    //prefab id
    public List<int> levelBlocksIds;
    
    //block positions
    public List<int> gridIds;

    //Monster path markers (accessed by number in grid)
    public List<int> monsterPath;
    
    //buildArea (accessed by number in grid)
    public List<int> buildArea;
}
