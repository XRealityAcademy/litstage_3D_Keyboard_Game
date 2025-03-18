using UnityEngine;

public class TriggerZoneDialog : MonoBehaviour
{
    [Header("References")]
    public DialogContainer dialogContainer; // ✅ Reference to the dialog container

    [Header("Settings")]
    public bool triggerOnlyOnce = true;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || (hasTriggered && triggerOnlyOnce)) return;
        hasTriggered = true;

        if (dialogContainer != null)
        {
            Debug.Log("📌 Player entered trigger zone! Attempting to show dialog...");

            // ✅ Activate the dialog
            dialogContainer.SetVisible(true);

            // ✅ Verify if it actually became visible
            if (dialogContainer.IsVisible())
            {
                Debug.Log("✅ Dialog successfully displayed!");
            }
            else
            {
                Debug.LogError("❌ Dialog did not become visible! Something is wrong.");
            }
        }
        else
        {
            Debug.LogError("❌ dialogContainer reference is missing in TriggerZoneDialog!");
        }
    }

    public void CloseDialog()
    {
        if (dialogContainer != null)
        {
            Debug.Log("🗑️ Closing dialog...");
            dialogContainer.SetVisible(false);
        }
    }
}