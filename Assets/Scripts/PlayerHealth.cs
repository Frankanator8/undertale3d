using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static int maxHealth = 90;
    public static int currentHealth = 90;
    public static int kr = 0;
    public static int iframes = 0;
    private static int krFrames = 0;

    static private GameObject healthBar; // Reference to the health bar GameObject
    static private GameObject healthText;
    static private GameObject krBar;

    void Start()
    {
        healthBar = transform.Find("Health").gameObject; // Find the HealthBar child object
        healthText = transform.Find("HealthText").gameObject; // Find the HealthText child object
        krBar = transform.Find("KR").gameObject; // Find the KR child object
        UpdateHealthBar();
    }

    static void UpdateHealthBar()
    {
        // Update the health bar's scale based on current health
        float healthPercentage = (float)(currentHealth-kr) / maxHealth;
        healthBar.GetComponent<RectTransform>().localScale = new Vector3(healthPercentage, 1f, 1f);

        // Update the health text
        healthText.GetComponent<TextMeshProUGUI>().text = $"{currentHealth}/{maxHealth}";

        // Update the KR bar's scale based on current KR
        float krPercentage = (float)(currentHealth) / maxHealth;
        krBar.GetComponent<RectTransform>().localScale = new Vector3(krPercentage, 1f, 1f);

        if (PlayerHealth.kr > 0)
        {
            healthText.GetComponent<TextMeshProUGUI>().color = Color.magenta;
        }
        else
        {
            healthText.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }

    void Update()
    {
        // For testing purposes, you can press the H key to take damage
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
        if (Input.GetKey(KeyCode.J))
        {
            TakeDamage(1, 1, 0);
        }
    }
    void FixedUpdate()
    {
        if (PlayerHealth.iframes > 0)
        {
            PlayerHealth.iframes--;
        }
        if (PlayerHealth.kr > 0)
        {
            if (PlayerHealth.krFrames == 0)
            {
                PlayerHealth.kr--;
                PlayerHealth.currentHealth--;
                if (PlayerHealth.kr < 10)
                {
                    PlayerHealth.krFrames = 50;
                } else if (PlayerHealth.kr < 20)
                {
                    PlayerHealth.krFrames = 25;
                } else if (PlayerHealth.kr < 30)
                {
                    PlayerHealth.krFrames = 12;
                } else if (PlayerHealth.kr < 40)
                {
                    PlayerHealth.krFrames = 6;
                } else
                {
                    PlayerHealth.krFrames = 1;
                }
                UpdateHealthBar();
            } else
            {
                PlayerHealth.krFrames--;
            }
            
        }
    }

    public static void TakeDamage(int damage, int kr, int iframes)
    {
        if (PlayerHealth.iframes > 0)
        {
            // If the player is in invincibility frames, do not take damage
            return;
        }
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0; // Prevent health from going below 0
        PlayerHealth.kr += kr;
        if (PlayerHealth.kr > currentHealth - 1) PlayerHealth.kr = currentHealth - 1; // Prevent KR from exceeding max health
        PlayerHealth.iframes = iframes;
        UpdateHealthBar();
    }

    public static void TakeDamage(int damage)
    {
        TakeDamage(damage, 0, 40); // Call the overloaded method with default KR and iframes values
    }

    public static void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth; // Prevent health from exceeding max health
        UpdateHealthBar();
    }
    
}
