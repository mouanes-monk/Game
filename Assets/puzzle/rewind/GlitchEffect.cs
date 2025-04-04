using UnityEngine;

public class GlitchEffect : MonoBehaviour
{
    public Material glitchMaterial; // Assign a glitch shader material in the Inspector
    private bool isGlitching = false;

    void Start()
    {
        if (glitchMaterial != null)
            glitchMaterial.SetFloat("_GlitchIntensity", 0f); // Start with no glitch
    }

    // Called when rewind starts
    public void StartRewind()
    {
        if (!isGlitching && glitchMaterial != null)
        {
            isGlitching = true;
            glitchMaterial.SetFloat("_GlitchIntensity", 1f); // Apply glitch effect
        }
    }

    // Called when rewind ends
    public void StopRewind()
    {
        if (isGlitching && glitchMaterial != null)
        {
            isGlitching = false;
            glitchMaterial.SetFloat("_GlitchIntensity", 0f); // Remove glitch effect
        }
    }
}
