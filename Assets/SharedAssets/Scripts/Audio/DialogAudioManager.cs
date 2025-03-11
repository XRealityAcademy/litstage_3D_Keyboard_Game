using UnityEngine;

public class DialogAudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // ✅ Ensure it doesn’t auto-play
        audioSource.loop = false;
    }

    /// <summary>
    /// Plays a given audio clip for the dialog.
    /// </summary>
    public void PlayVoiceClip(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.Stop(); // ✅ Stop previous clip before playing new one
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("⚠️ No audio clip assigned for this dialog line!");
        }
    }

    /// <summary>
    /// Stops any currently playing voice.
    /// </summary>
    public void StopVoice()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}