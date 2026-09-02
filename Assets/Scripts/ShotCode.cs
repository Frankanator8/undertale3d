using System;
using UnityEngine;

public class ShotCode : MonoBehaviour
{
    public Action<GameObject> onShot;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            onShot?.Invoke(other.gameObject);
        }
    }

}
