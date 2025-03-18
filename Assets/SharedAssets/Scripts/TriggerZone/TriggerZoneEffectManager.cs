using System.Collections;
using System.Diagnostics;
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
        UnityEngine.Debug.Log($"🔍 Start() called on {gameObject.name} (Instance ID: {gameObject.GetInstanceID()}) at {Time.time} seconds", this);

        // ✅ Check if it's already disabled (prevents overriding DisableEffects())
        if (!effectsActive)
        {
            UnityEngine.Debug.Log($"🚫 {gameObject.name} was initialized with effects disabled. Skipping Start() activation.");
            return; // 🚀 Exit early so it doesn't enable effects again
        }

        if (triggerLight != null)
        {
            triggerLight.enabled = true;
            UnityEngine.Debug.Log($"💡 Light ENABLED on {gameObject.name}");
        }

        if (expandingEffect != null)
        {
            expandingEffect.StartEffect();
            UnityEngine.Debug.Log($"🌟 Expanding effect STARTED on {gameObject.name}");
        }
    }

    public void TriggerEffects()
    {
        if (effectsActive)
        {
            UnityEngine.Debug.Log($"⚠️ [{gameObject.name}] TriggerEffects() was called, but effects are already active. Skipping...");
            return; // 🚫 Prevents running multiple times
        }

        effectsActive = true; // ✅ Set active state before running

        UnityEngine.Debug.Log($"🔥 [{gameObject.name}] Activating Effects...");

        if (expandingEffect != null && !expandingEffect.gameObject.activeSelf)
        {
            expandingEffect.gameObject.SetActive(true);
            expandingEffect.StartEffect();
            UnityEngine.Debug.Log($"✅ Expanding effect ENABLED on {gameObject.name}");
        }

        if (colorEffect != null && !colorEffect.gameObject.activeSelf)
        {
            colorEffect.gameObject.SetActive(true);
            UnityEngine.Debug.Log($"🎨 Color effect ENABLED on {gameObject.name}");
        }

        if (triggerParticles != null && !triggerParticles.isPlaying)
        {
            triggerParticles.Play();
            UnityEngine.Debug.Log($"💨 Particles PLAYING on {gameObject.name}");
        }

        if (triggerLight != null && !triggerLight.enabled)
        {
            triggerLight.enabled = true;
            UnityEngine.Debug.Log($"💡 Light TURNED ON on {gameObject.name}");
        }
    }

    public void DisableEffects()
    {
        if (!effectsActive)
        {
            UnityEngine.Debug.LogWarning($"⚠️ [{gameObject.name}] DisableEffects() called, but effects are already disabled. Skipping...");
            return;
        }

        effectsActive = false; // ✅ Reset active state

        UnityEngine.Debug.Log($"🔴 [{gameObject.name}] Deactivating All Effects...");

        if (expandingEffect != null)
        {
            expandingEffect.gameObject.SetActive(false);
            UnityEngine.Debug.Log($"❌ Expanding effect DISABLED on {gameObject.name}");
        }

        if (colorEffect != null)
        {
            colorEffect.gameObject.SetActive(false);
            UnityEngine.Debug.Log($"🎨 Color effect DISABLED on {gameObject.name}");
        }

        if (triggerParticles != null && triggerParticles.isPlaying)
        {
            triggerParticles.Stop();
            UnityEngine.Debug.Log($"💨 Particles STOPPED on {gameObject.name}");
        }

        if (triggerLight != null && triggerLight.enabled)
        {
            triggerLight.enabled = false;
            UnityEngine.Debug.Log($"💡 Light TURNED OFF on {gameObject.name}");
        }
    }

    private IEnumerator EnsureLightStaysOff()
    {
        yield return new WaitForSeconds(0.1f); // ✅ Wait a short delay

        if (triggerLight != null && triggerLight.enabled)
        {
            UnityEngine.Debug.LogWarning($"⚠️ triggerLight was re-enabled! Forcing it OFF again.");
            triggerLight.enabled = false; // ✅ Force it OFF again
        }
    }
}