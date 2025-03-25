using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public GameObject designPrefab; // Optional: the Blender design (prefab)

    public InventoryItem(string name, GameObject prefab = null)
    {
        itemName = name;
        designPrefab = prefab;
    }
}