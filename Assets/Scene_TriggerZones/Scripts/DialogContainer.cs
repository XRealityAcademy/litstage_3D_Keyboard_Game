using UnityEngine;

public class DialogContainer : MonoBehaviour
{
    [Header("References")]
    public GameObject dialogUI; // ✅ The pre-existing dialog inside this container

    private void Start()
    {
        if (dialogUI == null)
        {
            Debug.LogError("❌ Dialog UI reference is missing! Assign it in the Inspector.");
            return;
        }

        // ✅ Ensure it starts inactive
        dialogUI.SetActive(false);
        Debug.Log("🔹 Dialog initialized as HIDDEN.");
    }

    // ✅ Show or hide the dialog
    public void SetVisible(bool isVisible)
    {
        if (dialogUI == null)
        {
            Debug.LogError("❌ Dialog UI is NULL! Cannot set visibility.");
            return;
        }

        dialogUI.SetActive(isVisible);
        Debug.Log("🔄 Dialog is now " + (isVisible ? "VISIBLE ✅" : "HIDDEN ❌"));
    }

    // ✅ Check if the dialog is currently active
    public bool IsVisible()
    {
        return dialogUI != null && dialogUI.activeSelf;
    }
}