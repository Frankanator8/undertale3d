using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    Rigidbody rb;
    Vector3 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
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
    }

    void FixedUpdate()
    {
        Vector3 worldMove = transform.TransformDirection(input) * speed;
        Vector3 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(worldMove.x, velocity.y, worldMove.z);
    }
}
