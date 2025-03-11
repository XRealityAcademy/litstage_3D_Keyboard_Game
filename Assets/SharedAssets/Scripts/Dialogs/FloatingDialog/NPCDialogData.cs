using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialog", menuName = "Dialog/NPCDialog")]
public class NPCDialogData : ScriptableObject
{
    [Header("NPC Info")]
    public string npcName;
    [System.Serializable]
    public class DialogLine
    {
        [TextArea(3, 10)] public string text; // Stores dialog text
        public AudioClip voiceClip; // Stores corresponding audio file
    }

    [Header("Dialog Content")]
    public DialogLine[] dialogLines; // Array of text + audio
}