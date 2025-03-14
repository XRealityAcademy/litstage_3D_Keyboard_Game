using UnityEngine;
using System.Collections;

public class TriggerZoneEffect : MonoBehaviour
{
    private Vector3 originalScale;
    private bool isExpanding = true;
    private float scaleAmount = 1.4f; // How much it expands
    private float speed = 2f; // Speed of effect
    private bool isPlayerInside = false;
    private bool isActive = false; // ✅ Default to inactive
    private Coroutine pulseRoutine;
    private Renderer zoneRenderer;

    [Header("Manager Reference")]
    public TriggerZoneEffectManager effectManager; // ✅ Assigned per instance

    void Start()
    {
        originalScale = transform.localScale; // Save initial size
        zoneRenderer = GetComponent<Renderer>();

        // ✅ Ensure each TriggerZoneEffect checks its assigned manager, not global
        if (effectManager != null)
        {
            HideMaterial(); // ❌ Hide if this effect is not controlled by its manager
        }

    }

    void Update()
    {
        if (!isActive || isPlayerInside) return; // ✅ Stop expanding if disabled or player inside

        float scaleFactor = isExpanding ? scaleAmount : 1f;
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale * scaleFactor, Time.deltaTime * speed);

        if (Mathf.Abs(transform.localScale.x - (originalScale.x * scaleFactor)) < 0.01f)
        {
            isExpanding = !isExpanding; // Switch direction
        }
    }

    private IEnumerator PulseEffect()
    {
        while (isActive)
        {
            if (isPlayerInside) yield return null;

            float time = 0;
            Vector3 startScale = transform.localScale;
            Vector3 targetScale = originalScale * scaleAmount;

            while (time < 1f / speed)
            {
                transform.localScale = Vector3.Lerp(startScale, targetScale, time * speed);
                time += Time.deltaTime;
                yield return null;
            }

            time = 0;
            startScale = transform.localScale;
            targetScale = originalScale;

            while (time < 1f / speed)
            {
                transform.localScale = Vector3.Lerp(startScale, targetScale, time * speed);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void StartEffect()
    {
        if (isActive) return;

        isActive = true;
        ShowMaterial(); // ✅ Show material when activated
        if (pulseRoutine == null)
        {
            pulseRoutine = StartCoroutine(PulseEffect());
        }
    }

    public void StopEffect()
    {
        isActive = false;
        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
            pulseRoutine = null;
        }
        transform.localScale = originalScale;
        HideMaterial(); // ✅ Hide material when deactivated
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            StopEffect();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isActive) return;
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            StartEffect();
        }
    }

    // ✅ **Helper Methods to Show/Hide Material**
    private void ShowMaterial()
    {
        if (zoneRenderer != null)
        {
            zoneRenderer.enabled = true;
        }
    }

    private void HideMaterial()
    {
        if (zoneRenderer != null)
        {
            zoneRenderer.enabled = false;
        }
    }
}