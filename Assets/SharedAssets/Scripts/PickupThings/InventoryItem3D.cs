using UnityEngine;

[System.Serializable]
public class InventoryItem3D
{
    public string itemName;
    public GameObject designPrefab; // Optional: the Blender design (prefab)

    public InventoryItem3D(string name, GameObject prefab = null)
    {
        itemName = name;
        designPrefab = prefab;
    }
}