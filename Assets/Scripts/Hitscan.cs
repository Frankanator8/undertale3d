using System.Collections.Generic;
using UnityEngine;

public interface IHitscanTarget
{
    // False means the ray keeps going past this volume (head capsule sits
    // inside the body capsule and must not shadow it).
    bool BlocksHitscan { get; }
    void OnHitscan(GameObject source);
}

public static class Hitscan
{
    const int BufferSize = 64;
    const float StopEpsilon = 1e-3f;

    static readonly RaycastHit[] hits = new RaycastHit[BufferSize];
    static readonly DistanceComparer comparer = new DistanceComparer();

    public static void Fire(Ray ray, float maxDistance, int projectileLayer, Transform ignoreRoot, GameObject source, out Vector3 impactPoint)
    {
        int count = Physics.RaycastNonAlloc(ray, hits, maxDistance, ~0, QueryTriggerInteraction.Collide);
        int projectileMask = 1 << projectileLayer;

        int valid = 0;
        for (int i = 0; i < count; i++)
        {
            Collider col = hits[i].collider;
            if (col == null)
                continue;
            if (ignoreRoot != null && col.transform.IsChildOf(ignoreRoot))
                continue;
            if ((col.excludeLayers.value & projectileMask) != 0)
                continue;

            hits[valid++] = hits[i];
        }

        if (valid > 1)
            System.Array.Sort(hits, 0, valid, comparer);

        float stopDistance = maxDistance;
        for (int i = 0; i < valid; i++)
        {
            Collider col = hits[i].collider;
            IHitscanTarget target = col.GetComponent<IHitscanTarget>();
            bool blocks = target != null ? target.BlocksHitscan : !col.isTrigger;
            // if target is null (i.e. has no IHitScan), then if it is a trigger, we let the ray pass through, otherwise we stop at the collider. 
            if (blocks)
            {
                stopDistance = hits[i].distance;
                break;
            }
        }

        for (int i = 0; i < valid; i++)
        {
            if (hits[i].distance > stopDistance + StopEpsilon)
                break;

            IHitscanTarget target = hits[i].collider.GetComponent<IHitscanTarget>();
            target?.OnHitscan(source);
        }

        impactPoint = ray.GetPoint(stopDistance);
    }

    class DistanceComparer : IComparer<RaycastHit>
    {
        public int Compare(RaycastHit a, RaycastHit b)
        {
            return a.distance.CompareTo(b.distance);
        }
    }
}
