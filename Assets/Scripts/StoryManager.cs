using UnityEngine;
using TMPro;

public class StoryManager : MonoBehaviour
{
    public StoryData storyData; // Reference to scriptable object
    public TextMeshProUGUI storyText;
    private int currentIndex = 0;

    void Start()
    {
        if (storyData == null || storyData.storyLines.Length == 0)
        {
            Debug.LogError("StoryData is missing or empty!"); // Debug error message
            return;
        }

        DisplayStory();
    }

    public void NextLine()
    {
        if (currentIndex < storyData.storyLines.Length - 1)
        {
            currentIndex++;
            DisplayStory();
        }
        else
        {
            Debug.Log("End of story reached.");
        }
    }

    private void DisplayStory()
    {
        if (storyData == null || storyData.storyLines.Length == 0)
        {
            Debug.LogError("StoryData is missing or empty!"); // Debug error
            return;
        }

        if (currentIndex >= storyData.storyLines.Length)
        {
            Debug.LogError($"Invalid index: {currentIndex}. Story has only {storyData.storyLines.Length} lines.");
            return;
        }

        storyText.text = storyData.storyLines[currentIndex];
    }
}