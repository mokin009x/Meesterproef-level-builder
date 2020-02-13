using UnityEngine;

public class Skybox3DCamera : MonoBehaviour
{
    public Camera useSpecificCamera;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void LateUpdate()
    {
        Camera camera = useSpecificCamera ? useSpecificCamera : Camera.main;

        if (!camera)
        {
            Debug.LogWarning("Skybox3DCamera.Update() " + name + " no valid camera!");
            return;
        }

        transform.rotation = camera.transform.rotation;
    }
}