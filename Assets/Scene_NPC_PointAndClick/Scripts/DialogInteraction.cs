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
    public TextMeshProUGUI npcNameplate; // 🏷️ Displays NPC name in the dialog
    [Header("NPC Information")]
    public string npcName; // 🏷️ Custom NPC name set from the Inspector

    [Header("NPC Settings")]
    [TextArea(2, 5)]
    public string npcDialogText = "Hello, traveler! Welcome to my shop."; // 📝 Default text

    [Header("NPC Data")]
    public NPCDialogData npcData; // 📜 ScriptableObject for Dialog
    private int currentLineIndex = 0; // 📌 Track current dialog position

    [Header("Audio Manager")]
    private DialogAudioManager audioManager; // ✅ Use external audio manager
    private DialogTextAnimator textAnimator; // ✅ New Text Animator



    void Start()
    {

        // **Check if UI elements are assigned**
        if (npcSpeechBubble == null || npcText == null || npcClickableIcon == null || npcDialogButton == null)
        {
            Debug.LogError("⚠️ NPCInteraction: Missing UI elements in Inspector!");
            return;
        }

        // **Initialize UI**
        npcSpeechBubble.SetActive(true); // Hide dialog at start
        npcDialogButton.gameObject.SetActive(false);
        npcClickableIcon.onClick.AddListener(OnNPCIconClick); // Assign click event
        npcDialogButton.onClick.AddListener(CloseDialog); // Assign close button event

        // **Load NPC Data if available**
        if (npcData != null)
        {
            npcNameplate.text = npcData.npcName; // Set NPC Name
        }


        // ✅ Get or Add the DialogAudioManager component
        audioManager = GetComponent<DialogAudioManager>();
        if (audioManager == null)
        {
            audioManager = gameObject.AddComponent<DialogAudioManager>();
        }

        // ✅ Get or Add the DialogTextAnimator component
        textAnimator = GetComponent<DialogTextAnimator>();
        if (textAnimator == null)
        {
            textAnimator = gameObject.AddComponent<DialogTextAnimator>();
        }
    }

    public void OnNPCIconClick()
    {
        // If the speech bubble is inactive, activate it
        if (!npcSpeechBubble.activeSelf)
        {
            npcSpeechBubble.SetActive(true);
        }

        if (npcData != null && npcData.dialogLines.Length > 0)
        {
            // **Check if this is the LAST line**
            if (currentLineIndex >= npcData.dialogLines.Length)
            {
                CloseDialog();  // ✅ If user clicks after the last line, close dialog
                return;
            }

            // **Show the current dialog line and sound **
            audioManager.PlayVoiceClip(npcData.dialogLines[currentLineIndex].voiceClip);
            textAnimator.AnimateText(npcText, npcData.dialogLines[currentLineIndex].text, 0.05f);



            currentLineIndex++; // ✅ Move to the next line
        }
        else
        {
            textAnimator.AnimateText(npcText, npcDialogText, 0.05f); // Default text

        }

    }

    // **Closes the dialog when clicking after the last line**
    public void CloseDialog()
    {
        npcSpeechBubble.SetActive(false);
        currentLineIndex = 0; // ✅ Reset for the next interaction
        // 🎵 **Stop any playing audio**
        audioManager.StopVoice();
    }
}