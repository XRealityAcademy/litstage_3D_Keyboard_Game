using UnityEngine;
using TMPro;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogUI;
    public TextMeshProUGUI dialogText;
    public float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;
    public bool IsTyping { get; private set; } = false; // ✅ Track whether typing is active
    private string currentMessage = "";

    void Start()
    {
        dialogUI.SetActive(false);
    }

    public void ShowDialog(string message)
    {
        dialogUI.SetActive(true);

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        currentMessage = message;
        typingCoroutine = StartCoroutine(TypeText(message));
    }

    IEnumerator TypeText(string message)
    {
        IsTyping = true; // ✅ Set typing status to active
        dialogText.text = "";

        foreach (char letter in message.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        IsTyping = false; // ✅ Set typing status to finished
    }

    public void SkipTyping()
    {
        if (IsTyping)
        {
            IsTyping = false; // ✅ Stop typing and display the full message
            dialogText.text = currentMessage;
        }
    }

    public void HideDialog()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogText.text = "";
        IsTyping = false;
        dialogUI.SetActive(false);
    }
}