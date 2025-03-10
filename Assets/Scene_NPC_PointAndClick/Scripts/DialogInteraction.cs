using UnityEngine;
using TMPro; // ✅ Using TextMeshPro for UI Text
using UnityEngine.UI;
using System.Collections;

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
        npcSpeechBubble.SetActive(false); // Hide dialog at start
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
            // Shrink the button on first click
            StartCoroutine(ShrinkDialogButton());
            //npcSpeechBubble.SetActive(true);
            StartCoroutine(AnimatePopup()); // ✅ Smooth pop-up animation

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

        // Reset button size for next interaction
        npcClickableIcon.transform.localScale = new Vector3(3f, 3f, 3f);
    }

    private IEnumerator AnimatePopup()
    {
        npcSpeechBubble.transform.localScale = Vector3.zero; // Start small
        npcSpeechBubble.SetActive(true);

        float duration = 0.3f;
        float time = 0;

        while (time < duration)
        {
            float scale = Mathf.Lerp(0, 1, time / duration);
            npcSpeechBubble.transform.localScale = new Vector3(scale, scale, scale);
            time += Time.deltaTime;
            yield return null;
        }

        npcSpeechBubble.transform.localScale = Vector3.one; // Ensure final scale is 1
    }

    private IEnumerator ShrinkDialogButton()
    {
        float duration = 0.2f; // Animation time
        float time = 0;
        Vector3 startScale = npcClickableIcon.transform.localScale; // Start with the big size
        Vector3 endScale = Vector3.one; // Normal size (1,1,1)

        while (time < duration)
        {
            npcClickableIcon.transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        npcClickableIcon.transform.localScale = endScale; // Ensure final size is exact
    }
}