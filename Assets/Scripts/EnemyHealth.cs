using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    GameObject healthBar;
    public float damage = 10f;
    public float headshotDamage = 10f;

    public float maxHealth = 100f;
    public float currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthBar = transform.Find("HealthBar").gameObject;
        GetComponentInChildren<HeadshotCode>().onHeadshot += OnHeadshot;
        GetComponentInChildren<ShotCode>().onShot += OnShot;
    }

    void OnHeadshot()
    {
        currentHealth -= headshotDamage;
        UpdateHealthBar();
    }

    void OnShot(GameObject bullet)
    {
        Destroy(bullet);
        currentHealth -= damage;
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        float healthPercentage = currentHealth / maxHealth;
        healthBar.GetComponent<SpriteRenderer>().size = new Vector2(healthPercentage*1.7f, 0.3f);
        healthBar.transform.localPosition = new Vector3(-0.85f + healthPercentage * 0.85f, 1.31f, 0f);

    }
}
