using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [Header("Dialogue ID")]
    public string dialogueId; // Unique identifier for this dialogue

    [Header("Dialogue Content")]
    public DialogueLine[] dialogueLines; // Array of text + audio

    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(3, 10)]
        public string text;
        public AudioClip voiceClip;
    }
}