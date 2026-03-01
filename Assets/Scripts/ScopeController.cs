using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScopeController : MonoBehaviour
{
    public GameObject scopeOverlay; // Assign the scope overlay UI element in the Inspector
    public Camera camera;
    public CameraController cameraController; // Reference to the CameraController script

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button to toggle scope
        {
            StartCoroutine(ShowScopeOverlay());
        }

        if (Input.GetMouseButtonUp(1)) // Hide scope when right mouse button is released
        {
            scopeOverlay.SetActive(false); // Hide the scope overlay when not aiming
            camera.fieldOfView = 60f; // Reset camera FOV to default
            cameraController.mouseSensitivity = 200f; // Reset mouse sensitivity to default
        }
    }

    IEnumerator ShowScopeOverlay()
    {
        cameraController.mouseSensitivity = 30f;
        scopeOverlay.SetActive(true); // Show the scope overlay
        for (float alpha = 0f; alpha <= 1f; alpha += Time.deltaTime * 10) // Fade in over 0.1 seconds
        {
            for (int i = 0; i < scopeOverlay.transform.childCount; i++)
            {
                Image childImage = scopeOverlay.transform.GetChild(i).GetComponent<Image>();
                if (childImage != null)
                {
                    Color childColor = childImage.color;
                    childColor.a = alpha; // Fade in child elements
                    childImage.color = childColor;
                }
            }
            camera.fieldOfView = Mathf.Lerp(60f, 30f, alpha); // Zoom in the camera
            yield return null; // Wait for the next frame
        }
        for (int i = 0; i < scopeOverlay.transform.childCount; i++)
        {
            Image childImage = scopeOverlay.transform.GetChild(i).GetComponent<Image>();
            if (childImage != null)
            {
                Color childColor = childImage.color;
                childColor.a = 1f; // Set fully opaque
                childImage.color = childColor;
            }
        }
        yield return null;
    }
}
