using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Image image;
    TextMeshProUGUI itemKeyText;
    TextMeshProUGUI itemCountText;
    public FoodItem foodItem;
    public KeyCode keyCode; // Assign this in the Inspector with the desired key code
    public int itemCount; // Assign this in the Inspector with the desired item count

    public Sprite gunIcon; // Assign this in the Inspector with the gun icon sprite

    public bool isGunSlot = false;
    void Start()
    {
        image = transform.Find("Image").GetComponent<Image>();
        itemCountText = transform.Find("Number").GetComponent<TextMeshProUGUI>();
        itemKeyText = transform.Find("Keyboard").Find("Key").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGunSlot)
        {
            image.sprite = gunIcon;
            itemKeyText.text = keyCode.ToString()[5].ToString();
            itemCountText.text = ""; // No count for the gun
        }
        else
        {
            if (foodItem != null)
            {
                image.sprite = foodItem.itemIcon;
                itemKeyText.text = keyCode.ToString()[5].ToString(); // Display only the number part of the KeyCode (e.g., "Alpha1" becomes "1")
                itemCountText.text = itemCount.ToString(); // Assuming you have a count for the item, replace "1" with the actual count
            }
            else
            {
                image.sprite = null; // Clear the image if no item is assigned
                itemKeyText.text = "";
                itemCountText.text = "";
            }
        }
        if (Input.GetKeyDown(keyCode))
        {
            if (isGunSlot)
            {
                Inventory.SetActiveFoodItem(null); // Set active food item to null when gun is selected
            }
            else
            {
                Inventory.SetActiveFoodItem(foodItem);
            }
        }

    }
}
