using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialog", menuName = "Dialog/NPCDialog")]
public class NPCDialogDataS : ScriptableObject
{
    [System.Serializable]
    public class DialogLine
    {
        [TextArea(3, 10)] public string text; // Stores dialog text
        public AudioClip voiceClip; // Stores corresponding audio file
    }

    public DialogLine[] dialogLines; // Array of text + audio
}