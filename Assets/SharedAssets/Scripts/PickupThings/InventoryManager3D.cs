using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager3D : MonoBehaviour
{
    public static InventoryManager3D instance;

    // Use TextMeshProUGUI since you’re using TMP
    public TextMeshProUGUI inventoryText;

    // List to hold our collected inventory items
    private List<InventoryItem3D> collectedItems = new List<InventoryItem3D>();

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
        InventoryItem3D newItem = new InventoryItem3D(itemName, designPrefab);
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