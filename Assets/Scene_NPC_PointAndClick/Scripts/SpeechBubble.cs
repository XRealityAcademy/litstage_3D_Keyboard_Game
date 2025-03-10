using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public TextMeshProUGUI bubbleText;  // Reference to the text
    public Button bubbleButton;         // Reference to the button

    void Start()
    {
        bubbleText.alpha = 0; // Hide text initially
        bubbleButton.onClick.AddListener(ShowMessage);
    }

    void ShowMessage()
    {
        bubbleText.alpha = 1; // Make text visible
        bubbleText.text = "Hello, traveler!"; // Set message text
    }

    public void DisplayText()
    {
        bubbleText.gameObject.SetActive(true);
        bubbleText.text = "Hello, traveler!"; // Example text
    }
}