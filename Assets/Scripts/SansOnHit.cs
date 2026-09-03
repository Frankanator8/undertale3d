using UnityEngine;

public class SansOnHit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponentInChildren<ShotCode>().onShot += OnShot;
    }

    // Update is called once per frame
    void OnShot(GameObject bullet)
    {
        int randomX = Random.Range(-60, 60);
        int randomY = Random.Range(-60, 60);
        transform.position = new Vector3(randomX, transform.position.y, randomY);
    }
}
