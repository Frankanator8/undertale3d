using UnityEngine;
[CreateAssetMenu(fileName = "FoodItem", menuName = "Food Item")]
public class FoodItem : ScriptableObject
{
    public string itemName;
    public int healthRestored;

    public Sprite itemIcon;

    public void Consume(PlayerHealth playerHealth)
    {
        if (healthRestored < 0)
        {
            PlayerHealth.Heal(PlayerHealth.maxHealth-PlayerHealth.currentHealth);
        } else
        {
            PlayerHealth.Heal(healthRestored);
        }

        if (itemName == "Bisicle")
        {
            // Add Unisicle to inventory
        }
        
    }
}