using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Settings")]
    public float mouseSensitivity = 200f;
    public Transform playerBody; // Drag the Player Capsule/Root here

    [Header("Shake")]
    public float defaultShakeIntensity = 2f;
    public float defaultShakeDuration = 0.15f;

    public float Yaw { get; private set; }

    private float xRotation = 0f;
    private Vector3 restLocalPosition;
    private float shakeTimeRemaining;
    private float shakeDuration = 0.15f;
    private float shakeIntensity;

    void Start()
    {
        // Locks the cursor to the middle of the screen and hides it
        Cursor.lockState = CursorLockMode.Locked;
        restLocalPosition = transform.localPosition;
        if (playerBody != null)
            Yaw = playerBody.eulerAngles.y;
    }

    public void Shake()
    {
        Shake(defaultShakeIntensity, defaultShakeDuration);
    }

    public void Shake(float intensity, float duration)
    {
        shakeIntensity = intensity;
        shakeDuration = Mathf.Max(duration, 0.0001f);
        shakeTimeRemaining = duration;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        Yaw += mouseX;
        xRotation = Mathf.Clamp(xRotation - mouseY, -90f, 90f);
    }

    void LateUpdate()
    {
        float shakePitch = 0f;
        float shakeYaw = 0f;
        float shakeRoll = 0f;
        Vector3 posOffset = Vector3.zero;

        if (shakeTimeRemaining > 0f)
        {
            shakeTimeRemaining -= Time.deltaTime;
            float t = Mathf.Clamp01(shakeTimeRemaining / shakeDuration);
            float envelope = t * t;

            // Kick up, plus a short decaying jitter so follow-up shots can refresh the shake
            shakePitch = -shakeIntensity * envelope;
            shakeYaw = (Mathf.PerlinNoise(Time.time * 35f, 0.1f) * 2f - 1f) * shakeIntensity * 0.4f * envelope;
            shakeRoll = (Mathf.PerlinNoise(0.3f, Time.time * 35f) * 2f - 1f) * shakeIntensity * 0.3f * envelope;

            posOffset = new Vector3(
                (Mathf.PerlinNoise(Time.time * 28f, 1.7f) * 2f - 1f),
                envelope * 0.4f + (Mathf.PerlinNoise(2.3f, Time.time * 28f) * 2f - 1f) * 0.5f,
                (Mathf.PerlinNoise(4.1f, Time.time * 28f) * 2f - 1f)
            ) * (0.015f * shakeIntensity * envelope);
        }

        transform.localPosition = restLocalPosition + posOffset;
        // Apply look in world space so interpolated rigidbody yaw on the parent cannot jitter the camera
        transform.rotation = Quaternion.Euler(0f, Yaw, 0f)
            * Quaternion.Euler(xRotation + shakePitch, -90f + shakeYaw, shakeRoll);
    }
}
