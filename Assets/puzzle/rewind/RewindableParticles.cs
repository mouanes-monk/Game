using UnityEngine;
using System.Collections;

public class RewindableParticles : MonoBehaviour
{
    [Header("Rewind Settings")]
    public float rewindSpeedMultiplier = 2f;
    
    [Header("Debug")]
    public bool showDebug = true;

    private ParticleSystem ps;
    private float originalSpeed;
    private bool isRewinding;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        originalSpeed = ps.main.simulationSpeed;
        if (showDebug) Debug.Log($"[Particles] Initialized with speed {originalSpeed}", this);
    }

    public void StartRewind()
    {
        if (!isRewinding)
        {
            StartCoroutine(RewindCoroutine());
        }
    }

    IEnumerator RewindCoroutine()
    {
        isRewinding = true;
        if (showDebug) Debug.Log("[Particles] Starting rewind", this);

        var main = ps.main;
        main.simulationSpeed = -originalSpeed * rewindSpeedMultiplier;
        ps.Play();

        // Calculate rewind duration based on particle lifetime
        float rewindDuration = ps.main.duration * 2f; // Extra buffer time

        float elapsed = 0f;
        while (elapsed < rewindDuration)
        {
            elapsed += Time.deltaTime;
            
            // Visual progress indicator
            if (showDebug && elapsed % 0.5f < Time.deltaTime)
                Debug.Log($"[Particles] Rewind progress: {elapsed/rewindDuration:P0}", this);

            yield return null;
        }

        main.simulationSpeed = originalSpeed;
        isRewinding = false;
        if (showDebug) Debug.Log("[Particles] Rewind complete", this);
    }
}