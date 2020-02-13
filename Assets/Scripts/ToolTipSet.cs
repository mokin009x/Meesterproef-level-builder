using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTipSet : MonoBehaviour
{
    public GameObject targetButton;
    public GameObject toolTip;
    public int toolTipId;
    public bool toolTipMove;
    public List<TextMeshProUGUI> toolTips;

    public GameObject triangleTest;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void ChangeText(int toolTipTextId)
    {
        toolTip.SetActive(true);
        toolTip.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = toolTips[toolTipTextId].text;
        toolTipMove = true;

        //ChangePosition();
    }

    public void ChangeTriangle(GameObject target)
    {
        float test = 0;
        test = target.transform.position.x;
        triangleTest.transform.position = new Vector3(test, triangleTest.transform.position.y, triangleTest.transform.position.z);
    }


    public void ChangePosition(GameObject targetPosition)
    {
        toolTip.transform.position = targetPosition.transform.position;
    }

    public void HideToolTip()
    {
        toolTipMove = false;
        toolTip.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        toolTip.SetActive(false);
    }
}