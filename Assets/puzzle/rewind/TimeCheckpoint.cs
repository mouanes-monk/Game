using UnityEngine;

public class TimeCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRewind rewind = other.GetComponent<PlayerRewind>() ?? other.GetComponentInParent<PlayerRewind>();
            
            if (rewind != null)
            {
                rewind.lastCheckpoint = transform; // ✅ Update last checkpoint
                Debug.Log("✅ Checkpoint Updated: " + transform.position);
            }
            else
            {
                Debug.LogWarning("⚠ PlayerRewind not found on Player or Parent!");
            }
        }
    }
}
