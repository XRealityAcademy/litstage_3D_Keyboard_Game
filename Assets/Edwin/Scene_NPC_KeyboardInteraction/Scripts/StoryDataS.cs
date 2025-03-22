using UnityEngine;

[CreateAssetMenu(fileName = "NewStory", menuName = "Story/Narrative")]
public class StoryDataS : ScriptableObject
{
    [TextArea(3, 10)] public string[] storyLines; // Stores multiple lines
}