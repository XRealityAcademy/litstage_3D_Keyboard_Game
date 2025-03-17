using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [Header("Trigger Settings")]
    public bool triggerOnlyOnce = false; // If true, disable after first use
    private bool hasTriggered = false;

    [Header("Effects Manager")]
    public TriggerZoneEffectManager effectManager; // Reference to effects manager
    public AudioClip triggerSound;
    private AudioSource audioSource;
    private Renderer triggerRenderer; // ✅ Reference to TriggerZone Renderer

    private void Start()
    {
       // Debug.Log("starting triggerZone");
        // ✅ Attach AudioSource dynamically
        audioSource = gameObject.AddComponent<AudioSource>();
        if (triggerSound != null)
        {
            audioSource.clip = triggerSound;
            audioSource.playOnAwake = false;
        }

        // ✅ Get the Renderer of TriggerZone
        triggerRenderer = GetComponent<Renderer>();

        // ✅ **Disable TriggerZone Renderer on Start (Keep Effects Visible)**
        if (Application.isPlaying && triggerRenderer != null)
        {
            triggerRenderer.enabled = false; // ❌ Make sure it's NOT visible in-game
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggerOnlyOnce && hasTriggered) return;

        hasTriggered = true;
        Debug.Log("✅ Player entered the trigger zone: " + gameObject.name);

        // ✅ Play sound BEFORE hiding visuals
        if (triggerSound != null)
        {
            Debug.Log("Playing sound");
            audioSource.Play();
        }

        // ✅ **Only hide trigger zone if `triggerOnlyOnce` is TRUE**
        if (triggerOnlyOnce)
        {
            HideTriggerZone(); // ✅ Hide effects & disable collider

            if (triggerSound != null)
            {
                // ✅ Schedule full deactivation **ONLY** when `triggerOnlyOnce` is enabled
                Invoke(nameof(DisableTriggerZone), triggerSound.length);
            }
            else {
                DisableTriggerZone();
            }


        }
    }

    private void HideTriggerZone()
    {
        GetComponent<Collider>().enabled = false; // Disable trigger detection

        // ❌ **Don't touch the triggerRenderer (Keep Invisible)**
        if (effectManager != null)
        {
            effectManager.DisableEffects(); // ✅ Immediately stop effects (particles, lights, etc.)
        }
    }

    private void DisableTriggerZone()
    {
//        Debug.Log("🚫 Disabling Trigger Zone after sound finishes.");
        gameObject.SetActive(false); // ✅ Fully disable after audio completes
    }
}