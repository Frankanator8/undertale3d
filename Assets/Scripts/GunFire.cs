using UnityEngine;

public class GunFire : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject player;
    public Camera camera;
    public float fireRate = 0.5f; // Time between shots
    float nextFireTime = 0f;

    public float bulletSpeed = 20f; // Speed of the bullet
    public float fireShakeIntensity = 2f;
    public float fireShakeDuration = 0.15f;

    CameraController cameraController;
    float maxDistance;
    int projectileLayer;

    void Start()
    {
        if (camera != null)
            cameraController = camera.GetComponent<CameraController>();

        projectileLayer = bulletPrefab != null ? bulletPrefab.layer : 0;
        float lifetime = 5f;
        if (bulletPrefab != null)
        {
            BulletExpire expire = bulletPrefab.GetComponent<BulletExpire>();
            if (expire != null)
                lifetime = expire.lifetime;
        }
        maxDistance = bulletSpeed * lifetime;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button to fire
        {
            FireBullet();
        }
    }

    void FireBullet()
    {
        Vector3 origin = camera.transform.position + camera.transform.forward * 2f;
        Vector3 direction = camera.transform.forward;

        GameObject bullet = Instantiate(bulletPrefab, origin, camera.transform.rotation);

        Hitscan.Fire(
            new Ray(origin, direction),
            maxDistance,
            projectileLayer,
            player != null ? player.transform : null,
            bullet,
            out Vector3 impactPoint);

        BulletExpire expire = bullet.GetComponent<BulletExpire>();
        if (expire != null)
            expire.SetImpact(impactPoint, direction);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);

        if (cameraController != null)
            cameraController.Shake(fireShakeIntensity, fireShakeDuration);
    }
}
