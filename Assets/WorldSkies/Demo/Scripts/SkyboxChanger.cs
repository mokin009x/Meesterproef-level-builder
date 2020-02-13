using UnityEngine;
using UnityEngine.UI;

public class SkyboxChanger : MonoBehaviour
{
    private Dropdown _dropdown;
    public Material[] Skyboxes;

    public void Awake()
    {
        _dropdown = GetComponent<Dropdown>();
        //var options = Skyboxes.Select(skybox => skybox.name).ToList();
        //_dropdown.AddOptions(options);
    }

    public void ChangeSkybox()
    {
        RenderSettings.skybox = Skyboxes[_dropdown.value];
    }
}