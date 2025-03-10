using UnityEngine;
using TMPro; // ✅ Use TextMeshPro for UI Text
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NPC_Interaction : MonoBehaviour
{
    public GameObject npcSpeechBubble;   // Speech bubble UI
    public TextMeshProUGUI npcText;      // ✅ Changed to TextMeshProUGUI
    public Button npcClickableIcon;      // Clickable button/icon for interaction
    public string npcDialogText = "Hello, traveler! Welcome to my shop."; // Default NPC text

    public DialogManager npcDialogManager;

    void Start()
    {
        if (npcSpeechBubble == null || npcText == null || npcClickableIcon == null)
        {
            Debug.LogError("⚠️ NPCInteraction: Missing UI elements in Inspector!");
        }

        npcSpeechBubble.SetActive(true);
        npcClickableIcon.onClick.AddListener(OnNPCIconClick);
    }

    public void OnNPCIconClick()
    {
        Debug.Log("✅ NPC Icon Clicked! Running OnNPCIconClick()");

        bool isActive = npcSpeechBubble.activeSelf;
        npcSpeechBubble.SetActive(!isActive);

        if (!isActive)
        {
            npcText.text = npcDialogText;

            if (npcDialogManager != null)
            {
                npcDialogManager.ShowDialog(npcDialogText);
            }
            else
            {
                Debug.LogWarning("⚠️ DialogManager is missing! Ensure it's assigned.");
            }
        }
    }
}