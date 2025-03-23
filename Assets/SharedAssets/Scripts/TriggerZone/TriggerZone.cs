using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private Collider triggerCollider; // ✅ Store collider reference

    [Header("Event Settings")]
    public string eventToTrigger; // ✅ Custom event name set in Inspector
    public string eventToTriggerInmediately; // Custom event to trigger inmediatly the player come into the trigger zone
    public GameEvent eventObject; // (Optional) If using a `GameEvent` system


    [Header("Visibility Settings")]
    public bool startsHidden = true; // ✅ If true, this zone starts invisible
    public string activationEvent; // ✅ The event that makes this visible

    [Header("Scene Transition Settings")]
    public string nextSceneName; // ✅ Set this in the Inspector


    private void Start()
    {
//        Debug.Log($"🔻 TriggerZone ({gameObject.name}) listening for event: '{activationEvent}'");


        // Debug.Log("starting triggerZone");
        // ✅ Attach AudioSource dynamically
        audioSource = gameObject.AddComponent<AudioSource>();
        if (triggerSound != null)
        {
            audioSource.clip = triggerSound;
            audioSource.playOnAwake = false;
        }

        // ✅ Hide this TriggerZone if `startsHidden` is enabled
        if (startsHidden)
        {

            if (effectManager != null)
            {
//                Debug.LogError("🔻 DisableEffects effectManager ");
                effectManager.DisableEffects();
            }
            else
            {
                Debug.LogError("🔻 effectManager is null");
            }

            SetVisible(false); // ✅ Initially hidden

        }

        // ✅ Get Renderer & Collider references
        triggerRenderer = GetComponent<Renderer>();
        triggerCollider = GetComponent<Collider>();

        // ✅ **Disable TriggerZone Renderer on Start (Keep Effects Visible)**
        if (Application.isPlaying && triggerRenderer != null)
        {
            triggerRenderer.enabled = false; // ❌ Make sure it's NOT visible in-game
        }



        if (!string.IsNullOrEmpty(activationEvent))
        {
            EventManager.Subscribe(activationEvent, ShowTriggerZone);
        }
        else
        {
//            Debug.LogError($"❌ activationEvent is EMPTY for {gameObject.name}! It won't become visible.");
        }
    }

    private void OnDestroy()
    {
        if (!string.IsNullOrEmpty(activationEvent))
        {
            EventManager.Unsubscribe(activationEvent, ShowTriggerZone);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggerOnlyOnce && hasTriggered) return;

        hasTriggered = true;
//        Debug.Log($"✅ Player entered the trigger zone: {gameObject.name}");


        if (eventObject != null)
        {
            eventObject.StartEvent();
        }

        if (triggerSound != null)
        {
            Debug.Log($"🔊 Playing sound: {triggerSound.name}");

            // ✅ Detach the AudioSource from the trigger zone
            Transform audioParent = new GameObject("DetachedAudioSource").transform;
            audioParent.position = transform.position; // Keep the sound at the same position
            AudioSource detachedAudio = audioParent.gameObject.AddComponent<AudioSource>();

            // ✅ Copy the original AudioSource settings
            detachedAudio.clip = triggerSound;
            detachedAudio.volume = audioSource.volume;
            detachedAudio.spatialBlend = audioSource.spatialBlend;
            detachedAudio.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
            detachedAudio.Play();

            // ✅ Destroy the detached audio object after sound finishes
            Destroy(audioParent.gameObject, triggerSound.length);
        }

        if (triggerOnlyOnce)
        {
            HideTriggerZone(); // Hide it immediately

            if (triggerSound != null)
            {
                CoroutineHelper.Instance.StartCoroutineExternal(DisableAfterSound(triggerSound.length)); // ✅ Use helper
            }
            else
            {
                gameObject.SetActive(false);
                if (!string.IsNullOrEmpty(eventToTrigger))
                {
                    EventManager.TriggerEvent(eventToTrigger);
                }
            }
        }
        else
        {
            // ✅ Trigger event immediately ONLY IF there's no sound
            if (triggerSound == null && !string.IsNullOrEmpty(eventToTrigger))
            {
                EventManager.TriggerEvent(eventToTrigger);
            }
        }

        // ✅ Trigger event immediately ONLY IF there's no sound
        if (!string.IsNullOrEmpty(eventToTriggerInmediately))
        {
            EventManager.TriggerEvent(eventToTriggerInmediately);
        }

        // ✅ Change Scene when entering the TriggerZone
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"🌍 Changing scene to: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
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

        SetVisible(false); // ✅ Hide it
    }

    private void DisableTriggerZone()
    {
        if (triggerSound != null)
        {
            float soundDuration = triggerSound.length;
            Debug.Log($"⏳ Waiting {soundDuration} seconds before disabling {gameObject.name}");

            StartCoroutine(DisableAfterSound(soundDuration)); // ✅ Wait before disabling
        }
        else
        {
            gameObject.SetActive(false); // ✅ Disable immediately if no sound
        }
    }

    private IEnumerator DisableAfterSound(float delay)
    {
        Debug.Log($"⏳ Waiting {delay} seconds before disabling {gameObject.name}");
        yield return new WaitForSeconds(delay);

        Debug.Log($"🚫 Disabling Trigger Zone: {gameObject.name}");
        gameObject.SetActive(false);

        // ✅ Now trigger the event after sound finishes
        if (!string.IsNullOrEmpty(eventToTrigger))
        {
            Debug.Log($"🚀 Triggering event: {eventToTrigger}");
            EventManager.TriggerEvent(eventToTrigger);
        }
    }

    private void ShowTriggerZone() { 

//        Debug.Log($"🔓 Trigger Zone activated by event: {activationEvent}");

        SetVisible(true); // ✅ Make it visible when event fires
        if (effectManager != null) {
            effectManager.TriggerEffects();
        }
        
    }


    private void SetVisible(bool isVisible)
    {
        if (triggerRenderer != null)
            triggerRenderer.enabled = isVisible;

        if (triggerCollider != null)
            triggerCollider.enabled = isVisible;

        gameObject.SetActive(isVisible);
    }
}