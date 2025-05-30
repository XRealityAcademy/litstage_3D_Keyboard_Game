using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemName = "key";
    public float pickupRange = 2f;

    // Optional reference to a Blender design prefab.
    public GameObject blenderDesignPrefab;

    // Optional reference to the container where the design should be instantiated.
    public Transform designContainer;

    // Scale multiplier for the instantiated Blender design.
    public float designScaleFactor = 1.0f;

    private GameObject designInstance;

    // This method runs in the Editor and ensures itemName is not null or empty.
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogWarning("itemName cannot be null or empty. Setting default value.", this);
            itemName = "Item";
        }
    }

    private void Start()
    {
        if (blenderDesignPrefab != null)
        {
            // Use a container if provided, or default to this GameObject.
            Transform parentTransform = designContainer != null ? designContainer : transform;
            designInstance = Instantiate(blenderDesignPrefab, parentTransform);
            designInstance.transform.localPosition = Vector3.zero;
            designInstance.transform.localRotation = Quaternion.identity;
            designInstance.transform.localScale = Vector3.one * designScaleFactor;

            // Ensure any Rigidbody on the design is kinematic to avoid non-convex MeshCollider issues.
            Rigidbody rb = designInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // Disable the MeshRenderer on the Item_Book to hide its default (purple) mesh.
            MeshRenderer mr = GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.enabled = false;
            }
        }
    }

    public void Collect()
    {
        Debug.Log("Item collected: " + itemName);
        InventoryManager3D.instance.AddItem(itemName, blenderDesignPrefab);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }

    public void TryCollect(Transform player)
    {
        float distance = Vector3.Distance(transform.position, player.position);
        Debug.Log($"📏 Distance to player: {distance} (pickupRange: {pickupRange})");
        if (distance <= pickupRange)
        {
            Collect();
        }
        else
        {
            Debug.Log($"🚫 Too far to pick up {itemName}");
        }
    }
}