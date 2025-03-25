using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    // Use TextMeshProUGUI since you’re using TMP
    public TextMeshProUGUI inventoryText;

    // List to hold our collected inventory items
    private List<InventoryItem> collectedItems = new List<InventoryItem>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Overloaded method: Add item without a design
    public void AddItem(string itemName)
    {
        AddItem(itemName, null);
    }

    public void AddItem(string itemName, GameObject designPrefab)
    {
        // Create and store your inventory item with an optional design.
        InventoryItem newItem = new InventoryItem(itemName, designPrefab);
        collectedItems.Add(newItem);
        UpdateInventoryUI();
    }

    // Update the inventory UI text to show collected items
    private void UpdateInventoryUI()
    {
        string displayText = "Inventory:\n";
        foreach (var item in collectedItems)
        {
            displayText += "- " + item.itemName + "\n";
        }
        inventoryText.text = displayText;
    }
}