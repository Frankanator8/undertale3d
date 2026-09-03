using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    Rigidbody rb;
    CameraController cameraController;
    Vector3 input;
    public bool freeze;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        cameraController = GetComponentInChildren<CameraController>();
    }

    void Update()
    {
        input = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) input += Vector3.left;
        if (Input.GetKey(KeyCode.S)) input += Vector3.right;
        if (Input.GetKey(KeyCode.A)) input += Vector3.back;
        if (Input.GetKey(KeyCode.D)) input += Vector3.forward;
        if (input.sqrMagnitude > 1f)
            input.Normalize();
        
        if (freeze)
        {
            input = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        float yaw = cameraController != null ? cameraController.Yaw : transform.eulerAngles.y;
        rb.MoveRotation(Quaternion.Euler(0f, yaw, 0f));

        Vector3 worldMove = Quaternion.Euler(0f, yaw, 0f) * input * speed;
        Vector3 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(worldMove.x, velocity.y, worldMove.z);
    }
}
