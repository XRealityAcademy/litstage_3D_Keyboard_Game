using System.Collections;
using UnityEngine;
using TMPro;

public class DialogTextAnimator : MonoBehaviour
{
    private Coroutine typingCoroutine;
    private bool isPlaying = false; // ✅ Track animation state

    /// <summary>
    /// Starts the typewriter effect for the given text.
    /// </summary>
    public void AnimateText(TextMeshProUGUI textComponent, string fullText, float speed = 0.05f)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // ✅ Stop any existing animation
        }
        typingCoroutine = StartCoroutine(TypeText(textComponent, fullText, speed));
    }

    /// <summary>
    /// Coroutine to display text letter by letter.
    /// </summary>
    private IEnumerator TypeText(TextMeshProUGUI textComponent, string fullText, float speed)
    {
        isPlaying = true; // ✅ Mark animation as active
        textComponent.text = "";

        foreach (char letter in fullText)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(speed); // ✅ Controls typing speed
        }

        isPlaying = false; // ✅ Mark animation as finished
    }

    /// <summary>
    /// Instantly shows the full text (skips animation).
    /// </summary>
    public void ShowFullText(TextMeshProUGUI textComponent, string fullText)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        textComponent.text = fullText;
        isPlaying = false; // ✅ Ensure it's marked as finished
    }

    /// <summary>
    /// Checks if text animation is still playing.
    /// </summary>
    public bool IsPlaying()
    {
        return isPlaying;
    }
}