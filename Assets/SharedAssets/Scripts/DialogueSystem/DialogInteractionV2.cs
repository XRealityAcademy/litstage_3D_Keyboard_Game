using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class DialogInteractionV2 : MonoBehaviour
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
    public bool canRepeatDialog = true;
    private bool hasPlayed = false;

    [Header("Scene Data")]
    public SceneDialogueData sceneData;
    public string characterName;
    public string dialogueId;

    private int currentLineIndex = 0;
    private DialogueData currentDialogue;

    [Header("Audio Manager")]
    private DialogAudioManager audioManager;
    private DialogTextAnimator textAnimator;

    private Coroutine jumpRoutine;
    private Coroutine pulseRoutine;

    public string eventToTriggerOnClose;

    [Header("Visibility Settings")]
    public string activationEvent;

    void LateUpdate()
    {
        if (DialogTriggerButtonCenter != null && headPositionAnchor != null)
        {
            Vector3 targetPos = DialogTriggerButtonCenter.transform.position;
            targetPos.x = headPositionAnchor.position.x;
            targetPos.z = headPositionAnchor.position.z;
            DialogTriggerButtonCenter.transform.position = targetPos;
        }
    }

    void Start()
    {
        if (npcSpeechBubble == null || npcText == null || dialogCloseButton == null)
        {
            Debug.LogError("⚠️ DialogInteractionV2: Missing UI elements in Inspector!");
            return;
        }

        originalDialogPosition = floatingDialog.position;
        npcSpeechBubble.SetActive(false);
        dialogCloseButton.gameObject.SetActive(false);

        if (DialogTriggerButtonCenter != null) DialogTriggerButtonCenter.SetActive(true);
        if (DialogTriggerButtonCorner != null) DialogTriggerButtonCorner.SetActive(false);

        audioManager = GetComponent<DialogAudioManager>() ?? gameObject.AddComponent<DialogAudioManager>();
        textAnimator = GetComponent<DialogTextAnimator>() ?? gameObject.AddComponent<DialogTextAnimator>();

        LoadDialogueData();

        if (!string.IsNullOrEmpty(activationEvent))
        {
            EventManager.Subscribe(activationEvent, showDialogAutomatically);
        }

        jumpRoutine = StartCoroutine(BounceDialogButton());
    }

    private void LoadDialogueData()
    {
        if (sceneData == null)
        {
            Debug.LogError("❌ SceneDialogueData is not assigned!");
            return;
        }

        CharacterData character = sceneData.characters.FirstOrDefault(c => c.characterName == characterName);

        if (character == null)
        {
            Debug.LogError($"❌ Character '{characterName}' not found in SceneDialogueData!");
            return;
        }

        currentDialogue = character.dialogueHandler.GetDialogueById(dialogueId);

        if (currentDialogue == null)
        {
            Debug.LogError($"❌ Dialogue ID '{dialogueId}' not found for '{characterName}'!");
            return;
        }

        npcNameplate.text = characterName;
        //Debug.Log($"✅ Loaded dialogue '{dialogueId}' for '{characterName}'");
    }

    public void showDialogAutomatically()
    {
        if (npcSpeechBubble == null)
        {
            Debug.LogError("❌ npcSpeechBubble is NULL! Cannot show dialog.");
            return;
        }

        npcSpeechBubble.SetActive(true);

        if (currentDialogue != null)
        {
            npcNameplate.text = characterName;
        }

        if (DialogTriggerButtonCenter != null)
            DialogTriggerButtonCenter.SetActive(true);

        if (DialogTriggerButtonCorner != null)
            DialogTriggerButtonCorner.SetActive(false);

        ShowNextDialogLineAutomatically();
    }

    private void ShowNextDialogLineAutomatically()
    {
        if (currentDialogue == null || currentLineIndex >= currentDialogue.dialogueLines.Length)
        {
            CloseDialog();
            return;
        }

        string lineText = currentDialogue.dialogueLines[currentLineIndex].text;
        AudioClip voiceClip = currentDialogue.dialogueLines[currentLineIndex].voiceClip;

        Debug.Log($"🎤 Playing Line {currentLineIndex}: {lineText}");

        if (textAnimator != null)
        {
            textAnimator.AnimateText(npcText, lineText);
        }
        else
        {
            npcText.text = lineText;
        }

        if (audioManager != null && voiceClip != null)
        {
            audioManager.PlayVoiceClip(voiceClip);
        }

        StartCoroutine(WaitForTextAndAudio(voiceClip));
    }

    private IEnumerator WaitForTextAndAudio(AudioClip voiceClip)
    {
        if (textAnimator != null)
        {
            while (textAnimator.IsPlaying())
            {
                yield return null;
            }
        }

        if (voiceClip != null)
        {
            float audioDuration = voiceClip.length;
            if (audioManager != null && audioManager.IsPlaying())
            {
                while (audioManager.IsPlaying())
                {
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(audioDuration);
            }
        }

        currentLineIndex++;

        if (currentLineIndex < currentDialogue.dialogueLines.Length)
        {
            ShowNextDialogLineAutomatically();
        }
        else
        {
            CloseDialog();

            if (!string.IsNullOrEmpty(eventToTriggerOnClose))
            {
                EventManager.TriggerEvent(eventToTriggerOnClose);
            }
        }
    }

    public void OnNPCIconClick()
    {
        if (!canRepeatDialog && hasPlayed && currentLineIndex == 0)
        {
            return;
        }

        hasPlayed = true;

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

        ShowNextDialogLine();
    }

    private void ShowNextDialogLine()
    {
        if (DialogTriggerButtonCorner != null)
        {
            DialogTriggerButtonCorner.GetComponent<Button>().interactable = false;
            StartCoroutine(EnableButtonAfterDelay(DialogTriggerButtonCorner, 0.2f));
        }

        if (currentDialogue == null || currentLineIndex >= currentDialogue.dialogueLines.Length)
        {
            CloseDialog();
            return;
        }

        string lineText = currentDialogue.dialogueLines[currentLineIndex].text;
        AudioClip voiceClip = currentDialogue.dialogueLines[currentLineIndex].voiceClip;

        if (textAnimator != null)
        {
            textAnimator.AnimateText(npcText, lineText);
        }
        else
        {
            npcText.text = lineText;
        }

        if (audioManager != null && voiceClip != null)
        {
            audioManager.PlayVoiceClip(voiceClip);
        }

        currentLineIndex++;
    }

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
            if (headPositionAnchor != null)
            {
                DialogTriggerButtonCenter.transform.position = headPositionAnchor.position;
            }

        }

        jumpRoutine = StartCoroutine(BounceDialogButton());

        // ✅ Fire the event to show the Trigger Zone after closing
        if (!string.IsNullOrEmpty(eventToTriggerOnClose))
        {
//            Debug.Log($"📢 Trying to trigger event: '{eventToTriggerOnClose}'"); // Add Debug
            EventManager.TriggerEvent(eventToTriggerOnClose);
        }
        else
        {
            //Debug.LogError("❌ eventToTriggerOnClose is EMPTY! Cannot trigger event.");
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