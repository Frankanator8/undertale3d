using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static int maxHealth = 20;
    public static int currentHealth = 20;

    static private GameObject healthBar; // Reference to the health bar GameObject
    static private GameObject healthText;

    void Start()
    {
        healthBar = transform.Find("Health").gameObject; // Find the HealthBar child object
        healthText = transform.Find("HealthText").gameObject; // Find the HealthText child object
        UpdateHealthBar();
    }

    static void UpdateHealthBar()
    {
        // Update the health bar's scale based on current health
        float healthPercentage = (float)currentHealth / maxHealth;
        healthBar.GetComponent<RectTransform>().localScale = new Vector3(healthPercentage, 1f, 1f);

        // Update the health text
        healthText.GetComponent<TextMeshProUGUI>().text = $"{currentHealth}/{maxHealth}";
    }

    void Update()
    {
        // For testing purposes, you can press the H key to take damage
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
    }

    public static void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0; // Prevent health from going below 0
        UpdateHealthBar();
    }

    public static void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth; // Prevent health from exceeding max health
        UpdateHealthBar();
    }
    
}
