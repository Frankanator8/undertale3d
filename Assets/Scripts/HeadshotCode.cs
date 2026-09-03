using System;
using UnityEngine;

public class HeadshotCode : MonoBehaviour, IHitscanTarget
{
    public Action onHeadshot;

    public bool BlocksHitscan => false;

    public void OnHitscan(GameObject source)
    {
        Debug.Log("Headshot!");
        onHeadshot?.Invoke();
    }
}
