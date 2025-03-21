using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueHandler
{
    public List<DialogueData> dialogueReferences; // Direct references to DialogueData assets

    public DialogueData GetDialogueById(string id)
    {
        foreach (DialogueData dialogue in dialogueReferences)
        {
            if (dialogue.dialogueId.Equals(id))
            {
                return dialogue;
            }
        }
        return null; // No matching dialogue found
    }
}