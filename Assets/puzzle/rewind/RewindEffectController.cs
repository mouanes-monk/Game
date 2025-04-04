using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class RewindEffectController : MonoBehaviour
{
    [Header("Volume Settings")]
    public Volume postProcessVolume;
    [Range(-100, 100)] public float rewindSaturation = -80f;
    public Color rewindTint = new Color(0.8f, 0.8f, 1f);
    public float fadeDuration = 0.3f;

    [Header("Debug")]
    public bool showDebugLogs = true;

    private ColorAdjustments colorAdjustments;
    private float defaultSaturation;
    private Color defaultTint;
    private Coroutine activeCoroutine;

    void Start()
    {
        InitializeEffects();
    }

    void InitializeEffects()
    {
        if (postProcessVolume == null)
        {
            Debug.LogError("PostProcess Volume not assigned!", this);
            return;
        }

        if (!postProcessVolume.profile.TryGet(out colorAdjustments))
        {
            Debug.LogError("ColorAdjustments not found in volume profile!", this);
            return;
        }

        defaultSaturation = colorAdjustments.saturation.value;
        defaultTint = colorAdjustments.colorFilter.value;

        if (showDebugLogs)
            Debug.Log($"Effect controller initialized. Default saturation: {defaultSaturation}", this);
    }

    public void StartRewindEffects()
    {
        if (colorAdjustments == null)
        {
            Debug.LogWarning("Cannot start effects - ColorAdjustments not available", this);
            return;
        }

        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(LerpEffects(rewindSaturation, rewindTint, "Start"));
        
        if (showDebugLogs)
            Debug.Log("Starting rewind effects", this);
    }

    public void StopRewindEffects()
    {
        if (colorAdjustments == null)
        {
            Debug.LogWarning("Cannot stop effects - ColorAdjustments not available", this);
            return;
        }

        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(LerpEffects(defaultSaturation, defaultTint, "Stop"));
        
        if (showDebugLogs)
            Debug.Log("Stopping rewind effects", this);
    }

    IEnumerator LerpEffects(float targetSat, Color targetTint, string operation)
    {
        float elapsed = 0f;
        float startSat = colorAdjustments.saturation.value;
        Color startTint = colorAdjustments.colorFilter.value;

        if (showDebugLogs)
            Debug.Log($"{operation} effects - Current: {startSat}, Target: {targetSat}", this);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            colorAdjustments.saturation.value = Mathf.Lerp(startSat, targetSat, t);
            colorAdjustments.colorFilter.value = Color.Lerp(startTint, targetTint, t);
            
            if (showDebugLogs && elapsed % 0.5f < Time.deltaTime)
                Debug.Log($"{operation} progress: {t:P0}", this);

            yield return null;
        }

        // Ensure exact values
        colorAdjustments.saturation.value = targetSat;
        colorAdjustments.colorFilter.value = targetTint;

        if (showDebugLogs)
            Debug.Log($"{operation} effects complete", this);
    }
}