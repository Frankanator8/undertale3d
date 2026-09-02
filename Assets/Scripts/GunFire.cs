using UnityEngine;

public class GunFire : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject player;
    public Camera camera;
    public float fireRate = 0.5f; // Time between shots
    float nextFireTime = 0f;

    public float bulletSpeed = 20f; // Speed of the bullet


    // FixedUpdate is called once per fixed frame
    void FixedUpdate()
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
        Vector3 playerVelocity = player.GetComponent<Rigidbody>().linearVelocity;

        if (rb != null)
        {
            rb.AddForce(camera.transform.forward * bulletSpeed, ForceMode.Impulse);
            rb.AddForce(playerVelocity, ForceMode.VelocityChange); // Add player's velocity to the bullet
        }
    }
}
