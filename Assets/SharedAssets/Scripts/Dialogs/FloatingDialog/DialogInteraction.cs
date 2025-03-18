using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogInteraction : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject npcSpeechBubble;
    public TextMeshProUGUI npcText;
    public Button dialogCloseButton;
    public TextMeshProUGUI npcNameplate;
    public Transform floatingDialog;
    public Transform headPositionAnchor;

    [Header("Dialog Icons")]
    public GameObject DialogTriggerButtonCenter;
    public GameObject DialogTriggerButtonCorner;

    [Header("Position Settings")]
    private Vector3 originalDialogPosition;
    private bool hasMoved = false;

    [Header("Dialog Settings")]
    public bool canRepeatDialog = true; // ✅ Allow replaying dialog
    private bool hasPlayed = false;     // ✅ Track if dialog has been played

    [Header("NPC Data")]
    public NPCDialogData npcData;
    private int currentLineIndex = 0;

    [Header("Audio Manager")]
    private DialogAudioManager audioManager;
    private DialogTextAnimator textAnimator;

    private Coroutine jumpRoutine;
    private Coroutine pulseRoutine;

    public string eventToTriggerOnClose; // ✅ Event to trigger when dialog closes

    [Header("Visibility Settings")]
    public string activationEvent; // ✅ The event that makes this visible


    void LateUpdate()
    {
        if (DialogTriggerButtonCenter != null && headPositionAnchor != null)
        {
            // ✅ Adjust only the X & Z position to follow the player, but keep the Y movement for bouncing
            Vector3 targetPos = DialogTriggerButtonCenter.transform.position;
            targetPos.x = headPositionAnchor.position.x;
            targetPos.z = headPositionAnchor.position.z;

            DialogTriggerButtonCenter.transform.position = targetPos; // ✅ Keeps bounce effect!
        }
    }



    void Start()
    {
        if (npcSpeechBubble == null || npcText == null || dialogCloseButton == null)
        {
            Debug.LogError("⚠️ NPCInteraction: Missing UI elements in Inspector!");
            return;
        }

        originalDialogPosition = floatingDialog.position;
        npcSpeechBubble.SetActive(false);
        dialogCloseButton.gameObject.SetActive(false);

        if (DialogTriggerButtonCenter != null) DialogTriggerButtonCenter.SetActive(true);
        if (DialogTriggerButtonCorner != null) DialogTriggerButtonCorner.SetActive(false);

        if (npcData != null) npcNameplate.text = npcData.npcName;

        audioManager = GetComponent<DialogAudioManager>() ?? gameObject.AddComponent<DialogAudioManager>();
        textAnimator = GetComponent<DialogTextAnimator>() ?? gameObject.AddComponent<DialogTextAnimator>();

        // ✅ Ensure we don't add multiple listeners
        if (DialogTriggerButtonCorner != null)
        {
            Button btn = DialogTriggerButtonCorner.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners(); // ✅ Remove duplicate listeners
                btn.onClick.AddListener(ShowNextDialogLine); // ✅ Ensure only one event is assigned
            }
        }

        if (!string.IsNullOrEmpty(activationEvent))
        {
            EventManager.Subscribe(activationEvent, showDialogAutomatically);
        }

        jumpRoutine = StartCoroutine(BounceDialogButton());
    }

    public void showDialogAutomatically()
    {
        Debug.LogError("🔥 ShowDialogAutomatically() CALLED!");

        if (npcSpeechBubble == null)
        {
            Debug.LogError("❌ npcSpeechBubble is NULL! Cannot show dialog.");
            return;
        }

        npcSpeechBubble.SetActive(true);

        if (npcData != null)
        {
            npcNameplate.text = npcData.npcName;
        }

        if (DialogTriggerButtonCenter != null)
            DialogTriggerButtonCenter.SetActive(true);

        if (DialogTriggerButtonCorner != null)
            DialogTriggerButtonCorner.SetActive(false);

        ShowNextDialogLineAutomatically();
    }

    private void ShowNextDialogLineAutomatically()
    {
        if (currentLineIndex >= npcData.dialogLines.Length)
        {
            CloseDialog();
            return;
        }

        string lineText = npcData.dialogLines[currentLineIndex].text;
        AudioClip voiceClip = npcData.dialogLines[currentLineIndex].voiceClip;

        Debug.Log($"🎤 Playing Line {currentLineIndex}: {lineText}");

        // ✅ Use DialogTextAnimator to animate text
        if (textAnimator != null)
        {
            textAnimator.AnimateText(npcText, lineText);
        }
        else
        {
            npcText.text = lineText; // Fallback if no animator exists
        }

        // ✅ Play voice audio
        if (audioManager != null && voiceClip != null)
        {
            audioManager.PlayVoiceClip(voiceClip);
        }

        // ✅ Move to the next line after text/audio finishes
        StartCoroutine(WaitForTextAndAudio(voiceClip));
    }

    private IEnumerator WaitForTextAndAudio(AudioClip voiceClip)
    {
        // ✅ Step 1: Ensure text animation finishes
        if (textAnimator != null)
        {
            while (textAnimator.IsPlaying()) // Wait until text animation completes
            {
                yield return null;
            }
        }

        // ✅ Step 2: Ensure voice clip plays and finishes
        if (voiceClip != null)
        {
            float audioDuration = voiceClip.length;

            if (audioManager != null && audioManager.IsPlaying())
            {
                while (audioManager.IsPlaying()) // 🔥 Wait for audio to finish
                {
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(audioDuration); // Fallback wait
            }
        }

        // ✅ Step 3: Move to the next line or close the dialog
        currentLineIndex++;

        if (currentLineIndex < npcData.dialogLines.Length)
        {
            Debug.Log($"🔄 Moving to next line: {currentLineIndex}");
            ShowNextDialogLineAutomatically();
        }
        else
        {
            Debug.Log("✅ Finished all dialog lines, closing dialog...");
            CloseDialog();

            if (!string.IsNullOrEmpty(eventToTriggerOnClose))
            {
                Debug.Log($"📢 Triggering event after dialog: '{eventToTriggerOnClose}'");
                EventManager.TriggerEvent(eventToTriggerOnClose);
            }
        }
    }

    private IEnumerator AnimatePopupAndShowText()
    {
        yield return AnimatePopup(); // ✅ Wait for the animation to complete
        ShowNextDialogLine();        // ✅ Then start the dialog text animation
    }

    public void OnNPCIconClick()
    {
        // ✅ Prevent replaying the entire conversation, but allow advancing it.
        if (!canRepeatDialog && hasPlayed && currentLineIndex == 0)
        {
            Debug.Log("🔒 Dialog already played and cannot be restarted!");
            return;
        }

        hasPlayed = true; // ✅ Mark the dialog as played only when starting

        if (!hasMoved)
        {
            hasMoved = true;
        }

        if (DialogTriggerButtonCenter != null) DialogTriggerButtonCenter.SetActive(false);
        if (DialogTriggerButtonCorner != null) DialogTriggerButtonCorner.SetActive(true);

        if (jumpRoutine != null)
        {
            StopCoroutine(jumpRoutine);
            jumpRoutine = null;
        }

        if (pulseRoutine == null)
        {
            pulseRoutine = StartCoroutine(PulseButtonEffect());
        }

        if (!npcSpeechBubble.activeSelf)
        {
            StartCoroutine(AnimatePopup());
        }

        // ✅ Ensure dialog only starts if there is content
        if (npcData != null && npcData.dialogLines.Length > 0)
        {
            ShowNextDialogLine(); // ✅ Always allow showing the next line
        }
    }
    private void ShowNextDialogLine()
    {
        if (DialogTriggerButtonCorner != null)
        {
            DialogTriggerButtonCorner.GetComponent<Button>().interactable = false; // ⏳ Temporarily disable button
            StartCoroutine(EnableButtonAfterDelay(DialogTriggerButtonCorner, 0.2f)); // ✅ Prevent double clicks
        }

        if (currentLineIndex >= npcData.dialogLines.Length)
        {
            CloseDialog();
            return;
        }

        string lineText = npcData.dialogLines[currentLineIndex].text;
        AudioClip voiceClip = npcData.dialogLines[currentLineIndex].voiceClip;

        // ✅ Animate Text
        if (textAnimator != null)
        {
            textAnimator.AnimateText(npcText, lineText);
        }
        else
        {
            npcText.text = lineText; // Fallback if no animator
        }

        // ✅ Play voice audio
        if (audioManager != null && voiceClip != null)
        {
            audioManager.PlayVoiceClip(voiceClip);
        }

        currentLineIndex++;
    }

    // ✅ Helper method to re-enable the button after a short delay
    private IEnumerator EnableButtonAfterDelay(GameObject button, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (button != null)
        {
            button.GetComponent<Button>().interactable = true;
        }
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

        floatingDialog.position = originalDialogPosition;
        hasMoved = false;

        // ✅ Hide the corner button when the dialog is finished
        if (DialogTriggerButtonCorner != null)
        {
            DialogTriggerButtonCorner.SetActive(false);
        }

        // ✅ Show the center button only if the dialog can be repeated
        if (canRepeatDialog && DialogTriggerButtonCenter != null)
        {
            DialogTriggerButtonCenter.SetActive(true);
            if (headPositionAnchor!=null){
                DialogTriggerButtonCenter.transform.position = headPositionAnchor.position;
            }
            
        }

        jumpRoutine = StartCoroutine(BounceDialogButton());

        // ✅ Fire the event to show the Trigger Zone after closing
        if (!string.IsNullOrEmpty(eventToTriggerOnClose))
        {
            Debug.Log($"📢 Trying to trigger event: '{eventToTriggerOnClose}'"); // Add Debug
            EventManager.TriggerEvent(eventToTriggerOnClose);
        }
        else
        {
            Debug.LogError("❌ eventToTriggerOnClose is EMPTY! Cannot trigger event.");
        }

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

        DialogTriggerButtonCorner.transform.localScale = endScale;
    }
}