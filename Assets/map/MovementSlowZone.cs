using UnityEngine;

public class MovementSlowZone : MonoBehaviour
{
    public float slowMultiplier = 0.5f;
    public string playerTag = "Player";

    private PlayerMovement affectedPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            affectedPlayer = other.GetComponentInParent<PlayerMovement>();
            if (affectedPlayer != null)
            {
                affectedPlayer.moveSpeed = affectedPlayer.baseMoveSpeed * slowMultiplier;

                if (affectedPlayer.animator != null)
                    affectedPlayer.animator.speed = slowMultiplier;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            ResetPlayerSpeed();
        }
    }

    public void ResetPlayerSpeed()
    {
        if (affectedPlayer != null)
        {
            affectedPlayer.moveSpeed = affectedPlayer.baseMoveSpeed;

            if (affectedPlayer.animator != null)
                affectedPlayer.animator.speed = 1f;

            affectedPlayer = null;
        }
    }
}
