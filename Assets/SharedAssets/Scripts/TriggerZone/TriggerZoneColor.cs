using UnityEngine;

public class TriggerZoneColor : MonoBehaviour
{
    private Renderer zoneRenderer;
    public Color defaultColor = Color.green;
    public Color highlightColor = Color.yellow;
    private bool isPlayerInside = false; // Track if player is inside
    private bool isActive = true; // Allow enabling/disabling effect

    void Start()
    {
        zoneRenderer = GetComponent<Renderer>();
        if (zoneRenderer != null)
        {
            zoneRenderer.material.color = defaultColor; // Start with default color
        }
    }

    public void EnableColorChange()
    {
        isActive = true;
        if (!isPlayerInside && zoneRenderer != null)
        {
            zoneRenderer.material.color = defaultColor; // Restore default color
        }
    }

    public void DisableColorChange()
    {
        isActive = false;
        if (zoneRenderer != null)
        {
            zoneRenderer.material.color = Color.clear; // Make it invisible
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive) return; // Do nothing if deactivated
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (zoneRenderer != null)
            {
                zoneRenderer.material.color = highlightColor; // Change to highlight color
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isActive) return; // Do nothing if deactivated
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (zoneRenderer != null)
            {
                zoneRenderer.material.color = defaultColor; // Reset to default color
            }
        }
    }
}