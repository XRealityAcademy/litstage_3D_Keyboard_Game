using UnityEngine;

public class TriggerZoneEffectManager : MonoBehaviour
{
    [Header("Effect References")]
    public TriggerZoneEffect expandingEffect;
    public TriggerZoneColor colorEffect;
    public ParticleSystem triggerParticles;
    public Light triggerLight;

    private bool effectsActive = true; // Track if effects are active

    void Start()
    {
        if (triggerLight != null)
        {
            triggerLight.enabled = true; // ✅ Ensure light starts enabled
        }

        if (expandingEffect != null)
        {
            expandingEffect.StartEffect(); // ✅ Start expanding only for this instance
        }
    }

    public void TriggerEffects()
    {
        if (!effectsActive) return; // ✅ Prevent multiple triggers
        effectsActive = false;

        Debug.Log($"🔥 [{gameObject.name}] Activating Effects...");

        expandingEffect?.StopEffect(); // ✅ Stop only THIS expanding effect
        colorEffect?.DisableColorChange(); // ✅ Disable only THIS color effect

        if (triggerParticles != null)
        {
            triggerParticles.Play(); // ✅ Play only THIS particle effect
        }

        if (triggerLight != null)
        {
            triggerLight.enabled = false; // ✅ Turn off only THIS light
        }
    }

    public void DisableEffects()
    {
        if (!effectsActive) return; // ✅ Ensure effects are only disabled once

//        Debug.Log($"🔴 [{gameObject.name}] Deactivating All Effects...");

        expandingEffect?.gameObject.SetActive(false); // ✅ Deactivate ONLY its own effect
        colorEffect?.gameObject.SetActive(false); // ✅ Deactivate ONLY its own color effect

        if (triggerParticles != null)
        {
            triggerParticles.Stop();
        }

        if (triggerLight != null)
        {
            triggerLight.enabled = false;
        }
    }
}