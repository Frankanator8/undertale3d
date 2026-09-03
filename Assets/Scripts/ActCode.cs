using System;
using UnityEngine;

public class ActCode : MonoBehaviour, IHitscanTarget
{
    public Action<GameObject> onAct;
    public Sprite mercySprite;

    public bool BlocksHitscan => true;

    public void OnHitscan(GameObject source)
    {
        onAct?.Invoke(source);
    }

    public void SwitchToMercySprite()
    {
        GetComponent<SpriteRenderer>().sprite = mercySprite;
    }
}
