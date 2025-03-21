using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSceneDialogueData", menuName = "Dialogue/SceneDialogueData")]
public class SceneDialogueData : ScriptableObject
{
    public string sceneName;
    public List<CharacterData> characters; // Holds all characters in the scene
}