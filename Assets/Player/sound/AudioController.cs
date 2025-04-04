using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource mainTheme;
    public AudioSource rewindSound;
    
    private void Start() => rewindSound.Stop();

    public void PlayRewind()
    {
        if (mainTheme.isPlaying)
            mainTheme.Pause();
        
        rewindSound.Play();
        Invoke(nameof(ResumeMainTheme), rewindSound.clip.length);
    }

    public void StopRewind()
    {
        rewindSound.Stop();
        ResumeMainTheme();
    }

    private void ResumeMainTheme()
    {
        if (!mainTheme.isPlaying)
            mainTheme.Play();
    }
}