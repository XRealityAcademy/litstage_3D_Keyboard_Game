using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public DialogManager dialogManager;
    public NPCDialogData npcDialog;
    private int currentLine = 0;
    private bool isPlayerNearby = false;

    public AudioSource audioSource;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key pressed near NPC!");

            if (dialogManager == null || npcDialog == null || npcDialog.dialogLines.Length == 0)
            {
                Debug.LogError("DialogManager or NPCDialogData is not assigned properly!");
                return;
            }

            Debug.Log($"Current Line: {currentLine}, Dialog Length: {npcDialog.dialogLines.Length}");

            if (currentLine >= npcDialog.dialogLines.Length)
            {
                Debug.Log("Conversation finished. Hiding dialog.");
                dialogManager.HideDialog();
                return;
            }

            if (dialogManager.IsTyping)
            {
                Debug.Log("Skipping typing effect");
                dialogManager.SkipTyping();
            }
            else
            {
                Debug.Log("Showing next line...");
                ShowNextLine();
            }
        }
    }

    private void ShowNextLine()
    {
        if (npcDialog == null || npcDialog.dialogLines.Length == 0)
        {
            Debug.LogError("NPCDialogData is missing or empty!");
            return;
        }

        if (currentLine >= npcDialog.dialogLines.Length)
        {
            Debug.Log("End of conversation. Hiding dialog.");
            dialogManager.HideDialog();
            return;
        }

        Debug.Log($"Displaying line {currentLine}: {npcDialog.dialogLines[currentLine].text}");
        dialogManager.ShowDialog(npcDialog.dialogLines[currentLine].text);

        // Play the voice-over audio
        if (npcDialog.dialogLines[currentLine].voiceClip != null && audioSource != null)
        {
            audioSource.Stop(); // Stop any currently playing audio
            audioSource.PlayOneShot(npcDialog.dialogLines[currentLine].voiceClip);
        }

        currentLine++;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered trigger: " + other.name); // Debug log

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered NPC area!"); // Log when player is detected
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            currentLine = 0; 
            dialogManager.HideDialog();
        }
    }
}