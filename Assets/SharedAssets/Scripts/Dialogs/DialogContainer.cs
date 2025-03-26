using UnityEngine;

public class DialogContainer : MonoBehaviour
{
    [Header("References")]
    public GameObject dialogUI; // ✅ The UI to show/hide
    public string eventToListen; // ✅ Event name set in the Inspector
    


    private void Start()
    {
        if (dialogUI == null)
        {
            Debug.LogError("❌ Dialog UI reference is missing! Assign it in the Inspector.");
            return;
        }

        // ✅ Ensure it starts hidden
        dialogUI.SetActive(false);

        // ✅ Subscribe dynamically to any event (set in Inspector)
        if (!string.IsNullOrEmpty(eventToListen))
        {
            EventManager.Subscribe(eventToListen, ShowDialog);
        }
    }

    private void OnDestroy()
    {
        // ✅ Unsubscribe when object is destroyed
        if (!string.IsNullOrEmpty(eventToListen))
        {
            EventManager.Unsubscribe(eventToListen, ShowDialog);
        }
    }

    private void ShowDialog()
    {
        SetVisible(true);
    }

    public void CloseDialog()
    {
        SetVisible(false);

    }

    public void SetVisible(bool isVisible)
    {
        if (dialogUI == null)
        {
            Debug.LogError("❌ Dialog UI is NULL! Cannot set visibility.");
            return;
        }

        dialogUI.SetActive(isVisible);
//        Debug.Log($"🔄 Dialog ({eventToListen}) is now " + (isVisible ? "VISIBLE ✅" : "HIDDEN ❌"));
    }

    public bool IsVisible()
    {
        return dialogUI != null && dialogUI.activeSelf;
    }
}