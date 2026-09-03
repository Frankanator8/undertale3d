using UnityEngine;

public class BulletExpire : MonoBehaviour
{
    public float lifetime = 5f; // Time in seconds before the bullet is destroyed
    public TrailRenderer trail;
    public float trailDelay = 0.06f; // Seconds to wait after spawn before the trail appears

    bool dead;
    float trailDelayRemaining;
    bool hasImpact;
    Vector3 impactPoint;
    Vector3 impactDirection;

    void Awake()
    {
        if (trail == null)
            trail = GetComponentInChildren<TrailRenderer>();
        if (trail == null)
            return;

        // TrailRenderer mis-tracks when parented to a scaled/rotating rigidbody.
        // Keep it in world space at scale 1 and follow the bullet each frame.
        Vector3 spawnPos = transform.position;
        trail.emitting = false;
        trail.Clear();
        trail.transform.SetParent(null);
        trail.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
        trail.transform.localScale = Vector3.one;
        trail.Clear();
        trailDelayRemaining = trailDelay;
    }

    public void SetImpact(Vector3 point, Vector3 direction)
    {
        hasImpact = true;
        impactPoint = point;
        impactDirection = direction;
    }

    void LateUpdate()
    {
        if (dead)
            return;

        if (hasImpact && Vector3.Dot(impactPoint - transform.position, impactDirection) <= 0f)
        {
            transform.position = impactPoint;
            if (trail != null)
                trail.transform.position = impactPoint;
            Kill();
            return;
        }

        if (trail == null)
            return;

        trail.transform.position = transform.position;

        if (trailDelayRemaining > 0f)
        {
            trailDelayRemaining -= Time.deltaTime;
            if (trailDelayRemaining <= 0f)
            {
                trail.Clear();
                trail.emitting = true;
            }
        }
    }

    void Update()
    {
        if (dead)
            return;

        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
        }
        else
        {
            Kill();
        }
    }

    void Kill()
    {
        if (dead)
            return;
        dead = true;

        if (trail != null)
        {
            trail.emitting = false;
            Destroy(trail.gameObject, trail.time + 0.1f);
        }
        Destroy(gameObject);
    }
}
