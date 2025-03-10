using UnityEngine;
using TMPro; // ✅ Using TextMeshPro for UI Text
using UnityEngine.UI;

public class DialogInteraction : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject npcSpeechBubble;   // 🗨️ Dialog Panel (for visibility toggle)
    public TextMeshProUGUI npcText;      // 📝 Dialog Text
    public Button npcClickableIcon;      // 🎯 Dialog Button (Opens dialog)
    public Button npcDialogButton;       // ❌ Close Button (Closes dialog)

    [Header("NPC Settings")]
    [TextArea(2, 5)]
    public string npcDialogText = "Hello, traveler! Welcome to my shop."; // 📝 Default text

    void Start()
    {
        Debug.Log("✅ DialogInteraction `Start()` is running on " + gameObject.name);

        // **Check if UI elements are assigned**
        if (npcSpeechBubble == null || npcText == null || npcClickableIcon == null || npcDialogButton == null)
        {
            Debug.LogError("⚠️ NPCInteraction: Missing UI elements in Inspector!");
            return;
        }

        // **Initialize UI**
        npcSpeechBubble.SetActive(true); // Hide dialog at start
        npcClickableIcon.onClick.AddListener(OnNPCIconClick); // Assign click event
        npcDialogButton.onClick.AddListener(CloseDialog); // Assign close button event
    }

    public void OnNPCIconClick()
    {
        bool isActive = npcSpeechBubble.activeSelf;
        npcSpeechBubble.SetActive(isActive); // Toggle visibility

        if (isActive)
        {
            npcText.text = npcDialogText; // Update text
        }
    }

    public void CloseDialog()
    {
        npcSpeechBubble.SetActive(false); // Hide the dialog
    }
}