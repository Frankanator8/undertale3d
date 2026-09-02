using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    static List<FoodItem> inventory = new List<FoodItem>();
    static bool updateInventory = false;

    static FoodItem activeFoodItem;
    static bool updateActive = false;
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
    public static void SetActiveFoodItem(FoodItem item)
    {
        activeFoodItem = item;
        updateActive = true;
    }

    public static bool HasActiveFood()
    {
        return activeFoodItem != null;
    }

    public static FoodItem GetActiveFoodItem()
    {
        return activeFoodItem;
    }

    static bool HasItem(FoodItem item)
    {
        return inventory.Contains(item);
    }

    public GameObject inventorySlotPrefab; // Assign this in the Inspector with the InventorySlot prefab
    List<GameObject> inventorySlots = new List<GameObject>();

    public GameObject layoutGroup; // Assign this in the Inspector with the HorizontalLayoutGroup component
    public List<FoodItem> testItemCounts = new List<FoodItem>();
    public bool test;
    public GameObject gun;
    public GameObject food;
    public PlayerMovement playerMovement; // Assign the player's PlayerMovement in the Inspector; frozen while eating

    [Header("Eating")]
    public Vector3 eatLocalPosition = new Vector3(0.3f, -0.6f, 0.6f); // Food's local position (relative to camera) at the mouth
    public Vector3 eatLocalEuler = new Vector3(40f, 90f, -20f); // Food's local rotation (euler) at the mouth
    public float eatRaiseTime = 0.15f; // Time to raise/lower the food to/from the mouth pose
    public float eatBiteRate = 5f; // Minecraft-style nibble speed while eating, in bites per second
    public Vector3 eatBobPositionOffset = new Vector3(0f, -0.02f, 0.02f); // Extra local offset applied on each bite, on top of eatLocalPosition
    public Vector3 eatBobRotationOffset = new Vector3(5f, 0f, 0f); // Extra local rotation (degrees) applied on each bite

    bool isEating = false;
    Vector3 restLocalPosition;
    Quaternion restLocalRotation;

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

    void UpdateActive()
    {
        // Reset the food's pose so it doesn't stay mid-bite when switching items or back to the gun
        food.transform.localPosition = restLocalPosition;
        food.transform.localRotation = restLocalRotation;

        if (activeFoodItem == null)
        {
            gun.SetActive(true);
            food.SetActive(false);
        }
        else
        {
            gun.SetActive(false);
            food.SetActive(true);
            food.GetComponent<SpriteRenderer>().sprite = activeFoodItem.itemIcon;
        }
    }
    void Start()
    {
        // Cache the food's authored transform as its resting (non-eating) pose
        restLocalPosition = food.transform.localPosition;
        restLocalRotation = food.transform.localRotation;

        layoutGroup = GameObject.Find("HBox"); // Find the layout group in the scene
        _UpdateInventory(); // Initial inventory update
        UpdateActive(); // Initial active item update
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

        if (updateActive)
        {
            UpdateActive();
            updateActive = false;
        }

        if (Input.GetMouseButtonDown(1) && activeFoodItem != null && !isEating) // Right click (held) to eat the active food item
        {
            StartCoroutine(Eat());
        }
    }

    IEnumerator Eat()
    {
        isEating = true;
        SetFrozen(true); // Can't move while eating

        FoodItem itemBeingEaten = activeFoodItem;

        float duration = Mathf.Max(itemBeingEaten.eatTime, 0.05f);
        float raiseTime = Mathf.Min(eatRaiseTime, duration * 0.5f);
        float chewTime = duration - raiseTime;

        Vector3 startPosition = food.transform.localPosition;
        Quaternion startRotation = food.transform.localRotation;
        Quaternion mouthRotation = Quaternion.Euler(eatLocalEuler);

        // Quickly raise the food up to the mouth; releasing the button cancels with no effect
        float t = 0f;
        while (t < raiseTime)
        {
            if (!Input.GetMouseButton(1))
            {
                yield return CancelEat();
                yield break;
            }

            t += Time.deltaTime;
            float pct = Mathf.Clamp01(t / raiseTime);
            food.transform.localPosition = Vector3.Lerp(startPosition, eatLocalPosition, pct);
            food.transform.localRotation = Quaternion.Slerp(startRotation, mouthRotation, pct);
            yield return null;
        }
        food.transform.localPosition = eatLocalPosition;
        food.transform.localRotation = mouthRotation;

        // Nibble on it Minecraft-style for the rest of the item's eat time; releasing still cancels
        t = 0f;
        while (t < chewTime)
        {
            if (!Input.GetMouseButton(1))
            {
                yield return CancelEat();
                yield break;
            }

            t += Time.deltaTime;
            // Abs(sin) makes a repeating 0->1->0 pulse: a quick snap toward the mouth and back, "eatBiteRate" times per second
            float bite = Mathf.Abs(Mathf.Sin(t * eatBiteRate * Mathf.PI));
            food.transform.localPosition = eatLocalPosition + eatBobPositionOffset * bite;
            food.transform.localRotation = mouthRotation * Quaternion.Euler(eatBobRotationOffset * bite);
            yield return null;
        }

        food.transform.localPosition = eatLocalPosition;
        food.transform.localRotation = mouthRotation;

        itemBeingEaten.Consume(null);

        if (HasItem(itemBeingEaten))
        {
            // Still have more of this item left; lower it back to the resting pose
            yield return LerpFood(restLocalPosition, restLocalRotation, raiseTime);
        }
        else
        {
            // None left; switch back to the gun
            SetActiveFoodItem(null);
        }

        isEating = false;
        SetFrozen(false);
    }

    IEnumerator CancelEat()
    {
        // Right click was released before the eat timer finished; lower the food back down and consume nothing
        yield return LerpFood(restLocalPosition, restLocalRotation, eatRaiseTime);
        isEating = false;
        SetFrozen(false);
    }

    void SetFrozen(bool frozen)
    {
        if (playerMovement != null)
        {
            playerMovement.freeze = frozen;
        }
    }

    IEnumerator LerpFood(Vector3 targetLocalPosition, Quaternion targetLocalRotation, float duration)
    {
        Vector3 startPosition = food.transform.localPosition;
        Quaternion startRotation = food.transform.localRotation;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float pct = Mathf.Clamp01(t / duration);
            food.transform.localPosition = Vector3.Lerp(startPosition, targetLocalPosition, pct);
            food.transform.localRotation = Quaternion.Slerp(startRotation, targetLocalRotation, pct);
            yield return null;
        }

        food.transform.localPosition = targetLocalPosition;
        food.transform.localRotation = targetLocalRotation;
    }
}
