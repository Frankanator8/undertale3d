using UnityEngine;

public class BulletExpire : MonoBehaviour
{
    public float lifetime = 5f; // Time in seconds before the bullet is destroyed

    // Update is called once per frame
    void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime; // Decrease lifetime by the time since last frame
        }
        else
        {
            Destroy(gameObject); // Destroy the bullet when lifetime expires
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject); // Destroy the bullet when it collides with something
    }
}
