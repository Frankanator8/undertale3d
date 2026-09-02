using System;
using UnityEngine;

public class ActCode : MonoBehaviour
{
    public Action<GameObject> onAct;
    public Sprite mercySprite;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            onAct?.Invoke(other.gameObject);
        }
    }

    public void SwitchToMercySprite()
    {
        GetComponent<SpriteRenderer>().sprite = mercySprite;
    }
}

