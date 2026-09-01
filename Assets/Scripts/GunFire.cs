using UnityEngine;

public class GunFire : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Camera camera;
    public float fireRate = 0.5f; // Time between shots
    float nextFireTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button to fire
        {
            if (Time.time >= nextFireTime)
            {
                FireBullet();
                nextFireTime = Time.time + fireRate;
            }

        }
    }

    void FireBullet()
    {
        // Instantiate the bullet at the camera's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, camera.transform.position + camera.transform.forward * 2f, camera.transform.rotation);

        Collider bulletCol = bullet.GetComponent<Collider>();
        Collider playerCol = GetComponent<Collider>();
        if (bulletCol != null && playerCol != null)
            Physics.IgnoreCollision(bulletCol, playerCol);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(camera.transform.forward * 1000f);
        }
    }
}
