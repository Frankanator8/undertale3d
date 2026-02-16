using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Settings")]
    public float mouseSensitivity = 200f;
    public Transform playerBody; // Drag the Player Capsule/Root here

    private float xRotation = 0f;

    void Start()
    {
        // Locks the cursor to the middle of the screen and hides it
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get mouse input multiplied by sensitivity and time
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calculate vertical rotation (Up/Down)
        xRotation -= mouseY;
        
        // Clamp the rotation so you can't flip the camera over behind you
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply vertical rotation to the camera
        transform.localRotation = Quaternion.Euler(xRotation, -90f, 0f);

        // Apply horizontal rotation to the player body (Left/Right)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
