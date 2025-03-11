using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogInteraction : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject npcSpeechBubble;   // Dialog Panel (for visibility toggle)
    public TextMeshProUGUI npcText;      // Dialog Text
    public Button dialogTriggerButton;      // Dialog Button (Opens dialog)
    public Button dialogCloseButton;       // Close Button (Closes dialog)
    public TextMeshProUGUI npcNameplate; // Displays NPC name in the dialog

    [Header("NPC Data")]
    public NPCDialogData npcData; // 📜 ScriptableObject for Dialog
    private int currentLineIndex = 0; // 📌 Track current dialog position

    [Header("Audio Manager")]
    private DialogAudioManager audioManager; //  Use external audio manager
    private DialogTextAnimator textAnimator; // New Text Animator


    private Vector3 bigButtonSize;// Normal dialog button size
    private float animationDuration = 0.2f; // Animation duration
    private Coroutine jumpRoutine;
    private Coroutine pulseRoutine;


    void Start()
    {

        // **Check if UI elements are assigned**
        if (npcSpeechBubble == null || npcText == null || dialogTriggerButton == null || dialogCloseButton == null)
        {
            Debug.LogError("⚠️ NPCInteraction: Missing UI elements in Inspector!");
            return;
        }

        bigButtonSize = dialogTriggerButton.transform.localScale;// Save original button size
        

        // **Initialize UI**
        npcSpeechBubble.SetActive(false); // Hide dialog at start
        dialogCloseButton.gameObject.SetActive(false);
        dialogTriggerButton.onClick.AddListener(OnNPCIconClick); // Assign click event
        dialogCloseButton.onClick.AddListener(CloseDialog); // Assign close button event

        // **Load NPC Data if available**
        if (npcData != null)
        {
            npcNameplate.text = npcData.npcName; // Set NPC Name
        }


        // Get or Add the DialogAudioManager component
        audioManager = GetComponent<DialogAudioManager>();
        if (audioManager == null)
        {
            audioManager = gameObject.AddComponent<DialogAudioManager>();
        }

        // Get or Add the DialogTextAnimator component
        textAnimator = GetComponent<DialogTextAnimator>();
        if (textAnimator == null)
        {
            textAnimator = gameObject.AddComponent<DialogTextAnimator>();
        }

        jumpRoutine = StartCoroutine(BounceDialogButton());
        pulseRoutine = null;

    }

    public void OnNPCIconClick()
    {

        if (jumpRoutine != null)
        {
            StopCoroutine(jumpRoutine); // Stop the bouncing effect
            jumpRoutine = null;
        }

        if (pulseRoutine == null)
        {
            pulseRoutine = StartCoroutine(PulseButtonEffect());            
        }

        // If the speech bubble is inactive, activate it
        if (!npcSpeechBubble.activeSelf)
        {
            // Shrink the button on first click
            StartCoroutine(ShrinkDialogButton());
            //npcSpeechBubble.SetActive(true);
            StartCoroutine(AnimatePopup()); // Smooth pop-up animation

        }

        if (npcData != null && npcData.dialogLines.Length > 0)
        {
            // **Check if this is the LAST line**
            if (currentLineIndex >= npcData.dialogLines.Length)
            {
                CloseDialog();  // If user clicks after the last line, close dialog
                return;
            }

            // **Show the current dialog line and sound **
            audioManager.PlayVoiceClip(npcData.dialogLines[currentLineIndex].voiceClip);
            textAnimator.AnimateText(npcText, npcData.dialogLines[currentLineIndex].text, 0.05f);



            currentLineIndex++; // Move to the next line
        }

    }

    // **Closes the dialog when clicking after the last line**
    public void CloseDialog()
    {
        
        // **Stop pulsing effect if running**
        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
            pulseRoutine = null;
        }

        npcSpeechBubble.SetActive(false);
        currentLineIndex = 0; // Reset for the next interaction
        // **Stop any playing audio**
        audioManager.StopVoice();



        jumpRoutine = StartCoroutine(BounceDialogButton());
        StartCoroutine(ResetDialogButton()); // 🛠️ Restore button smoothly

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
        float time = 0;
        Vector3 startScale = dialogTriggerButton.transform.localScale;
        Vector3 endScale = new Vector3(1.5f, 1.5f, 1.5f);

        Vector3 fixedPosition = dialogTriggerButton.transform.position; // Lock current position

        while (time < animationDuration)
        {
            dialogTriggerButton.transform.localScale = Vector3.Lerp(startScale, endScale, time / animationDuration);
            dialogTriggerButton.transform.position = fixedPosition; // Keep it in place
            time += Time.deltaTime;
            yield return null;
        }

        dialogTriggerButton.transform.localScale = endScale;
        dialogTriggerButton.transform.position = fixedPosition; // Ensure it stays in place

        
    }

    private IEnumerator ResetDialogButton()
    {
        float time = 0;
        Vector3 startScale = dialogTriggerButton.transform.localScale;
        Vector3 endScale = bigButtonSize; // Restore to big size using the originalSize

        while (time < animationDuration)
        {
            dialogTriggerButton.transform.localScale = Vector3.Lerp(startScale, endScale, time / animationDuration);
            time += Time.deltaTime;
            yield return null;
        }

        dialogTriggerButton.transform.localScale = endScale; // Ensure final size is exact
    }

    private IEnumerator BounceDialogButton()
    {
        while (true) // Keep bouncing until stopped
        {
            yield return new WaitForSeconds(1.5f); // Delay between bounces

            float duration = 0.3f; // Jump duration
            float time = 0;
            Vector3 startPos = dialogTriggerButton.transform.localPosition;
            Vector3 endPos = startPos + new Vector3(0, 25f, 0); // Jump up by 10 units

            // Jump up animation
            while (time < duration)
            {
                dialogTriggerButton.transform.localPosition = Vector3.Lerp(startPos, endPos, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            time = 0;
            // Jump down animation
            while (time < duration)
            {
                dialogTriggerButton.transform.localPosition = Vector3.Lerp(endPos, startPos, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator PulseButtonEffect()
    {
        float pulseDuration = 0.5f; // Speed of pulse animation
        float pulseSize = 2f; // Slightly bigger than normal (like a heart beat)

        while (true) // Keep looping until the user clicks
        {
            // Pulse Up
            yield return ScaleButton(pulseSize, pulseDuration);

            // Pulse Down
            yield return ScaleButton(1f, pulseDuration);
        }
    }

    // Helper function to smoothly scale the button
    private IEnumerator ScaleButton(float targetScale, float duration)
    {
        float time = 0;
        Vector3 startScale = dialogTriggerButton.transform.localScale;
        Vector3 endScale = new Vector3(targetScale, targetScale, targetScale);
        Vector3 fixedPosition = dialogTriggerButton.transform.position; // Lock position

        while (time < duration)
        {
            dialogTriggerButton.transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            dialogTriggerButton.transform.position = fixedPosition; // Keep in place
            time += Time.deltaTime;
            yield return null;
        }

        dialogTriggerButton.transform.localScale = endScale;
        dialogTriggerButton.transform.position = fixedPosition;
    }
}