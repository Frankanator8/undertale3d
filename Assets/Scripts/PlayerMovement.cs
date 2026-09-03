using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 12f;
    public LayerMask groundMask;
    public bool freeze;

    const float coyoteTime = 0.1f;
    const float groundSkin = 0.1f;

    Rigidbody rb;
    CameraController cameraController;
    Collider playerCollider;
    Vector3 input;
    bool jumpQueued;
    float coyoteTimeRemaining;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        cameraController = GetComponentInChildren<CameraController>();
        playerCollider = GetComponent<Collider>();
        if (groundMask.value == 0)
            groundMask = LayerMask.GetMask("Ground");
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

        if (IsGrounded())
            coyoteTimeRemaining = coyoteTime;
        else
            coyoteTimeRemaining -= Time.deltaTime;

        if (!freeze && Input.GetKeyDown(KeyCode.Space) && coyoteTimeRemaining > 0f)
        {
            jumpQueued = true;
            coyoteTimeRemaining = 0f;
        }
    }

    void FixedUpdate()
    {
        float yaw = cameraController != null ? cameraController.Yaw : transform.eulerAngles.y;
        rb.MoveRotation(Quaternion.Euler(0f, yaw, 0f));

        Vector3 worldMove = Quaternion.Euler(0f, yaw, 0f) * input * speed;
        Vector3 velocity = rb.linearVelocity;
        if (jumpQueued && !freeze)
        {
            velocity.y = jumpForce;
            jumpQueued = false;
        }
        else if (jumpQueued)
        {
            jumpQueued = false;
        }
        rb.linearVelocity = new Vector3(worldMove.x, velocity.y, worldMove.z);
    }

    bool IsGrounded()
    {
        if (playerCollider == null)
            return false;

        Bounds bounds = playerCollider.bounds;
        float radius = Mathf.Min(bounds.extents.x, bounds.extents.z) * 0.4f;
        radius = Mathf.Max(radius, 0.05f);
        float distance = bounds.extents.y - radius + groundSkin;
        if (distance < groundSkin)
            distance = groundSkin;

        return Physics.SphereCast(
            bounds.center,
            radius,
            Vector3.down,
            out _,
            distance,
            groundMask,
            QueryTriggerInteraction.Ignore);
    }
}
