using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private float cameraRotationX;
    private CharacterController control;

    public float maxCameraRotationX = 60f;

    public float moveSpeed = 5f;

    private Camera playerCamera;

    public bool showGUI = true;
    public float turnSpeed = 180f;

    // Use this for initialization
    private void Start()
    {
        control = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        Screen.lockCursor = true;

        // look
        transform.Rotate(0f, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, 0f);

        if (playerCamera)
        {
            cameraRotationX = Mathf.Clamp(cameraRotationX + -Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime, -maxCameraRotationX, maxCameraRotationX);
            playerCamera.transform.forward = transform.forward;
            playerCamera.transform.Rotate(cameraRotationX, 0f, 0f);
        }

        // move
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        move = transform.TransformDirection(move).normalized;
        move *= moveSpeed;
        control.SimpleMove(move);
    }

    private void OnGUI()
    {
        if (!showGUI) return;

        GUILayout.BeginArea(new Rect(Screen.width * 0.1f, Screen.height * 0.7f, Screen.width * 0.8f, Screen.height * 0.25f), GUI.skin.GetStyle("Box"));
        GUILayout.Label("3D Skybox for Unity");
        GUILayout.Label("The dark grey buildings are level geometry that contain your player within the gameplay area.");
        GUILayout.Label("The light grey buildings are in the \"3D Skybox model\", which is a scale model \"containing\" the level geometry.  Any 3D effects like particle systems, animation, moving lights, etc, may be placed in the 3D Skybox for great effects!  An ordinary 2D Unity Skybox can also be included.  In this example scene, the standard \"Sunny1 Skybox\" is used.");
        GUILayout.EndArea();
    }
}