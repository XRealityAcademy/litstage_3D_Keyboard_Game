using UnityEngine;
using TMPro;

public class SpaceElement : MonoBehaviour
{
    public string messageText = "Hello, you clicked a SpaceElement!";
    public GameObject messagePanel; // This is a small panel that appears above the SpaceElement
    public TextMeshProUGUI messageTextComponent;

    private Coroutine hideMessageCoroutine; // To auto-hide the message

    public void DisplayMessage()
    {
        if (messagePanel != null && messageTextComponent != null)
        {
            messagePanel.SetActive(true);
            messageTextComponent.text = messageText;

            // Stop previous coroutine if it's running
            if (hideMessageCoroutine != null)
            {
                StopCoroutine(hideMessageCoroutine);
            }

            // Hide the message after 3 seconds
            hideMessageCoroutine = StartCoroutine(HideMessageAfterSeconds(3));
        }
        Debug.Log("📢 Displaying Message: " + messageText);
    }

    private System.Collections.IEnumerator HideMessageAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        messagePanel.SetActive(false);
    }
}