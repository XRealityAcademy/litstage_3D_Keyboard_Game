using UnityEngine;

public class TriggerZoneDialog : MonoBehaviour
{
    [Header("References")]
    public GameObject player;  // The player GameObject
    public GameObject dialogUI; // The dialog UI to attach

    [Header("Settings")]
    public bool triggerOnlyOnce = true; // Whether the event should happen once
    private bool hasTriggered = false; // Prevent re-triggering if enabled

    private Transform dialogTransform;
    private Transform playerTransform;

    private void Start()
    {
        if (dialogUI != null)
        {
            dialogUI.SetActive(false); // Hide dialog initially
            dialogTransform = dialogUI.transform;
        }

        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !hasTriggered)
        {
            hasTriggered = true; // Mark as triggered if one-time event

            if (dialogUI != null && playerTransform != null)
            {
                AttachDialogToPlayer();
            }
        }
    }

    private void AttachDialogToPlayer()
    {
        dialogUI.SetActive(true); // Show the dialog
        dialogTransform.SetParent(playerTransform); // Make the dialog follow the player
        dialogTransform.localPosition = new Vector3(0, 2.5f, 0); // Adjust position above player head
    }

    public void CloseDialog()
    {
        if (dialogUI != null)
        {
            dialogUI.SetActive(false);
            dialogTransform.SetParent(null); // Detach from player
        }
    }
}