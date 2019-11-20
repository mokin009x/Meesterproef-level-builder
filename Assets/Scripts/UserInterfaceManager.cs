using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonMethods(int buttonId)
    {
        //Tool selection
        
        if (buttonId == 0)//Build block place tool
        {
            LevelBuilderManager.Instance.ChangeTool(LevelBuilderManager.Tools.BuildBlockPlace);
            Debug.Log("test");
        }
        
        if (buttonId == 1)//Build area assign tool
        {
            LevelBuilderManager.Instance.ChangeTool(LevelBuilderManager.Tools.BuildAreaAssign);
            Debug.Log("test");
        }
        
        if (buttonId == 2)//Monster path create tool
        {
            LevelBuilderManager.Instance.ChangeTool(LevelBuilderManager.Tools.MonsterPathCreate);
            Debug.Log("test");
        }
    }

}
