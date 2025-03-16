using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogInteraction : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject npcSpeechBubble;   // Dialog Panel (for visibility toggle)
    public TextMeshProUGUI npcText;      // Dialog Text
    public Button dialogCloseButton;     // Close Button (Closes dialog)
    public TextMeshProUGUI npcNameplate; // Displays NPC name in the dialog
    public Transform floatingDialog;     // FloatingDialog canvas reference

    [Header("Dialog Icons")]
    public GameObject DialogTriggerButtonCenter;  // ✅ The icon above NPC's head
    public GameObject DialogTriggerButtonCorner;  // ✅ The icon at the bottom-right of dialog

    [Header("Position Settings")]
    private Vector3 originalDialogPosition; // Stores the initial position of the dialog
    private bool hasMoved = false;

    [Header("NPC Data")]
    public NPCDialogData npcData; // 📜 ScriptableObject for Dialog
    private int currentLineIndex = 0;

    [Header("Audio Manager")]
    private DialogAudioManager audioManager; //  Use external audio manager
    private DialogTextAnimator textAnimator; // New Text Animator

    private Vector3 bigButtonSize; // Normal dialog button size
    private float animationDuration = 0.2f; // Animation duration
    private Coroutine jumpRoutine;
    private Coroutine pulseRoutine;

    void Start()
    {
        // **Check if UI elements are assigned**
        if (npcSpeechBubble == null || npcText == null || dialogCloseButton == null)
        {
            Debug.LogError("⚠️ NPCInteraction: Missing UI elements in Inspector!");
            return;
        }

        originalDialogPosition = floatingDialog.position; // Store initial position

        // **Initialize UI**
        npcSpeechBubble.SetActive(false); // Hide dialog at start
        dialogCloseButton.gameObject.SetActive(false);

        // **Ensure correct button visibility at start**
        if (DialogTriggerButtonCenter != null) DialogTriggerButtonCenter.SetActive(true);
        if (DialogTriggerButtonCorner != null) DialogTriggerButtonCorner.SetActive(false);

        // **Ensure DialogTriggerButtonCorner stays in bottom-right corner**
        // Ensure the button is initially hidden, but don't change its position
        if (DialogTriggerButtonCorner != null)
        {
            DialogTriggerButtonCorner.SetActive(false);
        }


        // **Load NPC Data if available**
        if (npcData != null) npcNameplate.text = npcData.npcName;

        // **Get or Add Components**
        audioManager = GetComponent<DialogAudioManager>() ?? gameObject.AddComponent<DialogAudioManager>();
        textAnimator = GetComponent<DialogTextAnimator>() ?? gameObject.AddComponent<DialogTextAnimator>();

        jumpRoutine = StartCoroutine(BounceDialogButton());
        pulseRoutine = null;
    }

    public void OnNPCIconClick()
    {
        // ✅ Move only ONCE
        if (!hasMoved)
        {
            hasMoved = true;
        }

        // ✅ Switch Button Visibility (Hide center, Show corner)
        if (DialogTriggerButtonCenter != null) DialogTriggerButtonCenter.SetActive(false);
        if (DialogTriggerButtonCorner != null) DialogTriggerButtonCorner.SetActive(true);

        // ✅ Stop bouncing effect
        if (jumpRoutine != null)
        {
            StopCoroutine(jumpRoutine);
            jumpRoutine = null;
        }

        // ✅ Start pulsing effect
        if (pulseRoutine == null)
        {
            pulseRoutine = StartCoroutine(PulseButtonEffect());
        }

        // ✅ If the speech bubble is inactive, activate it
        if (!npcSpeechBubble.activeSelf)
        {
            StartCoroutine(AnimatePopup()); // Smooth pop-up animation
        }

        // ✅ Show the dialog text and play the sound
        if (npcData != null && npcData.dialogLines.Length > 0)
        {
            ShowNextDialogLine();
        }
    }

    private void ShowNextDialogLine()
    {
        if (currentLineIndex >= npcData.dialogLines.Length)
        {
            CloseDialog();
            return;
        }

        // ✅ Show the current dialog line
        npcText.text = npcData.dialogLines[currentLineIndex].text;

        // ✅ Play the associated voice clip
        if (audioManager != null && npcData.dialogLines[currentLineIndex].voiceClip != null)
        {
            audioManager.PlayVoiceClip(npcData.dialogLines[currentLineIndex].voiceClip);
        }

        currentLineIndex++;
    }

    public void CloseDialog()
    {
        // ✅ Hide the Dialog
        npcSpeechBubble.SetActive(false);
        dialogCloseButton.gameObject.SetActive(false);
        currentLineIndex = 0;

        // ✅ Stop any playing audio
        if (audioManager != null)
        {
            audioManager.StopVoice();
        }

        // ✅ Stop pulsing effect
        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
            pulseRoutine = null;
        }

        // ✅ Reset FloatingDialog position
        floatingDialog.position = originalDialogPosition;
        hasMoved = false;

        // ✅ Switch Button Visibility Back (Show center, Hide corner)
        if (DialogTriggerButtonCenter != null) DialogTriggerButtonCenter.SetActive(true);
        if (DialogTriggerButtonCorner != null) DialogTriggerButtonCorner.SetActive(false);

        jumpRoutine = StartCoroutine(BounceDialogButton());
    }

    private IEnumerator AnimatePopup()
    {
        npcSpeechBubble.transform.localScale = Vector3.zero;
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

        npcSpeechBubble.transform.localScale = Vector3.one;
    }

    private IEnumerator BounceDialogButton()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);

            float duration = 0.3f;
            float time = 0;
            Vector3 startPos = DialogTriggerButtonCenter.transform.localPosition;
            Vector3 endPos = startPos + new Vector3(0, 25f, 0);

            while (time < duration)
            {
                DialogTriggerButtonCenter.transform.localPosition = Vector3.Lerp(startPos, endPos, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            time = 0;
            while (time < duration)
            {
                DialogTriggerButtonCenter.transform.localPosition = Vector3.Lerp(endPos, startPos, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator PulseButtonEffect()
    {
        float pulseDuration = 0.5f;
        float pulseSize = 2f;

        while (true)
        {
            yield return ScaleButton(pulseSize, pulseDuration);
            yield return ScaleButton(1f, pulseDuration);
        }
    }

    private IEnumerator ScaleButton(float targetScale, float duration)
    {
        float time = 0;
        Vector3 startScale = DialogTriggerButtonCorner.transform.localScale;
        Vector3 endScale = new Vector3(targetScale, targetScale, targetScale);

        while (time < duration)
        {
            DialogTriggerButtonCorner.transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        DialogTriggerButtonCorner.transform.localScale = endScale; // ✅ Now only scales, doesn’t move!
    }
}