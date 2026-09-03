using System;
using UnityEngine;

public class ShotCode : MonoBehaviour, IHitscanTarget
{
    public Action<GameObject> onShot;

    public bool BlocksHitscan => true;

    public void OnHitscan(GameObject source)
    {
        Debug.Log("Shot!");
        onShot?.Invoke(source);
    }
}
