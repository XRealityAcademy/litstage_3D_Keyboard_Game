using UnityEngine;

public class NPC_Interaction : MonoBehaviour
{
    [Header("References")]
    public GameObject floatingDialog; // ✅ Reference to the FloatingDialog

    [Header("Settings")]
    public bool startWithDialogVisible = false; // ✅ New flag for initial visibility

    private bool hasDialog; // ✅ Determines if this NPC currently has a dialog

    void Start()
    {
        if (floatingDialog == null)
        {
            Debug.LogWarning($"❌ {gameObject.name} has no FloatingDialog assigned!");
            return;
        }

        // ✅ Set initial dialog visibility based on flag
        floatingDialog.SetActive(startWithDialogVisible);
        hasDialog = startWithDialogVisible;

        //Debug.Log($"🔄 FloatingDialog for {gameObject.name} starts as " +(startWithDialogVisible ? "VISIBLE ✅" : "HIDDEN ❌"));
    }

    // ✅ Enable or disable the dialog dynamically
    public void EnableDialog(bool enable)
    {
        if (floatingDialog != null)
        {
            floatingDialog.SetActive(enable);
            hasDialog = enable;
            Debug.Log($"🔄 FloatingDialog for {gameObject.name} is now " +
                      (enable ? "VISIBLE ✅" : "HIDDEN ❌"));
        }
    }

    // ✅ Check if this NPC currently has a dialog visible
    public bool HasDialog()
    {
        return hasDialog;
    }
}