using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    static List<FoodItem> inventory = new List<FoodItem>();
    static bool updateInventory = false;
    public static FoodItem RemoveItem(FoodItem item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            UpdateInventory();
            return item;
        }
        else
        {
            Debug.LogWarning("Item not found in inventory: " + item.itemName);
            return null;
        }
    }

    public static void AddItem(FoodItem item)
    {
        inventory.Add(item);
        UpdateInventory();
    }

    public GameObject inventorySlotPrefab; // Assign this in the Inspector with the InventorySlot prefab
    List<GameObject> inventorySlots = new List<GameObject>();

    public GameObject layoutGroup; // Assign this in the Inspector with the HorizontalLayoutGroup component
    public List<FoodItem> testItemCounts = new List<FoodItem>();
    public bool test;

    static void UpdateInventory()
    {
        updateInventory = true;
    }

    void _UpdateInventory()
    {
        // Clear existing inventory slots
        foreach (GameObject child in inventorySlots)
        {
            Destroy(child.gameObject);
        }

        inventorySlots.Clear();
        inventorySlots.Add(Instantiate(inventorySlotPrefab, layoutGroup.transform)); // Create a slot for the gun
        inventorySlots[0].GetComponent<InventorySlot>().isGunSlot = true; // Mark this slot as a gun slot
        inventorySlots[0].GetComponent<InventorySlot>().keyCode = KeyCode.Alpha1; // Assign the desired key code for the gun
        inventorySlots[0].GetComponent<InventorySlot>().itemCount = 1; // Assign the desired item count for the gun
        // Create new inventory slots for each item in the inventory

        // Count up food items and assign them to slots
        Dictionary<FoodItem, int> itemCounts = new Dictionary<FoodItem, int>();
        foreach (FoodItem item in inventory)
        {
            if (itemCounts.ContainsKey(item))
            {
                itemCounts[item]++;
            }
            else
            {
                itemCounts[item] = 1;
            }
        }

        for (int i = 0; i < itemCounts.Count; i++)
        {
            FoodItem item = new List<FoodItem>(itemCounts.Keys)[i];
            int count = itemCounts[item];

            GameObject slot = Instantiate(inventorySlotPrefab, layoutGroup.transform);
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            inventorySlot.foodItem = item;
            inventorySlot.keyCode = KeyCode.Alpha2 + i; // Assign key codes starting from Alpha2
            inventorySlot.itemCount = count;

            inventorySlots.Add(slot);
        }
    }

    void Start()
    {
        layoutGroup = GameObject.Find("HBox"); // Find the layout group in the scene
        _UpdateInventory(); // Initial inventory update
        if (test)
        {
            // add dictionary for testingf
            foreach (FoodItem item in testItemCounts)
            {
                AddItem(item);
            }

        }
        
    }

    void Update()
    {
        if (updateInventory)
        {
            _UpdateInventory();
            updateInventory = false;
        }
    }
}
