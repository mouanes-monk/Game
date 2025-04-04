using UnityEngine;

public class ObstacleRewind : MonoBehaviour
{
    public AudioSource mainTheme;
    public AudioSource rewindSound;
    
    private PlayerRewind playerRewind;

    void Start()
    {
        playerRewind = FindObjectOfType<PlayerRewind>();
        rewindSound.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerRewind != null)
        {
            mainTheme.Pause();
            rewindSound.Play();
            
            // Subscribe to event
            playerRewind.OnRewindComplete += HandleRewindComplete;
            playerRewind.StartRewind();
        }
    }

    void HandleRewindComplete()
    {
        rewindSound.Stop();
        mainTheme.Play();
        
        // Unsubscribe
        if (playerRewind != null)
            playerRewind.OnRewindComplete -= HandleRewindComplete;
    }

    void OnDestroy()
    {
        // Clean up event subscription
        if (playerRewind != null)
            playerRewind.OnRewindComplete -= HandleRewindComplete;
    }
}