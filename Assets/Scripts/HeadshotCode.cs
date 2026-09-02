using System;
using UnityEngine;

public class HeadshotCode : MonoBehaviour
{
    public Action onHeadshot;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            onHeadshot?.Invoke();
        }
    }
}
